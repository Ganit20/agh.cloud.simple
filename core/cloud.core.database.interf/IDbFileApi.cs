using System;
using cloud.core.objects.Database;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace cloud.core.database.interf
{
	public interface IDbFileApi
	{
        [Post("addFile")]
        public  Task AddFile(int userId, double fileSize);

        [Delete("removeFile")]
        public Task RemoveFile(int userId, double fileSize);
        [Get("getUserFileData/{userId}")]
        public  Task GetUserFileData([Path]int userId);

    }
}

