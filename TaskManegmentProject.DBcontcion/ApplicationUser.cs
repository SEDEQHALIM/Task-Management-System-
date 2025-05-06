using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TaskManegmentProject.DBcontcion
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
