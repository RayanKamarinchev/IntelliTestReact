//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text.Json;
//using System.Threading.Tasks;
//using IntelliTest.Core.Contracts;
//using IntelliTest.Core.Models;
//using IntelliTest.Core.Models.Questions;
//using IntelliTest.Core.Models.Tests;
//using IntelliTest.Core.Models.Users;
//using IntelliTest.Data.Enums;
//using IntelliTest.Infrastructure;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using static IntelliTest.Infrastructure.Constraints;
//using Org.BouncyCastle.Ocsp;
//using IntelliTest.Core.Models.Enums;

//namespace IntelliTest.Controllers
//{
//    [Authorize]
//    [ApiController]
//    public class TestsController : ControllerBase
//    {
//        private readonly ITestService testService;
//        private readonly ITestResultsService testResultsService;
//        private readonly IMemoryCache cache;
//        private readonly IStudentService studentService;
//        private readonly IClassService classService;

//        public TestsController(ITestService _testService, IMemoryCache _cache, IStudentService _studentService,
//                               IClassService _classService, ITestResultsService testResultsService)
//        {
//            testService = _testService;
//            cache = _cache;
//            studentService = _studentService;
//            classService = _classService;
//            this.testResultsService = testResultsService;
//        }

//        [HttpGet]
//        public IActionResult GetFilter(Filter model)
//        {
//            //return PartialView("FilterMenuPartialView", model);
//        }

//        [HttpGet]
//        public async Task<QueryModel<TestViewModel>> Index(string SearchTerm, int Grade, Subject Subject, Sorting Sorting, int currentPage
//                                                           , int teacherId, int studentId)
//        {
//            if (User.IsAdmin())
//            {
//                return RedirectToAction("Index", "Tests", new { area = "Admin" });
//            }
//            if (cache.TryGetValue("tests", out QueryModel<TestViewModel>? model) && false)
//            {
//            }
//            else
//            {
//                if (currentPage == 0)
//                {
//                    currentPage = 1;
//                }
//                QueryModel<TestViewModel> query = new QueryModel<TestViewModel>(SearchTerm, Grade, Subject, Sorting, currentPage);
//                model = await testService.GetAll(teacherId, studentId, query);
//                //var cacheEntryOptions = new DistributedCacheEntryOptions()
//                //    .SetSlidingExpiration(TimeSpan.FromMinutes(10));
//                var cacheEntryOptions = new MemoryCacheEntryOptions()
//                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
//                cache.SetAsync("tests", model, cacheEntryOptions);
//            }
//            return model;
//        }

//        [HttpGet]
//        [Authorize(Roles = "Teacher")]
//        public async Task<QueryModel<TestViewModel>> MyTests([FromQuery] QueryModel<TestViewModel> query,
//                                                             int teacherId, int studentId)
//        {
//            QueryModel<TestViewModel> model = await testService.GetMy(teacherId, studentId, query);
//            return model;
//        }


//        [Route("Tests/Edit/{Id}")]
//        [HttpGet]
//        public async Task<IActionResult> Edit(TestEditViewModel viewModel, int id)
//        {
//            if (!User.IsTeacher())
//            {
//                return Unauthorized();
//            }

//            if (viewModel.PublicityLevel == PublicityLevel.Private
//                || !await testService.ExistsbyId(id))
//            {
//                return NotFound();
//            }

//            if (viewModel.PublicityLevel == PublicityLevel.ClassOnly)
//            {
//                int testId = (int)TempData.Peek("testId");

//                if (TempData.Peek("TeacherId") is null)
//                {
//                    return RedirectToAction("Logout", "User");
//                }

//                bool isCreator = await testService.IsTestCreator(testId, (int)TempData.Peek("TeacherId"));
//                if (!isCreator)
//                {
//                    return Unauthorized();
//                }
//            }

//            var testModel = await testService.GetById(id);
//            var testToEdit = testResultsService.ToEdit(testModel);
//            testToEdit.Id = id;
//            TempData["PublicityLevel"] = testToEdit.PublicityLevel;
//            return View("Edit", testToEdit);
//        }


//        [HttpPost]
//        [Route("Tests/Edit/{Id}")]
//        [IgnoreAntiforgeryToken]
//        public async Task<IActionResult> Edit(int id, [FromBody] TestEditViewModel model)
//        {
//            if (!User.IsTeacher())
//            {
//                return Unauthorized();
//            }

//            if (!await testService.ExistsbyId(id))
//            {
//                return NotFound();
//            }

//            if (!ModelState.IsValid ||
//                model.ClosedQuestions is null || !AllQuestionsHaveAnswerIndexes(model.ClosedQuestions) ||
//                TempData.Peek("PublicityLevel") is null)
//            {
//                return View("Edit", model);
//            }

//            model.PublicityLevel = (PublicityLevel)TempData["PublicityLevel"];

//            await testService.Edit(id, model, (int)TempData.Peek(TeacherId));

//            TempData["message"] = "Успешно редактира тест!";
//            return Content("redirect");
//        }

//        private bool AllQuestionsHaveAnswerIndexes(List<ClosedQuestionViewModel> closedQuestions)
//        {
//            return closedQuestions.All(c => c.AnswerIndexes.Any(ai => ai));
//        }


//        [HttpGet]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> Create()
//        {
//            TempData["Classes"] = (await classService.GetAll(User.Id(), User.IsStudent(), User.IsTeacher()))
//                                  .Select(c => c.Name)
//                                  .ToArray();
//            return View("Create", new TestViewModel());
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(TestViewModel model)
//        {
//            if (!ModelState.IsValid ||
//                TempData.Peek("Classes") is null)
//            {
//                return View(model);
//            }

//            if (!User.IsTeacher())
//            {
//                return Unauthorized();
//            }

//            string[] classNames = (string[])TempData.Peek("Classes");
//            string[] selectedClasses = classNames.Where((c, i) => model.Selected[i]).ToArray();

//            if (TempData.Peek(TeacherId) is null)
//            {
//                return RedirectToAction("Logout", "User");
//            }

//            int id = await testService.Create(model, (int)TempData.Peek(TeacherId), selectedClasses);
//            return RedirectToAction("Edit", new { id = id });
//        }

//        [HttpGet]
//        [Authorize(Roles = "Student")]
//        [Route("Tests/Take/{testId}")]
//        public async Task<IActionResult> Take(int testId)
//        {
//            if (!await testService.ExistsbyId(testId))
//            {
//                return NotFound();
//            }

//            if (TempData.Peek(StudentId) is null)
//            {
//                return RedirectToAction("Logout", "User");
//            }
//            var studentId = (int)TempData.Peek(StudentId);
//            if (await testService.IsTestTakenByStudentId(testId, studentId))
//            {
//                return RedirectToAction("ReviewAnswers", new { testId = testId, studentId = studentId });
//            }

//            var test = testService.ToSubmit(await testService.GetById(testId));
//            return View(test);
//        }

//        [HttpPost]
//        [Route("Tests/Take/{testId}")]
//        [Authorize(Roles = "Student")]
//        public async Task<IActionResult> Take(TestSubmitViewModel model, int testId)
//        {
//            if (!await testService.ExistsbyId(testId))
//            {
//                return NotFound();
//            }

//            if (TempData.Peek(StudentId) is null)
//            {
//                return RedirectToAction("Logout", "User");
//            }

//            var studentId = (int)TempData.Peek(StudentId);
//            if (await testService.IsTestTakenByStudentId(testId, studentId))
//            {
//                return RedirectToAction("ReviewAnswers", new { testId = testId, studentId = studentId });
//            }

//            await testResultsService.AddTestAnswer(model.OpenQuestions, model.ClosedQuestions, studentId, testId);

//            TempData["message"] = "Успешно предаде теста!";
//            TempData.Remove("TestStarted");
//            return RedirectToAction("ReviewAnswers", new { testId = testId, studentId = studentId });
//        }

//        [HttpGet]
//        [Route("Review/{testId}/{studentId}")]
//        public async Task<IActionResult> ReviewAnswers(int testId, int studentId)
//        {
//            var teacherId = (int?)TempData.Peek(TeacherId);
//            var student = await studentService.GetStudent(studentId);
//            bool isStudentsTeacher = student.Classes
//                                            .Select(c => c.Class)
//                                            .Select(c => c.Teacher)
//                                            .Any(t => t.Id == teacherId);
//            if (!User.IsStudent() && !isStudentsTeacher)
//            {
//                return Unauthorized();
//            }
//            if (!await testService.ExistsbyId(testId))
//            {
//                return NotFound();
//            }

//            if (TempData.Peek(StudentId) is null || studentId != (int)TempData.Peek(StudentId))
//            {
//                return Unauthorized();
//            }

//            if (!await testService.IsTestTakenByStudentId(testId, (int)TempData.Peek(StudentId)))
//            {
//                return NotFound();
//            }

//            var test = await testResultsService.GetStudentsTestResults(testId, studentId);
//            return View(test);
//        }

//        [HttpGet]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> Statistics(int testId)
//        {
//            if (TempData.Peek("TeacherId") is null)
//            {
//                return RedirectToAction("Logout", "User");
//            }

//            if (!await testService.IsTestCreator(testId, (int)TempData.Peek("TeacherId")))
//            {
//                return NotFound();
//            }

//            var model = await testResultsService.GetStatistics(testId);

//            return View(model);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Teacher")]
//        [Route("Test/Delete/{Id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (TempData.Peek("TeacherId") is null)
//            {
//                return RedirectToAction("Logout", "User");
//            }

//            if (!await testService.IsTestCreator(id, (int)TempData.Peek("TeacherId")))
//            {
//                return NotFound();
//            }

//            await testService.DeleteTest(id);
//            TempData["message"] = "Успешно изтри тест";
//            return RedirectToAction("ViewProfile", "User");
//        }

//        [HttpGet]
//        [Authorize(Roles = "Teacher")]
//        public IActionResult AddQuestion(OpenQuestionViewModel question)
//        {
//            if (TempData.Peek("editModel") is null)
//            {
//                return View("Edit");
//            }

//            var model = JsonSerializer.Deserialize<TestEditViewModel>(TempData.Peek("editModel").ToString());
//            model.OpenQuestions.Add(question);
//            TempData["editModel"] = JsonSerializer.Serialize(model);

//            return View("Edit", model);
//        }

//        [HttpGet]
//        [Authorize(Roles = "Teacher")]
//        [Route("Examiners/{testId}")]
//        public async Task<IActionResult> ExaminersAll(int testId)
//        {
//            IEnumerable<StudentViewModel> examiners = await studentService.GetExaminers(testId);
//            return View("ExaminersAll", examiners);
//        }

//        [HttpGet]
//        [Authorize(Roles = "Teacher")]
//        [Route("Examiners/{testId}/{studentId}")]
//        public async Task<IActionResult> TestGrading(int testId, int studentId)
//        {
//            var testResult = await testResultsService.GetStudentsTestResults(testId, studentId);
//            TempData["QuestionIds"] = testResult.OpenQuestions.Select(q => q.Id.ToString()).ToArray();
//            TempData["TestId"] = testId;
//            return View("TestGrading", testResult);
//        }
//        [HttpPost]
//        [Authorize(Roles = "Teacher")]
//        [Route("Examiners/{testId}/{studentId}")]
//        public async Task<IActionResult> TestGrading(int testId, int studentId, int teacherId, TestReviewViewModel scoredTest)
//        {
//            if (TempData.Peek("TeacherId") is null)
//            {
//                return RedirectToAction("Logout", "User");
//            }

//            if (!await testService.IsTestCreator(testId, (int)TempData.Peek("TeacherId")))
//            {
//                return NotFound();
//            }

//            if (TempData["QuestionIds"] is null || TempData["TestId"] is null || (int)TempData["TestId"] != testId)
//            {
//                return View("TestGrading", scoredTest);
//            }

//            int[] quesitonIds = (int[])TempData["QuestionIds"];
//            for (int i = 0; i < quesitonIds.Length; i++)
//            {
//                scoredTest.OpenQuestions[i].Id = quesitonIds[i];
//            }

//            await testResultsService.SubmitTestScore(testId, studentId, scoredTest);

//            TempData.Remove("TestId");
//            TempData["Message"] = "Успешно оценихте теста";
//            return RedirectToRoute(new
//            {
//                controller = "Tests",
//                action = "ExaminersAll",
//                testId = testId
//            });
//        }
//    }
//}