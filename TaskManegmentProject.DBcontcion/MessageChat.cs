using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManegmentProject.DBcontcion
{
    public class MessageChat :GeneralEntity
    {
       

        public string SenderId { get; set; }

        public string Content { get; set; }

        public string WorkSpaceId { get; set; }


        [ForeignKey("WorkSpaceId")]
        public WorkSpace WorkSpace { get; set; }

        [ForeignKey("SenderId")]
        public ApplicationUser User { get; set; }
    }
}
