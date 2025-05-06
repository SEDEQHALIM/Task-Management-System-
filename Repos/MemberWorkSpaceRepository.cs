using TaskManegmentProject.Models;

namespace TaskManegmentProject.Repos
{
    public class MemberWorkSpaceRepository:Repository<MemberWorkSpace> , IMemberWorkSpaceRepository
    {

        private readonly TMContextDB _context;

        public MemberWorkSpaceRepository(TMContextDB context) : base(context)
        {
            _context = context;

        }

    }
}
