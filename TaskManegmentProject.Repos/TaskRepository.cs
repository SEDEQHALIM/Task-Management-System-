using Microsoft.EntityFrameworkCore;
using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public class TaskRepository : Repository<MyTask> , ITaskRepository
    {

        private readonly TMContextDB _context;

        public TaskRepository(TMContextDB context) : base(context)
        {
            _context = context;
            
        }

        public IQueryable<MyTask> GetTasksByWorkSpace(string workSpaceId)
        {
            return _context.MyTask
                .Where(t => !string.IsNullOrEmpty(workSpaceId) && t.WorkSpaceId == workSpaceId);
        }

        public IQueryable<MyTask> GetTasksByUser(string userId)
        {
            return _context.MyTask
                .Where(t => !string.IsNullOrEmpty(userId) &&
                           (t.CreatedBy.Equals(userId) || t.AssignTo.Equals(userId)));
        }


        public async Task<List<MyTask>> GetTasksByStatusAndUserIdAsync(int status, string userId, string workSpaceId)
        {
            IQueryable<MyTask> tasks = _context.MyTask.Where(t => t.AssignTo == userId && t.WorkSpaceId == workSpaceId);

            if (status >= 0)
            {
                tasks = tasks.Where(t => (int)t.Status == status);
            }

            return await tasks.ToListAsync();
        }



    }
}
