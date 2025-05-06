using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManegmentProject.DBcontcion;
using TaskManegmentProject.MyHubs;
using TaskManegmentProject.Repos;

namespace TaskManegmentProject.Controllers
{
    public class ChatController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageChatRepository _messageChatRepository;
        private readonly IHubContext<Chat> _chatHubContext
;


        public ChatController(
            UserManager<ApplicationUser> userManger,
            IMessageChatRepository messageChatRepository,
            IHubContext<Chat> chatHubContext

            )
        {
            _userManager = userManger;
            _messageChatRepository = messageChatRepository;
            _chatHubContext = chatHubContext;
        }



        [HttpPost]
        public async Task<IActionResult> ChatSendMessage(string workspaceId, string content)
        {

            ApplicationUser getUser = await _userManager.GetUserAsync(User);


            MessageChat newMassage = new MessageChat()
            {
                SenderId = getUser.Id,
                Content = content,
                WorkSpaceId = workspaceId,
            };

            await _messageChatRepository.CreateAsync(newMassage);
            await _messageChatRepository.SaveAsync();


            await _chatHubContext.Clients.All.SendAsync("ReseaveMessage",new {
                SenderId = getUser.Id,
                Content = content,
                WorkSpaceId = workspaceId,
                User = getUser
            });


            return Json(new
            {
                message = "Massege Resived",
                data = newMassage,
                status = 200
            });
        }




    }
}
