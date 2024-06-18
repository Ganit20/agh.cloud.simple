using System;
using AutoMapper;
using cloud.core.database.DbContexts;
using cloud.core.database.interf;
using cloud.core.objects.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace cloud.core.database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController:ControllerBase, IDbUserApi
    {
        private readonly CloudDbContext _context;

        public UserController(CloudDbContext context)
		{
            _context = context;
		}
        [HttpGet(IDbUserApi.GetUserByIdPath)]

        public async Task<User> GetUserById( int id)
        {
           return  (await _context.Users.SingleOrDefaultAsync(x => x.Id == id)).Adapt<User>();
        }
        [HttpGet(IDbUserApi.GetUserByMailPath)]

        public async Task<User> GetUserByMail( string email)
        {
            return (await _context.Users.SingleOrDefaultAsync(x => x.Login.ToLower() == email.ToLower())).Adapt<User>();
        }
        [HttpPut(IDbUserApi.PutSaveRefreshTokenPath)]
        public async Task PutSaveRefreshToken( string refreshToken, int id)
        {
            var user =await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            user.RefreshToken = refreshToken;
            await _context.SaveChangesAsync();
        }
        [HttpPost(IDbUserApi.PostUser)]

        public async Task<User> Adduser([FromBody] AddUserRequest request)
        {
            var entity = await _context.Users.AddAsync(new objects.Database.DbUser()
            {
                CreateDate = DateTime.UtcNow,
                Data = new objects.Database.DbUserFilesData()
                {
                    FileSaved = 0,
                    SpaceUsed = 0,
                },
                Login = request.Email,
                PasswordHash = request.PasswordHash,
                StatusId = 1,
                SubscriptionId = 1
            });
            await _context.SaveChangesAsync();
            return (entity.Entity).Adapt<User>();
        }
        [HttpGet(IDbUserApi.GetUserFileInfoPath)]
        public async Task<UserFileInfo> GetUserFileInfo([Path] int id)
        {
            var user = await _context.Users.Include(x => x.Subscription).Include(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);
            return new UserFileInfo() { FileSaved = user.Data.FileSaved, MaxCapacity = user.Subscription.MaximmumSpace, SpaceUsed = user.Data.SpaceUsed, SubscriptionName = user.Subscription.Name };
        }

    }
}

