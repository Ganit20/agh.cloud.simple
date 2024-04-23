using System;
using cloud.core.objects.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloud.core.objects.Model
{
	public class User
	{
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreateDate { get; set; }
        public int SubscriptionId { get; set; }
        public int StatusId { get; set; }

    }
}

