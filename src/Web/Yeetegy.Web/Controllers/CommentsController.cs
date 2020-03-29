﻿using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Yeetegy.Services.Data.Interfaces;
using Yeetegy.Web.ViewModels;

namespace Yeetegy.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller // removed Base
    {
        private readonly ICommentsService commentsService;
        private readonly IPostsService postsService;

        public CommentsController(ICommentsService commentsService, IPostsService postsService)
        {
            this.commentsService = commentsService;
            this.postsService = postsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string postId, int offset)
        {
            if (await this.postsService.DoesPostExistAsync(postId))
            {
                var comments = await this.commentsService.GetCommentsAsync<CommentsViewModel>(postId, offset, 10);

                return this.Json(comments);
            }

            return this.NotFound();
        }

        // [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseAddComment>> Post([FromForm]AddCommentsModel data)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await this.commentsService.CreateCommentAsync(data, userId);

                return new ResponseAddComment() { Status = "Created" };
            }

            return this.Unauthorized();
        }
    }
}
