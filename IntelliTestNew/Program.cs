using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using IntelliTest.Core.Contracts;
using IntelliTest.Core.Hubs;
using IntelliTest.Core.Models.Mails;
using IntelliTest.Core.Services;
using IntelliTest.Data.Entities;
using IntelliTest.Infrastructure;
using IntelliTestReact.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
                      builder => builder.WithOrigins("https://localhost:44411")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
});

var connectionString = builder.Configuration["ConnectionString"];

// Add services to the container.
builder.Services.AddDbContext<IntelliTestDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
       .AddDefaultUI()
       .AddEntityFrameworkStores<IntelliTestDbContext>()
       .AddDefaultTokenProviders();

//External logins
builder.Services.AddAuthentication()
       .AddFacebook(facebookOptions =>
       {
           facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
           facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
       }).AddGoogle(googleOptions =>
       {
           googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
           googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
       });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/Error/401";
});

builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
}).AddJsonOptions(o => o.JsonSerializerOptions
                        .ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<ITestResultsService, TestResultsService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<ILessonService, LessonService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IClassService, ClassService>();
builder.Services.AddTransient<IStudentService, StudentService>();

builder.Services.AddSignalR();

builder.Services.AddMemoryCache();
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = "localhost:6379";
//});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
                                                                   o.TokenLifespan = TimeSpan.FromHours(3));

builder.Services.AddIdentityServer()
       .AddApiAuthorization<User, IntelliTestDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
string[] roleNames = { "Teacher", "Student" };
IdentityResult roleResult;
app.SeedAdmin();

foreach (var roleName in roleNames)
{
    var roleExist = await roleManager.RoleExistsAsync(roleName);
    if (!roleExist)
    {
        //create the roles and seed them to the database: Question 1
        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("all");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.MapHub<ChatHub>("/chatHub");
app.MapHub<TestEditHub>("/testEditHub");

app.Run();