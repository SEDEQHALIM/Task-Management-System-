using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManegmentProject.DBcontcion
{
    public class MessageMentions:GeneralEntity
    {

        
        public string MessageId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("MessageId")]
        public MessageChat Message { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User{ get; set; }

    }
}
