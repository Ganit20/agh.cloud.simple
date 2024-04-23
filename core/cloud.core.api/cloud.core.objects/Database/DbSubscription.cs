using System;
using System.ComponentModel.DataAnnotations;

namespace cloud.core.objects.Database
{
	public class DbSubscription
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public double MaximmumSpace { get; set; }
		public virtual List<DbUser> Users { get; set; }
	}
}

