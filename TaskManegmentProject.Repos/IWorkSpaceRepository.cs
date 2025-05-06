using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public interface IWorkSpaceRepository :IRepository<WorkSpace>
    {
        Task<WorkSpace> GetByOwnerId(string id);
        Task<List<WorkSpace>> GetAllWorkSpaceByOwnerId(string id);
        Task<WorkSpace> GetByOwnerIdAndWorkSpcaeId(string ownerId, string worksapceId);

        Task<List<WorkSpace>> GetAllWorkSpaceThatSharedWithMe(string userId);
    }
}
