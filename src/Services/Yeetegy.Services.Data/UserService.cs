﻿using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Yeetegy.Data.Common.Repositories;
using Yeetegy.Data.Models;
using Yeetegy.Services.Data.Interfaces;

namespace Yeetegy.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<UserPostVote> userLikePostRepository;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepository,IDeletableEntityRepository<UserPostVote> userLikePostRepository)
        {
            this.userRepository = userRepository;
            this.userLikePostRepository = userLikePostRepository;
        }

        public async Task AddPostAsync(Post post, string userId)
        {
            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.Id == userId);

            user.Posts.Add(post);

            await this.userRepository.SaveChangesAsync();
        }
    }
}