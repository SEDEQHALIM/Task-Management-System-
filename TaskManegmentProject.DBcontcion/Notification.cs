using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManegmentProject.Enums;

namespace TaskManegmentProject.DBcontcion
{
    public class Notification :GeneralEntity
    {
        
        public string UserId { get; set; }
        public string? TaskId { get; set; }
        public string WorkspaceId { get; set; }

        public NotificationAction Action{ get; set; }
        public bool IsReaded { get; set; } 

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("TaskId")]
        public MyTask Task { get; set; }

        [ForeignKey("WorkspaceId")]
        public WorkSpace WorkSpace{ get; set; }




    }
}
