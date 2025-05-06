using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManegmentProject.DBcontcion;
using TaskManegmentProject.DBcontcion.ViewModels;
using TaskManegmentProject.Enums;
using TaskManegmentProject.MyHubs;
using TaskManegmentProject.Repos;

namespace TaskManegmentProject.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IWorkSpaceRepository _workSpaceRepository;
        private readonly INotificationRepositry _notificationRepository;
        private readonly IHubContext<NotifcationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<StatusHub> _statusHub;

        public TaskController(
            ITaskRepository taskRepository,
            IWorkSpaceRepository workSpaceRepository, 
            INotificationRepositry notificationRepository,
            IHubContext<NotifcationHub> hubContext,
            UserManager<ApplicationUser> userManager,
            IHubContext<StatusHub> statusHub
            )
        {
            _taskRepository = taskRepository;
            _workSpaceRepository = workSpaceRepository;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _userManager = userManager;
            _statusHub = statusHub;
        }
        public async Task<IActionResult> Add(string workSpaceId)
        {

            ApplicationUser userData = await _userManager.GetUserAsync(User);

            WorkSpace workSpace = await _workSpaceRepository.GetByOwnerIdAndWorkSpcaeId(userData.Id, workSpaceId);
            if (workSpace == null)
            {
                return NotFound();
            }

            ViewData["membersList"] = workSpace.Members;
            TaskViewModel model = new TaskViewModel()
            {
                WorkSpaceId = workSpace.Id,

            };

            return View("Add", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ApplicationUser userData = await _userManager.GetUserAsync(User);

                WorkSpace workSpace = await _workSpaceRepository.GetByOwnerIdAndWorkSpcaeId(userData.Id,viewModel.WorkSpaceId);

                ViewData["membersList"] = workSpace.Members;

                return View(viewModel);
            }
            ApplicationUser getUser = await _userManager.GetUserAsync(User);

            MyTask newTask = new MyTask()
            {
                CreatedBy = getUser.Id,
                WorkSpaceId = viewModel.WorkSpaceId,
                Title = viewModel.Title,
                Description = viewModel.Description,
                Status = viewModel.Status,
                Priority = viewModel.Priority,
                AssignTo = viewModel.AssignTo
            };
            Notification newNotification = new Notification()
            {
                UserId = getUser.Id,
                WorkspaceId = viewModel.WorkSpaceId,
                Action = Enums.NotificationAction.TaskCreated,
                IsReaded = false
            };

            await _notificationRepository.CreateAsync(newNotification);
            Notification newNotificationData = 
                await _notificationRepository.
                GetNotificationByIdAsync(newNotification.Id);


            await _hubContext.Clients.All.SendAsync("ReceiveNotification", newNotificationData);
            await _taskRepository.CreateAsync(newTask);
            await _taskRepository.SaveAsync();

            return RedirectToAction("Index", "Home", new
            {
                id = viewModel.WorkSpaceId
            });
        }
    

        public async Task<IActionResult> Edit(string id)
        {

            MyTask task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            ApplicationUser userData = await _userManager.GetUserAsync(User);
            if (userData == null)
            {
                return Unauthorized();
            }

          
            WorkSpace workSpace = await _workSpaceRepository.GetByOwnerIdAndWorkSpcaeId(userData.Id, task.WorkSpaceId);
            if (workSpace == null)
            {
                return NotFound();
            }

            TaskViewModel model = new TaskViewModel
            {
                Id = task.Id,
                WorkSpaceId = task.WorkSpaceId,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                AssignTo = task.AssignTo
            };

            ViewData["membersList"] = workSpace.Members;

            return View("Edit", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // جلب بيانات المستخدم الحالي
                ApplicationUser userData = await _userManager.GetUserAsync(User);
                if (userData == null)
                {
                    return Unauthorized();
                }

                // جلب الـ WorkSpace
                WorkSpace workSpace = await _workSpaceRepository.GetByOwnerIdAndWorkSpcaeId(userData.Id, viewModel.WorkSpaceId);
                if (workSpace == null)
                {
                    return NotFound();
                }

                // إعادة تمرير قايمة الأعضاء للـ View
                ViewData["membersList"] = workSpace.Members;

                return View(viewModel);
            }

            MyTask task = await _taskRepository.GetByIdAsync(viewModel.Id);
            if (task == null)
            {
                return NotFound();
            }

            ApplicationUser getUser = await _userManager.GetUserAsync(User);
            if (getUser == null)
            {
                return Unauthorized();
            }

            task.Title = viewModel.Title;
            task.Description = viewModel.Description;
            task.Status = viewModel.Status;
            task.Priority = viewModel.Priority;
            task.AssignTo = viewModel.AssignTo;
            
            await _taskRepository.UpdateAsync(task);
            Notification newNotification = new Notification
            {
                UserId = getUser.Id,
                WorkspaceId = viewModel.WorkSpaceId,
                TaskId = task.Id, 
                Action = NotificationAction.TaskUpdated,
                IsReaded = false
            };

            await _notificationRepository.CreateAsync(newNotification);
            Notification newNotificationData = await _notificationRepository.GetNotificationByIdAsync(newNotification.Id);

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", newNotificationData);
            await _statusHub.Clients.All.SendAsync("TaskStatusChanged",task.Id,task.Status.ToString()); 

            await _taskRepository.SaveAsync();

            return RedirectToAction("Index", "Home", new
            {
                id = viewModel.WorkSpaceId
            });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(string taskId)
        {

            ApplicationUser getUser = await _userManager.GetUserAsync(User);
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return NotFound();

            Notification newNotification = new Notification()
            {
                UserId = getUser.Id,
                WorkspaceId = task.WorkSpaceId,
                Action = Enums.NotificationAction.TaskDeleted,
                IsReaded = false
            };
            await _notificationRepository.CreateAsync(newNotification);
            //await _notificationRepository.SaveAsync();
            //List<Notification> allRelatedNot = await _notificationRepository.GetAllNotifcationByTaskId(taskId);

            await _taskRepository.DeleteAsync(taskId);
            await _taskRepository.SaveAsync();

            return RedirectToAction("Index", "Home", new { id = task.WorkSpaceId });



        }


    }
}
