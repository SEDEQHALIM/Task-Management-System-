
using TaskManegmentProject.DBcontcion.ViewModels;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManegmentProject.DBcontcion;
using TaskManegmentProject.Models;

using TaskManegmentProject.Repos;

namespace TaskManegmentProject.Controllers;

[Authorize]
public class MySharedController : Controller
{
	private readonly ITaskRepository _taskRepository;
	private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
	private readonly INotificationRepositry _notificationRepository;

	private readonly IWorkSpaceRepository _workSpaceRepository;
    private readonly IMemberWorkSpaceRepository _memberWorkSpaceRepository;
	public MySharedController(
		ITaskRepository taskRepository,
		UserManager<ApplicationUser> userManger,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger,
        IWorkSpaceRepository workSpaceRepository,
		INotificationRepositry notificationRepository,
		IMemberWorkSpaceRepository memberWorkSpaceRepository)
    {
		_taskRepository = taskRepository;

		_userManager = userManger;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _logger = logger;
		_notificationRepository = notificationRepository;

		_workSpaceRepository = workSpaceRepository;

        _memberWorkSpaceRepository = memberWorkSpaceRepository;
    }

    [HttpGet]
	public async Task<IActionResult> Index(string id, int status = -1)
	{
		ApplicationUser authUser = await _userManager.GetUserAsync(User);
		List<WorkSpace> workData = await _workSpaceRepository.GetAllWorkSpaceThatSharedWithMe(authUser.Id);


		if (workData.Count == 0) {
			return View("NotSharedWorkSpaces");
		}


		ViewData["workSpaceList"] = workData;

		if (id == null)
		{
			id = workData[0].Id;
		}

		WorkSpace selectedWorkSpace = workData.Find(e => e.Id.Equals(id));

		if (selectedWorkSpace != null)
		{
			var tasks = await _taskRepository.GetTasksByStatusAndUserIdAsync(status, authUser.Id, selectedWorkSpace.Id);

			List<Notification> notificationsWorkSpace = await _notificationRepository.GetAllByWorkSpaceId(id);
			ViewData["NotifcationsList"] = notificationsWorkSpace;
			ViewData["SelectedWorkSpace"] = selectedWorkSpace;

			var model = new WorkSpaceWithTasksViewModel
			{
				WorkSpace = selectedWorkSpace,
				Tasks = tasks,
				Message = selectedWorkSpace.Messages,
				Members = selectedWorkSpace.Members
				

			};

			return View("Shared", model);
		}

		return View("Shared");
	}




   
 
}

