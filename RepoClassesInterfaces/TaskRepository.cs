using TaskManegmentProject.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManegmentProject.Repositiory
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
                           (t.CreatedBy == userId || t.TaskAssignments.Any(ta => ta.UserId == userId)));
        }

       
    }
}
