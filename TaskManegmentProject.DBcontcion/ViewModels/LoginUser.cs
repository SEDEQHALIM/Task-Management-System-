using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManegmentProject.DBcontcion.ViewModels
{

    public class LoginUser
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Adress")]
        public string  Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password Is Required")]
        [StringLength(50,MinimumLength =6,ErrorMessage ="Password must be between 6 to 50 char")]
        public string  Password { get; set; }
    }
}
