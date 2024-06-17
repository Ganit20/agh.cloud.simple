using System;
using System.ComponentModel.DataAnnotations;

namespace cloud.core.objects.Database
{
	public class DbUserFilesData
	{
		[Key]
		public int UserId { get; set; }
		public double SpaceUsed { get; set; }
		public int FileSaved { get; set; }
        public int SpaceLimit { get; set; }

        public virtual DbUser User { get; set; }
	}
}

