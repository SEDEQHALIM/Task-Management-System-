using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public class WorkSpaceRepository : Repository<WorkSpace> ,IWorkSpaceRepository
    {
        private readonly TMContextDB _context;

        public WorkSpaceRepository(TMContextDB context) : base(context)
        {
            _context = context;

        }

        public async Task<List<WorkSpace>> GetAllWorkSpaceByOwnerId(string id)
        {
            
            return await _context.WorkSpaces.
                                           Where(e => e.OwnerID.Equals(id))
                                           .Include(i => i.Tasks).Include(m=>m.Messages).Include(i => i.Members).ThenInclude(i => i.User)
                                           .ToListAsync(); ;
        }

        public async Task<List<WorkSpace>> GetAllWorkSpaceThatSharedWithMe(string userId)
        {
            return await _context.WorkSpaces.Include(e=>e.Messages)
          .Include(w => w.Members)
          .ThenInclude(m => m.User)
          .Where(w => w.Members.Any(m => m.User.Id == userId))
          .ToListAsync(); 
            
        }

        public async Task<WorkSpace> GetByOwnerId(string id)
        {
            WorkSpace? data =  await _context.WorkSpaces
                .Include(w => w.Tasks).Include(m=>m.Messages)     
                .Include(w => w.Members).ThenInclude(m=>m.User)
                .FirstOrDefaultAsync(w => w.Id.Equals(id));


            return data;
        }

        public async Task<WorkSpace> GetByOwnerIdAndWorkSpcaeId(string ownerId, string worksapceId)
        {
            return await _context.WorkSpaces.Include(m => m.Messages).Include(i => i.Members).ThenInclude(i => i.User).FirstOrDefaultAsync(e=>e.OwnerID.Equals(ownerId) && e.Id.Equals(worksapceId));

        }
    }
}
