using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloud.core.objects.Database
{
	public class DbUser
	{
		[Key]
		public int Id { get; set; }
		public string Login { get; set; }
		public string PasswordHash { get; set; }
		public DateTime CreateDate { get; set; }
		public int SubscriptionId { get; set; }
		public int StatusId { get; set; }
        public string? RefreshToken { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
		public virtual DbSubscription Subscription { get; set; }
		public virtual DbUserFilesData Data { get; set; }
	}
}

