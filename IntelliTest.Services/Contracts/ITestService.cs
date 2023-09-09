using IntelliTest.Core.Models;
using IntelliTest.Core.Models.Questions;
using IntelliTest.Core.Models.Tests;
using IntelliTest.Core.Models.Users;
using IntelliTest.Data.Entities;

namespace IntelliTest.Core.Contracts
{
    public interface ITestService
    {
        Task<QueryModel<TestViewModel>> GetAll(int? teacherId, int? studentId, QueryModel<TestViewModel> query);
        Task<QueryModel<TestViewModel>> GetMy(int? teacherId, int? studentId, QueryModel<TestViewModel> query);
        Task<TestViewModel> GetById(int id);
        Task<bool> ExistsbyId(int id);
        Task Edit(int id, TestEditViewModel model, int teacherId);
        TestSubmitViewModel ToSubmit(TestViewModel model);
        Task<bool> IsTestTakenByStudentId(int testId, int studentId);
        Task<QueryModel<TestViewModel>> TestsTakenByStudent(int studentId, QueryModel<TestViewModel> query);
        Task<int> Create(TestViewModel model, int teacherId, string[] classNames);
        public Task<bool> StudentHasAccess(int testId, int studentId);
        public Task DeleteTest(int id);
        Task SaveChanges();
        public Task<QueryModel<TestViewModel>> Filter(IQueryable<Test> testQuery, QueryModel<TestViewModel> query, int? teacherId, int? studentId);
        public Task<QueryModel<TestViewModel>> FilterMine(IEnumerable<Test> testQuery, QueryModel<TestViewModel> query);
        public Task<bool> IsTestCreator(int testId, int teacherId);

        Task<QueryModel<TestViewModel>> GetAllAdmin(QueryModel<TestViewModel> query);
    }
}