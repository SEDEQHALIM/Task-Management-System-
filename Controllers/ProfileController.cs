using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TaskManegmentProject.DBcontcion;
using TaskManegmentProject.DBcontcion.ViewModels;

namespace TaskManegmentProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ApplicationUser getUser = await _userManager.GetUserAsync(User);

            UserProfile userProfile = new UserProfile()
            {
                Id = getUser.Id,
                Name = getUser.Name,
                Email = getUser.Email,
                Imageurl = getUser.ImageUrl

            };

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> uploadFile(UserProfile userProfile)
        {

            ApplicationUser getUser = await _userManager.GetUserAsync(User);
            userProfile.Id = getUser.Id;
            userProfile.Name = getUser.Name;
            userProfile.Email = getUser.Email;
            userProfile.Imageurl = getUser.ImageUrl;
            var keysToRemove = ModelState.Keys
        .Where(key => key != nameof(UserProfile.ProfileImage))
        .ToList();
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (userProfile.ProfileImage == null || userProfile.ProfileImage.Length == 0)
            {
             
                ModelState.AddModelError("ProfileImage", "Please Upload Image");
               
                return View("Index", userProfile);
            }

            string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
            string fileExtension = Path.GetExtension(userProfile.ProfileImage.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                
                ModelState.AddModelError("ProfileImage", "Please upload a valid image file (jpg, jpeg, png, gif).");

               

                return View("Index", userProfile);
            }

            var theUploadFolde = Path.Combine(_webHostEnvironment.WebRootPath, "uploaded-file");

            if (!Directory.Exists(theUploadFolde))
            {
                Directory.CreateDirectory(theUploadFolde);
            }

            var uniqNameImage = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(theUploadFolde, uniqNameImage);

            string oldImg = getUser.ImageUrl;





            // هنا هنبدانحفظ الصورة في الفولدر
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await userProfile.ProfileImage.CopyToAsync(fileStream);
            }

            getUser.ImageUrl = "/uploaded-file/" + uniqNameImage;

            IdentityResult result = await _userManager.UpdateAsync(getUser);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(oldImg))
                {
                    string oldimgpath = Path.Combine(_webHostEnvironment.WebRootPath, oldImg.TrimStart('/'));
                    if (System.IO.File.Exists(oldimgpath))
                    {
                        System.IO.File.Delete(oldimgpath);
                    }
                }
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                getUser.ImageUrl = oldImg;
                await _userManager.UpdateAsync(getUser);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        
            return View("Index", userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfile userProfile)
        {
            ApplicationUser getUser = await _userManager.GetUserAsync(User);
            var fieldsToValidate = new List<string>();
            if (!string.IsNullOrEmpty(userProfile.Name))
            {
                fieldsToValidate.Add(nameof(UserProfile.Name));
            }
            if (!string.IsNullOrEmpty(userProfile.Password) || !string.IsNullOrEmpty(userProfile.ConformPassword))
            {
                fieldsToValidate.Add(nameof(UserProfile.Password));
                fieldsToValidate.Add(nameof(UserProfile.ConformPassword));
            }

            var keysToRemove = ModelState.Keys
        .Where(key => !fieldsToValidate.Contains(key))
        .ToList();
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }
            if (!string.IsNullOrEmpty(userProfile.Password) || !string.IsNullOrEmpty(userProfile.ConformPassword))
            {
                if (string.IsNullOrEmpty(userProfile.Password))
                {
                    ModelState.AddModelError("Password", "Password is required if Confirm Password is provided.");
                }
                if (string.IsNullOrEmpty(userProfile.ConformPassword))
                {
                    ModelState.AddModelError("ConformPassword", "Confirm Password is required if Password is provided.");
                }
            }

            if (!ModelState.IsValid)
            {
                userProfile.Id = getUser.Id;
                userProfile.Name = getUser.Name;
                userProfile.Email = getUser.Email;
                userProfile.Imageurl = getUser.ImageUrl;
                return View("Index", userProfile);
            }

            if (!string.IsNullOrEmpty(userProfile.Name))
            {
                getUser.Name = userProfile.Name;
            }
            if (!string.IsNullOrEmpty(userProfile.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(getUser);
                var result = await _userManager.ResetPasswordAsync(getUser, token, userProfile.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    userProfile.Id = getUser.Id;
                    userProfile.Name = getUser.Name;
                    userProfile.Email = getUser.Email;
                    userProfile.Imageurl = getUser.ImageUrl;
                    return View("Index", userProfile);
                }
            }
            var updateResult = await _userManager.UpdateAsync(getUser);
            if (updateResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            userProfile.Id = getUser.Id;
            userProfile.Name = getUser.Name;
            userProfile.Email = getUser.Email;
            userProfile.Imageurl = getUser.ImageUrl;
            return View("Index", userProfile);
        }

        }






}
