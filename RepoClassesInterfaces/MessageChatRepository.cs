using TaskManegmentProject.Models;

namespace TaskManegmentProject.Repositiory
{
    public class MessageChatRepository: Repository<MessageChat> , IMessageChatRepository
    {

        private readonly TMContextDB _context;

        public MessageChatRepository(TMContextDB context) : base(context)
        {
            _context = context;

        }
    }
}
