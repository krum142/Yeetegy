﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yeetegy.Services.Data.Interfaces;
using Yeetegy.Web.ViewModels;
using Yeetegy.Web.ViewModels.PostViewModels;

namespace Yeetegy.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoryService categoryService;

        public PostsController(IPostsService postsService, ICategoryService categoryService)
        {
            this.postsService = postsService;
            this.categoryService = categoryService;
        }

        public IActionResult GetPost()
        {
            var cookie = this.Request.Cookies.FirstOrDefault(c => c.Key == "IdCookie"); // potential ddos attack

            var header = this.Request.Headers.FirstOrDefault(x => x.Key == "x-category").Value.ToString().Trim();
            // you can use a swtich here could be faster
            if (string.IsNullOrWhiteSpace(header) || header.ToLower() == "newest")
            {
                var posts = this.postsService.GetFivePostsLatest(int.Parse(cookie.Value));

                if (posts.Any())
                {
                    this.Response.Cookies.Append("IdCookie", (int.Parse(cookie.Value) + 5).ToString());
                }

                return this.Json(JsonConvert.SerializeObject(posts));
            }

            if (this.categoryService.IsThereAny(header))
            {
                var posts = this.postsService.GetFivePostsCategory(int.Parse(cookie.Value),header);

                if (posts.Any())
                {
                    this.Response.Cookies.Append("IdCookie", (int.Parse(cookie.Value) + 5).ToString());
                }

                return this.Json(JsonConvert.SerializeObject(posts));
            }

            return this.Json(JsonConvert.SerializeObject(new List<PostsViewModel>()));
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new AddPostsViewModel()
            {
                Categorys = this.categoryService.GetAllListItems(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPostsViewModel post)
        {
            if (post.File != null && post.Tittle != null)
            {
                var checkCategory = this.categoryService.IsThereAny(post.Category);

                if (checkCategory && this.ModelState.IsValid)
                {
                    var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                    await this.postsService.CreatePostAsync(post, user);

                    return this.Redirect("/");
                }
            }

            post.Categorys = this.categoryService.GetAllListItems();

            return this.View(post);
        }
    }
}