using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManegmentProject.DBcontcion;
using TaskManegmentProject.Models;
using TaskManegmentProject.MyHubs;
using TaskManegmentProject.Repos;

namespace TaskManegmentProject.Controllers;

[Authorize]
public class HomeController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IWorkSpaceRepository _workSpaceRepository;
    private readonly IMemberWorkSpaceRepository _memberWorkSpaceRepository;
    private readonly INotificationRepositry _notificationRepository;
    private readonly IMessageChatRepository _messageChatRepository;
    private readonly IHubContext<NotifcationHub> _hubContext;
    private readonly IHubContext<Chat> _chatHubContext;

    public HomeController(
        UserManager<ApplicationUser> userManger,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger,
        IWorkSpaceRepository workSpaceRepository,
        IMemberWorkSpaceRepository memberWorkSpaceRepository,
        INotificationRepositry notificationRepository,
        IMessageChatRepository messageChatRepository,
        IHubContext<NotifcationHub> hubContext,
        IHubContext<Chat> chatHubContext

        )
    {
        _userManager = userManger;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _logger = logger;
        _workSpaceRepository = workSpaceRepository;
        _memberWorkSpaceRepository = memberWorkSpaceRepository;
        _notificationRepository = notificationRepository;
        _messageChatRepository = messageChatRepository;
        _hubContext = hubContext;
        _chatHubContext = chatHubContext;
    }

    [HttpGet]

    public async Task<IActionResult> Index(string id)
    {

        ApplicationUser authUser = await _userManager.GetUserAsync(User);
        List<WorkSpace> WorkData =
            await _workSpaceRepository.GetAllWorkSpaceByOwnerId(authUser.Id);
        ViewData["workSpaceList"] = WorkData;

        if (id == null)
        {
            if (WorkData != null && WorkData.Count() != 0)
            {
                id = WorkData[0].Id;
                List<Notification> notificationsWorkSpace = await _notificationRepository
                    .GetAllByWorkSpaceId(id);

                List<MessageChat> messageChats = await _messageChatRepository.GetAllMessageByWorkSpaceId(id);
                List<MemberWorkSpace> workSpacesMembers = WorkData[0].Members;

                ViewData["NotifcationsList"] = notificationsWorkSpace;
                ViewData["SelectedWorkSpace"] = WorkData[0];
                ViewData["messages"] = WorkData[0].Messages ;
                ViewData["members"] = WorkData[0].Members;

                return View("Index", WorkData[0]);
            }
            else
            {
                WorkSpace work = new WorkSpace()
                {
                    Name = "Frist Work Space",
                    OwnerID = authUser.Id,

                };
                await _workSpaceRepository.CreateAsync(work);
                await _workSpaceRepository.SaveAsync();

                WorkSpace fristWorkSpace = await _workSpaceRepository.GetByOwnerId(authUser.Id);
                List<Notification> notificationsWorkSpace = await _notificationRepository
                .GetAllByWorkSpaceId(work.Id);
                ViewData["NotifcationsList"] = notificationsWorkSpace;
                ViewData["SelectedWorkSpace"] = work.Id;
                ViewData["messages"] = work.Messages;
                ViewData["members"] = work.Members;
                return View("Index", fristWorkSpace);
            }
        }

        if (WorkData != null && WorkData.Count() != 0)
        {
            WorkSpace selectedWorSpace = WorkData.Find(e => e.Id.Equals(id));

            if (selectedWorSpace != null)
            {

                List<Notification> notificationsWorkSpace = await
                        _notificationRepository.GetAllByWorkSpaceId(selectedWorSpace.Id);
                ViewData["NotifcationsList"] = notificationsWorkSpace;
                ViewData["SelectedWorkSpace"] = selectedWorSpace;
                ViewData["messages"] = selectedWorSpace.Messages;
                ViewData["members"] = selectedWorSpace.Members;
                return View("Index", selectedWorSpace);
            }

        }

        return View("Index");




    }

    [HttpPost]
    public async Task<IActionResult> AddWorkSpace(string workSpaceName)
    {
        ApplicationUser getUser = await _userManager.GetUserAsync(User);
        if (getUser == null)
        {
            return Unauthorized();
        }
        if (string.IsNullOrEmpty(workSpaceName))
        {
            return BadRequest(new { success = false, message = "WorkSpace name cannot be empty." });
        }

        WorkSpace newWork = new WorkSpace()
        {
            Name = workSpaceName,
            OwnerID = getUser.Id

        };
        await _workSpaceRepository.CreateAsync(newWork);
        await _workSpaceRepository.SaveAsync();

        return Json(new
        {
            success = true,
            workSpace = new
            {
                id = newWork.Id,
                name = newWork.Name
            }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMemberToWorkSpace(string workSpaceId, string email)
    {

        if (workSpaceId == null || email == null)
        {
            return BadRequest(new
            {
                message = "Invalid Data"
            });
        }

        ApplicationUser getUserByEmail = await _userManager.FindByEmailAsync(email);

        if (getUserByEmail == null)
        {
            return NotFound(new
            {
                message = "Invalid User Data"
            });
        }

        bool isExist = await _memberWorkSpaceRepository.IsExistMemberInWorkSpace(workSpaceId, email);

        if (isExist)
        {
            return BadRequest(new
            {
                message = "User Already Exist"
            });
        }

        ApplicationUser getLoginId = await _userManager.GetUserAsync(User);

        MemberWorkSpace addNewMember = new MemberWorkSpace()
        {

            OwnerID = getUserByEmail.Id,
            WorkSpaceID = workSpaceId,

        };

        Notification newNotification = new Notification()
        {
            UserId = getLoginId.Id,
            WorkspaceId = workSpaceId,
            Action = Enums.NotificationAction.MemberAdded,
            IsReaded = false
        };

        await _memberWorkSpaceRepository.CreateAsync(addNewMember);
        await _notificationRepository.CreateAsync(newNotification);

        await _memberWorkSpaceRepository.SaveAsync();

        Notification newNotificationData = await _notificationRepository.GetNotificationByIdAsync(newNotification.Id);

        await _hubContext.Clients.All.SendAsync("ReceiveNotification", newNotificationData);

        return Ok(new
        {
            message = "Data Reached Successfuly",
            data = new
            {
                email = getUserByEmail.Email,
                imageUrl = getUserByEmail.ImageUrl,
                name = getUserByEmail.Name,
                id = getUserByEmail.Id
            }

        });
    }

    [HttpDelete("/home/DeleteMemberFromWorkSpace/{id}")]
    public async Task<IActionResult> DeleteMemberFromWorkSpace(string id)
    {
        MemberWorkSpace member = await _memberWorkSpaceRepository.GetByIdAsync(id);
        if (member != null)
        {
            ApplicationUser getLoginId = await _userManager.GetUserAsync(User);


            Notification newNotification = new Notification()
            {
                UserId = getLoginId.Id,
                WorkspaceId = member.WorkSpaceID,
                Action = Enums.NotificationAction.MemberRemoved,
                IsReaded = false
            };
            await _memberWorkSpaceRepository.DeleteAsync(member.Id);
            await _notificationRepository.CreateAsync(newNotification);

            await _memberWorkSpaceRepository.SaveAsync();


            Notification newNotificationData = await _notificationRepository.GetNotificationByIdAsync(newNotification.Id);

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", newNotificationData);
            return Json(new
            {
                message = "Deleted Successfully"
            });
        }
        return BadRequest(new
        {
            message = "Error: Member not found"
        });
    }

    [HttpPost]
  


    // فيها كلام مهم كتير فوي
    //public async Task<IActionResult> Index()
    //{

    //    //if (User.Identity.IsAuthenticated)
    //    //{
    //    //    var getUser = await _userManager.GetUserAsync(User);
    //    //    _logger.LogInformation("User is logged in:");
    //    //    _logger.LogInformation($"User ID: {getUser.Id}");
    //    //    _logger.LogInformation($"User Name: {getUser.Name}");
    //    //    _logger.LogInformation($"User Email: {getUser.Email}");

    //    //    var calems = User.Claims;
    //    //    foreach(var cal in calems)
    //    //    {
    //    //        _logger.LogInformation($"{cal.Type}: {cal.Value}");

    //    //    }

    //    //    var userRole = await _userManager.GetRolesAsync(getUser);

    //    //    if (userRole.Any())
    //    //    {
    //    //        _logger.LogInformation("User Roles:");
    //    //        foreach (var role in userRole)
    //    //        {
    //    //            _logger.LogInformation($"Role: {role}");
    //    //        }
    //    //    }

    //    //        IdentityResult createRole = await _roleManager.CreateAsync(new IdentityRole { Name = "Viewer" });

    //    //        IdentityResult roleRes = await _userManager.AddToRoleAsync(getUser, "Admin");

    //    //    if (roleRes.Succeeded)
    //    //    {
    //    //            _logger.LogInformation("Successfully added 'Admin' role to the user.");
    //    //    }
    //    //    else{
    //    //            foreach (var er in roleRes.Errors)
    //    //            {
    //    //                Console.WriteLine(er.Description);
    //    //            }
    //    //    }



    //    //}

    //    IdentityRole? singlerole = await _roleManager.FindByNameAsync("Admin");
    //    Console.WriteLine(singlerole.Id);
    //    return View();
    //}

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
