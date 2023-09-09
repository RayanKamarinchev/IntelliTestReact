using IntelliTest.Core.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliTest.Core.Models.Users;

namespace IntelliTest.Core.Contracts
{
    public interface IClassService
    {
        Task<IEnumerable<ClassViewModel>> GetAll(string userId, bool isStudent, bool isTeacher);
        Task<ClassViewModel?> GetById(int id);
        Task<bool> IsClassOwner(int id, string userId);
        Task Create(ClassViewModel model);
        Task Edit(ClassViewModel model, int id);
        Task Delete(int id);
        Task<bool> IsInClass(int classId, string userId, bool isStudent, bool isTeacher);
        Task<bool> RemoveStudent(int studentId, int id);
        Task<bool> AddStudent(int studentId, int id);
        public Task<List<StudentViewModel>> GetClassStudents(int id);

        Task<IEnumerable<ClassViewModel>> GetAllAdmin();
    }
}