using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public interface ITaskRepository :IRepository<MyTask>
    {
        IQueryable<MyTask> GetTasksByWorkSpace(string workSpaceId);
        IQueryable<MyTask> GetTasksByUser(string userId);

        Task<List<MyTask>> GetTasksByStatusAndUserIdAsync(int status, string userId, string workSpaceId);
    }
}
