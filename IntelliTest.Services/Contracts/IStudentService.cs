using IntelliTest.Core.Models.Users;
using IntelliTest.Core.Models.Questions;
using IntelliTest.Data.Entities;
using IntelliTest.Core.Models.Tests;

namespace IntelliTest.Core.Contracts
{
    public interface IStudentService
    {
        Task AddStudent(UserType model, string userId);
        int? GetStudentId(string userId);

        Task<Student> GetStudent(int studentId);
        public Task<List<StudentViewModel>> getClassStudents(int id);
        public Task<IEnumerable<StudentViewModel>> GetExaminers(int testId);
    }
}