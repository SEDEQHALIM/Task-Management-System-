using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManegmentProject.DBcontcion.ViewModels
{
    public class ChatWorkSpaceViewModel
    {
        public  List<MessageChat> messages { get; set; }
        public List<MemberWorkSpace> members { get; set; }
    }
}
