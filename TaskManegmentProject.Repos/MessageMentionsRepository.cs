using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManegmentProject.Repos
{
    public class MessageMentionsRepository : Repository<MessageMentionsRepository>, IMessageMentionsRepository
    {
        public MessageMentionsRepository(DBcontcion.TMContextDB DB) : base(DB)
        {
        }
    }
}
