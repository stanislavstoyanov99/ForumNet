﻿namespace ForumNet.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Data;
    using Data.Models;

    public class RepliesService : IRepliesService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public RepliesService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string description, int postId, string authorId)
        {
            var reply = new Reply
            {
                Description = description,
                CreatedOn = this.dateTimeProvider.Now(),
                ModifiedOn = this.dateTimeProvider.Now(),
                PostId = postId,
                AuthorId = authorId
            };

            await this.db.Replies.AddAsync(reply);
            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string description)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id);

            reply.Description = description;
            reply.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id);

            reply.IsDeleted = true;
            reply.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
        {
            var reply = await this.db.Replies
                .Where(r => r.Id == id)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return reply;
        }

        public async Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId)
        {
            var replies = await this.db.Replies
                .Where(r => r.PostId == postId)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return replies;
        }
    }
}