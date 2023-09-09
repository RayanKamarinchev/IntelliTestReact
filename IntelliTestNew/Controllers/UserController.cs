//using System;
//using System.IO;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using IntelliTest.Core.Contracts;
//using IntelliTest.Core.Models;
//using IntelliTest.Core.Models.Mails;
//using IntelliTest.Core.Models.Tests;
//using IntelliTest.Core.Models.Users;
//using IntelliTest.Data.Entities;
//using IntelliTest.Infrastructure;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace IntelliTest.Controllers
//{
//    [Authorize]
//    public class UserController : Controller
//    {
//        private readonly UserManager<User> userManager;
//        private readonly SignInManager<User> signInManager;
//        private readonly RoleManager<IdentityRole> roleManager;
//        private readonly ITeacherService teacherService;
//        private readonly IStudentService studentService;
//        private readonly ILessonService lessonService;
//        private readonly IEmailService emailService;
//        private readonly ITestService testService;
//        private readonly ITestResultsService testResultsService;
//        private readonly IWebHostEnvironment webHostEnvironment;

//        public UserController(UserManager<User> _userManager,
//                              SignInManager<User> _signInManager,
//                              RoleManager<IdentityRole> _roleManager,
//                              ITeacherService _teacherService,
//                              IStudentService _studentService,
//                              ILessonService _lessonService,
//                              IEmailService email_service,
//                              ITestService testService,
//                              IWebHostEnvironment webHostEnvironment,
//                              ITestResultsService testResultsService)
//        {
//            userManager = _userManager;
//            signInManager = _signInManager;
//            studentService = _studentService;
//            teacherService = _teacherService;
//            lessonService = _lessonService;
//            this.emailService = email_service;
//            roleManager = _roleManager;
//            this.testService = testService;
//            this.webHostEnvironment = webHostEnvironment;
//            this.testResultsService = testResultsService;
//        }

//        private async Task GiveRole(User user, string roleName)
//        {
//            var roleExists = await roleManager.RoleExistsAsync(roleName);
//            if (roleExists)
//            {
//                await userManager.AddToRoleAsync(user, roleName);
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> ViewProfile()
//        {
//            var user = await userManager.GetUserAsync(User);
//            EditUser model = new EditUser()
//            {
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                IsTeacher = User.IsTeacher(),
//                ImageUrl = user.PhotoPath
//            };
//            TempData["imagePath"] = user.PhotoPath;
//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ViewProfile(EditUser model)
//        {
//            var user = await userManager.GetUserAsync(User);
//            model.ImageUrl = (string)TempData.Peek("imagePath");
//            if (model.Image != null && model.Image.ContentType.StartsWith("image"))
//            {
//                string folder = "imgs/";
//                if (string.IsNullOrEmpty((string)TempData.Peek("imagePath")))
//                {
//                    folder += Guid.NewGuid() + "_" + model.Image.FileName;
//                    model.ImageUrl = folder;
//                }
//                else
//                {
//                    folder = (string)TempData["imagePath"];
//                }
//                string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
//                await model.Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
//                model.FirstName = user.FirstName;
//                model.LastName = user.LastName;
//                user.PhotoPath = folder;
//            }
//            else
//            {
//                if (!ModelState.IsValid)
//                {
//                    return View(model);
//                }
//                user.FirstName = model.FirstName;
//                user.LastName = model.LastName;
//                if (model.Password != null && await userManager.CheckPasswordAsync(user, model.Password))
//                {
//                    user.Email = model.Email;
//                }

//                if (!User.IsTeacher() && !User.IsStudent())
//                {
//                    if (model.IsTeacher)
//                    {
//                        await GiveRole(user, "Teacher");
//                        await teacherService.AddTeacher(User.Id(), model.School);
//                    }
//                    else
//                    {
//                        await GiveRole(user, "Student");
//                        await studentService.AddStudent(new UserType()
//                        {
//                            Grade = 0,
//                            IsStudent = true,
//                            School = ""
//                        }, User.Id());
//                    }

//                    return await Logout();
//                }
//            }

//            await testService.SaveChanges();
//            return View(model);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetPanel(string type)
//        {
//            switch (type)
//            {
//                case "results":
//                    var results = await testResultsService.GetStudentsTestsResults((Guid)TempData.Peek("StudentId"));
//                    return PartialView("Panels/UserTestResultsPartialView", results);
//                case "read":
//                    var read = await lessonService.ReadLessons(User.Id());
//                    return PartialView("Panels/ReadLikedLeassonsPartialView", read);
//                case "like":
//                    var liked = await lessonService.LikedLessons(User.Id());
//                    return PartialView("Panels/ReadLikedLeassonsPartialView", liked);
//                case "myTests":
//                    QueryModel<TestViewModel> myTests = new QueryModel<TestViewModel>();
//                    if (User.IsTeacher())
//                    {
//                        myTests = await testService.GetMy((Guid)TempData.Peek("TeacherId"), null, new QueryModel<TestViewModel>());
//                    }
//                    else if (User.IsStudent())
//                    {
//                        Guid studentOwnerId = (Guid)TempData.Peek("StudentId");
//                        myTests = await testService.TestsTakenByStudent(studentOwnerId, new QueryModel<TestViewModel>());
//                    }

//                    return PartialView("Panels/MyTestsPartialView", myTests);
//                default:
//                    var user = await userManager.GetUserAsync(User);
//                    EditUser model = new EditUser()
//                    {
//                        FirstName = user.FirstName,
//                        LastName = user.LastName,
//                        Email = user.Email,
//                        IsTeacher = User.IsTeacher()
//                    };
//                    return PartialView("Panels/UserInfoPartialView", model);
//            }
//        }

//        [HttpGet]
//        public async Task<bool> CheckPassword(string password)
//        {
//            var user = await userManager.GetUserAsync(User);
//            bool res = await userManager.CheckPasswordAsync(user, password);
//            return res;
//        }

       
//        public void AddRoleIdsToTempData(string userId)
//        {
//            if (!TempData.Keys.Contains("TeacherId"))
//            {
//                TempData["TeacherId"] = teacherService.GetTeacherId(userId);
//            }
//            if (!TempData.Keys.Contains("StudentId"))
//            {
//                TempData["StudentId"] = studentService.GetStudentId(userId);
//            }
//        }
//    }
//}
