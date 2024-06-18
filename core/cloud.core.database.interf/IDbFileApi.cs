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
        public  Task AddFile(int userId, double fileSize);

        [Delete("removeFile")]
        public Task RemoveFile(int userId, double fileSize);
        [Get("shared/{id}")]
        public Task<FileShareLink> GetSharedFile(Guid id);
        [Post("share")]
        public Task<Guid> CreateShareLink([Body] FileShareRequest request);
    }
}

