namespace IntelliTest.Core.Contracts
{
    public interface ITeacherService
    {
        Task AddTeacher(string userId, string school);
        int? GetTeacherId(string userId);
    }
}
