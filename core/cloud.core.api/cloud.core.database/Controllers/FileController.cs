using System;
using cloud.core.database.DbContexts;
using cloud.core.database.interf;
using cloud.core.objects.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cloud.core.database.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FileController:ControllerBase, IDbFileApi
    {
        private readonly CloudDbContext _context;

        public FileController(CloudDbContext context)
        {
            _context = context;
        }

        [HttpPost("addFile")]
        public async Task AddFile(int userId, double fileSize)
        {
            var userFileData = await _context.UserFilesData.FirstOrDefaultAsync(u => u.UserId == userId);

            if (userFileData == null)
            {
                userFileData = new DbUserFilesData
                {
                    UserId = userId,
                    SpaceUsed = fileSize,
                    FileSaved = 1
                };
                _context.UserFilesData.Add(userFileData);
            }
            else
            {
                userFileData.SpaceUsed += fileSize;
                userFileData.FileSaved += 1;
                _context.UserFilesData.Update(userFileData);
            }

            await _context.SaveChangesAsync();
        }

        [HttpDelete("removeFile")]
        public async Task RemoveFile(int userId, double fileSize)
        {
            var userFileData = await _context.UserFilesData.FirstOrDefaultAsync(u => u.UserId == userId);

            if (userFileData == null)
            {
                throw new ArgumentException();
            }

            userFileData.SpaceUsed -= fileSize;
            userFileData.FileSaved -= 1;

            if (userFileData.FileSaved < 0)
            {
                userFileData.FileSaved = 0;
            }

            if (userFileData.SpaceUsed < 0)
            {
                userFileData.SpaceUsed = 0;
            }

            _context.UserFilesData.Update(userFileData);
            await _context.SaveChangesAsync();

        }

        [HttpGet("getUserFileData/{userId}")]
        public async Task GetUserFileData(int userId)
        {
            var userFileData = await _context.UserFilesData.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}


