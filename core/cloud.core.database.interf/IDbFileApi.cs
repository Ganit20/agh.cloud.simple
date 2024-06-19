using System;
using cloud.core.objects.Database;
using cloud.core.objects.Model;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace cloud.core.database.interf
{
    [BasePath("api/file")]

    public interface IDbFileApi
    {
        [Post("addFile")]
        public Task AddFile(int userId, double fileSize);

        [Delete("removeFile")]
        public Task RemoveFile(int userId, double fileSize);
        [Get("shared/{id}")]
        public Task<FileShareLink> GetSharedFile([Path] Guid id);
        [Post("share")]
        public Task<Guid> CreateShareLink([Body] FileShareRequest request);
        [Get("shared/all/{id}")]
        public Task<List<FileShareLink>> GetAllSharedFiles([Path] int id);
        [Get("shared/deactivate/{id}")]
        public Task DeactivateLink([Path] Guid id);
        
    }
}

