using System.ComponentModel.DataAnnotations.Schema;
using TaskManegmentProject.Enums;

namespace TaskManegmentProject.DBcontcion
{
    public class MyTask :GeneralEntity
    {
        public string CreatedBy { get; set; }
        public string WorkSpaceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string?  AssignTo { get; set; }



        [ForeignKey("CreatedBy")]
        public ApplicationUser User { get; set; }

        [ForeignKey("WorkSpaceId")]
        public WorkSpace WorkSpace { get; set; }

        [ForeignKey("AssignTo")]
        public ApplicationUser AssignUser { get; set; }

        public List<Notification> Notifications { get; set; }

    }
}
