using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManegmentProject.DBcontcion;

namespace TaskManegmentProject.Repos
{
    public class NotificationRepositry : Repository<Notification>, INotificationRepositry
    {
        private readonly TMContextDB _context;
        public NotificationRepositry(TMContextDB DB) : base(DB)
        {
            _context = DB;
        }

        public async Task<List<Notification>> GetAllByWorkSpaceId(string Id)
        {
            return await _context.Notification.
                Include(u => u.User).
                Where(e => e.WorkspaceId.Equals(Id)).OrderByDescending(e=>e.CreatedAt).
                ToListAsync();
        }

        public async Task<List<Notification>> GetAllNotifcationByTaskId(string id)
        {
            return await _context.Notification.Where(e => e.TaskId.Equals(id)).ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(string Id)
        {
            return await
                _context.
               Notification.
               Include(u => u.User).
               FirstOrDefaultAsync(e => e.Id.Equals(Id));
        }
    }
}
