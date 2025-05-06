using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManegmentProject.DBcontcion
{
    public class WorkSpace : GeneralEntity
    {
       public string Name { get; set; }

        public string OwnerID { get; set; }
       
        

        [ForeignKey("OwnerID")]
        public ApplicationUser Owner { get; set; }
        public List<MemberWorkSpace>? Members{ get; set; }
        public List<MyTask>? Tasks { get; set; }
        public List<MessageChat>? Messages { get; set; }


    }
}
