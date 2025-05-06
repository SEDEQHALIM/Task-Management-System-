using TaskManegmentProject.Models;

namespace TaskManegmentProject.Repositiory
{
    public class WorkSpaceRepository : Repository<WorkSpace> ,IWorkSpaceRepository
    {
        private readonly TMContextDB _context;

        public WorkSpaceRepository(TMContextDB context) : base(context)
        {
            _context = context;

        }
    }
}
