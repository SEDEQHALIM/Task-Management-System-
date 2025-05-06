using Microsoft.EntityFrameworkCore;
using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public class MemberWorkSpaceRepository:Repository<MemberWorkSpace> , IMemberWorkSpaceRepository
    {

        private readonly TMContextDB _context;

        public MemberWorkSpaceRepository(TMContextDB context) : base(context)
        {
            _context = context;

        }


        public async Task<bool> IsExistMemberInWorkSpace(string workspace,string email)
        {

            MemberWorkSpace isExist = await _context.MemberWorkSpace.Include(e=>e.User).FirstOrDefaultAsync(e=>e.WorkSpaceID.Equals(workspace) && e.User.Email.Equals(email));


            if (isExist !=null)
            {
                return true;
            }

            return false;
        }

    }
}
