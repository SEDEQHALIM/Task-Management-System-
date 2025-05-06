using TaskManegmentProject.Models;

namespace TaskManegmentProject.Repos
{
    public interface ITaskRepository :IRepository<MyTask>
    {
        IQueryable<MyTask> GetTasksByWorkSpace(string workSpaceId);
        IQueryable<MyTask> GetTasksByUser(string userId);
    }
}
