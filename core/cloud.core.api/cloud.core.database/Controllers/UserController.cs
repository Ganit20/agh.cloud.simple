using System;
using AutoMapper;
using cloud.core.database.DbContexts;
using cloud.core.database.interf;
using cloud.core.objects.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace cloud.core.database.Controllers
{
	public class UserController:ControllerBase, IDbUserApi
    {
        private readonly CloudDbContext _context;
        private readonly IMapper _mapper;

        public UserController(CloudDbContext context,IMapper mapper)
		{
            _context = context;
            _mapper = mapper;
		}
        [HttpGet(IDbUserApi.GetUserByIdMail)]

        public async Task<User> GetUserById([Path] int id)
        {
           return  _mapper.Map<User>(await _context.Users.SingleOrDefaultAsync(x => x.Id == id));
        }
        [HttpGet(IDbUserApi.GetUserByIdMail)]

        public async Task<User> GetUserByMail([Path] string email)
        {
            return _mapper.Map<User>(await _context.Users.SingleOrDefaultAsync(x => x.Login.ToLower() == email.ToLower()));
        }
        [HttpPost(IDbUserApi.PostUser)]

        public async Task<User> Adduser([Path] AddUserRequest request)
        {
            var entity = await _context.Users.AddAsync(new objects.Database.DbUser()
            {
                CreateDate = DateTime.Now,
                Data = new objects.Database.DbUserFilesData()
                {
                    FileSaved = 0,
                    SpaceUsed = 0,
                },
                Login = request.Email,
                PasswordHash = request.PasswordHash,
                StatusId = 1,
                SubscriptionId = 0
            });
            await _context.SaveChangesAsync();
            return _mapper.Map<User>(entity.Entity);
        }
    }
}

