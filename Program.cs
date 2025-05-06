using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskManegmentProject.DBcontcion;
using TaskManegmentProject.Repos;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using TaskManegmentProject.MyHubs;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews(
   options =>
{
   var policy = new AuthorizationPolicyBuilder()
       .RequireAuthenticatedUser()
       .Build();
   options.Filters.Add(new AuthorizeFilter(policy));
}).AddRazorRuntimeCompilation();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/signin-google" ;
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });
//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddSession(op =>
{
    op.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSignalR();

builder.Services.AddDbContext<TMContextDB>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
{
    op.Password.RequireDigit = false;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireUppercase = false;
    op.Password.RequireLowercase = false;
    op.Password.RequiredUniqueChars =0;
}).AddEntityFrameworkStores<TMContextDB>();

builder.Services.ConfigureApplicationCookie(op =>
{
    op.LoginPath = "/Account/Login"; 
    op.AccessDeniedPath = "/Account/AccessDenied"; 
    op.LogoutPath = "/Account/Logout"; 
});

builder.Services.AddScoped<ITaskRepository,TaskRepository>();
builder.Services.AddScoped<IWorkSpaceRepository, WorkSpaceRepository>();
builder.Services.AddScoped<IMemberWorkSpaceRepository, MemberWorkSpaceRepository>();
builder.Services.AddScoped<IMessageChatRepository, MessageChatRepository>();

builder.Services.AddScoped<IMessageMentionsRepository, MessageMentionsRepository>();
builder.Services.AddScoped<INotificationRepositry, NotificationRepositry>();
builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();


//builder.Services.AddScoped<IWorkSpaceRepository, WorkSpaceRepository>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=login}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<NotifcationHub>("/notifcationhub");
app.MapHub<Chat>("/chat/workspace");
app.MapHub<StatusHub>("/statusHub");
app.Run();
