using System;
using System.ComponentModel.DataAnnotations;

namespace cloud.core.objects.Database
{
	public class DbFileShareLink
	{
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiryDate { get; set; }


        public bool IsActive { get; set; } = true;
    }
}

