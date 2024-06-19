using System;
using cloud.core.database.DbContexts;
using cloud.core.database.interf;
using cloud.core.objects.Database;
using cloud.core.objects.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestEase;

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

        [HttpPost("share")]
        public async Task<Guid> CreateShareLink([FromBody] FileShareRequest request)
        {
            var shareLink = new DbFileShareLink
            {
                Id = Guid.NewGuid(),
                FilePath = request.FilePath,
                CreatedAt = DateTime.UtcNow,
                ExpiryDate = request.ExpiryDate,
            };

            _context.FileShareLinks.Add(shareLink);
            await _context.SaveChangesAsync();
            return shareLink.Id;
        }

        [HttpGet("shared/{id}")]
        public async Task<FileShareLink> GetSharedFile(Guid id)
        {
            return (await _context.FileShareLinks.SingleOrDefaultAsync(f => f.Id == id && f.IsActive)).Adapt< FileShareLink>();

          
        }
        [HttpGet("shared/all/{id}")]
        public async Task<List<FileShareLink>> GetAllSharedFiles(int id)
        {
            return (await _context.FileShareLinks.Where(f => f.FilePath.StartsWith(id.ToString())).ToListAsync()).Adapt<List<FileShareLink>>();


        }
        [HttpGet("shared/deactivate/{id}")]
        public async Task DeactivateLink([Path] Guid id)
        {
            var link =await _context.FileShareLinks.FirstOrDefaultAsync(x => x.Id == id);
            link.IsActive = false;
            await _context.SaveChangesAsync();
        }

    }
}


