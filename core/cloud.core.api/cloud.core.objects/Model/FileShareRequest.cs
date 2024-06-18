using System;
namespace cloud.core.objects.Model
{
	public class FileShareRequest
	{
            public string FilePath { get; set; }
            public DateTime? ExpiryDate { get; set; }
    }
}

