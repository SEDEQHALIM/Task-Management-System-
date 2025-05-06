using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManegmentProject.DBcontcion.ViewModels
{
  public  class RegiesterUser
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="Name must be betwwen 3 to 50 char ")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invaild Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password Is Required")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 to 50 char")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Conform Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and confirmation password do not match")]
        public string ConformPassword { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
