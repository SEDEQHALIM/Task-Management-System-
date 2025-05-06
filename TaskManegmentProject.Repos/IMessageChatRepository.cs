using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public interface IMessageChatRepository : IRepository<MessageChat>
    {
        Task<List<MessageChat>> GetAllMessageByWorkSpaceId(string id);
    }
}
