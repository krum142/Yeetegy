﻿using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Yeetegy.Services.Data.Interfaces;
using Yeetegy.Web.ViewModels;

namespace Yeetegy.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplaysController : Controller
    {
        private readonly IReplaysService replaysService;
        private readonly ICommentsService commentsService;

        public ReplaysController(IReplaysService replaysService, ICommentsService commentsService)
        {
            this.replaysService = replaysService;
            this.commentsService = commentsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string commentId)
        {
            if (await this.commentsService.DoesCommentExistAsync(commentId))
            {
                var replays = await this.replaysService.AllAsync<ReplayViewModel>(commentId);

                return this.Json(replays);
            }

            return this.NotFound();
        }
    }
}
