using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManegmentProject.DBcontcion.ViewModels
{
	public class WorkSpaceWithTasksViewModel
	{
		public WorkSpace WorkSpace { get; set; }
		public List<MyTask> Tasks { get; set; }

        public List <MemberWorkSpace> Members { get; set; }
        public List<MessageChat> Message { get; set; }
    }
}
