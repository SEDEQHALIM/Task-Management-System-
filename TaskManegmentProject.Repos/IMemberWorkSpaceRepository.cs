
using TaskManegmentProject.DBcontcion;
namespace TaskManegmentProject.Repos
{
    public interface IMemberWorkSpaceRepository:IRepository<MemberWorkSpace>
    {

        Task<bool> IsExistMemberInWorkSpace(string worksSpaceId, string email); 



    }
}
