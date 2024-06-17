﻿using System;
using System.ComponentModel.DataAnnotations;

namespace cloud.core.objects.Model
{
	public class LoginRequest
	{

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


}
}
