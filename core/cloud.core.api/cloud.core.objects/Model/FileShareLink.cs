using System;
using System.ComponentModel.DataAnnotations;

namespace cloud.core.objects.Model
{
	public class FileShareLink
	{
        
            public Guid Id { get; set; }

            public string FilePath { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime? ExpiryDate { get; set; }

            public bool IsActive { get; set; } = true;
        
    }
}

