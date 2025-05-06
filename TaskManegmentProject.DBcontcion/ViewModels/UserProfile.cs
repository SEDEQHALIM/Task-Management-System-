using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskManegmentProject.DBcontcion.ViewModels
{
   public class UserProfile
    {
        [Required]
        public string Id { get; set; }

        public string  Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IFormFile? ProfileImage { get; set; }

        [DataType(DataType.Password)]

        public string? Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]

        public string?  ConformPassword { get; set; }
        public string? Imageurl { get; set; }
    }
}
