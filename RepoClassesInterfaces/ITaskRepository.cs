using TaskManegmentProject.Models;

namespace TaskManegmentProject.Repositiory
{
    public interface ITaskRepository :IRepository<MyTask>
    {
        IQueryable<MyTask> GetTasksByWorkSpace(string workSpaceId);
        IQueryable<MyTask> GetTasksByUser(string userId);
    }
}
