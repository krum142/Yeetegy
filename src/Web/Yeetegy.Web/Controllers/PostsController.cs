﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Yeetegy.Services.Data;
using Yeetegy.Services.Data.Interfaces;
using Yeetegy.Web.ViewModels.PostViewModels;

namespace Yeetegy.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoryService categoryService;
        private readonly IConfiguration configuration;

        public PostsController(IPostsService postsService, ICategoryService categoryService, IConfiguration configuration)
        {
            this.postsService = postsService;
            this.categoryService = categoryService;
            this.configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            await categoryService.CreateAsync("2best category");

            var category = categoryService.GetAll();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPostsViewModel post)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await postsService.CreatePostAsync(post, user, configuration["CloudSettings"]);

            return Redirect("/");
        }
    }
}