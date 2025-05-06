using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManegmentProject.DBcontcion
{
    public class TaskAssignment :GeneralEntity
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("TaskId")]
        public MyTask Task { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
