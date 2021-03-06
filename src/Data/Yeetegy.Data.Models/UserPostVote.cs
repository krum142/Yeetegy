﻿using System;

using Yeetegy.Data.Common.Models;

namespace Yeetegy.Data.Models
{
    public class UserPostVote : BaseDeletableModel<string>
    {
        public UserPostVote()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string PostId { get; set; }

        public Post Post { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string Value { get; set; }
    }
}
