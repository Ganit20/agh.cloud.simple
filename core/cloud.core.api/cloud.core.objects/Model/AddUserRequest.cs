using System;
namespace cloud.core.objects.Model
{
	public class AddUserRequest
	{
		public string Email { get; set; }
		public string PasswordHash { get; set; }
	}
}

