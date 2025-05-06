using Microsoft.EntityFrameworkCore;
using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public class MessageChatRepository: Repository<MessageChat> , IMessageChatRepository
    {

        private readonly TMContextDB _context;

        public MessageChatRepository(TMContextDB context) : base(context)
        {
            _context = context;

        }

        public async Task<List<MessageChat>> GetAllMessageByWorkSpaceId(string id)
        {
            return await _context.MessageChat.Include(i=>i.User).
                Where(e=>e.WorkSpaceId.Equals(id))
                .ToListAsync();
        }
    }
}
