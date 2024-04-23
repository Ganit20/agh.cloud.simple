using System;
using Microsoft.AspNetCore.Http;

namespace cloud.core.objects.Model
{
	public class UploadFileRequest
	{
		public string File { get; set; }
		public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }
}

