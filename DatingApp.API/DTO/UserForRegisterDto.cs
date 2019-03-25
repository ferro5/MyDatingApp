﻿using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserForRegisterDto
    {
        [Required]
        [EmailAddress]
        public    string Email { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
     
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
