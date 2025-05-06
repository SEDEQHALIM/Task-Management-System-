using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public interface INotificationRepositry:IRepository<Notification>
    {
        Task<Notification> GetNotificationByIdAsync(string Id);
        Task<List<Notification>> GetAllByWorkSpaceId(string Id);

        Task<List<Notification>> GetAllNotifcationByTaskId(string id);
    }
}
