using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TaskManegmentProject.Enums;

namespace TaskManegmentProject.DBcontcion
{
    public class MemberWorkSpace:GeneralEntity
    {

        public string WorkSpaceID { get; set; }
        public string OwnerID { get; set; }
        public MemberRole MemberRole { get; set; } = MemberRole.Editor;
        public DateTime JoinAt { get; set; } = DateTime.UtcNow;
        public string? RoleID { get; set; }


        [ForeignKey("WorkSpaceID")]
        public WorkSpace WorkSpace { get; set; }

        [ForeignKey("OwnerID")]
        public ApplicationUser User { get; set; }

        [ForeignKey("RoleID")]
        public IdentityRole Role { get; set; }

    }
}
