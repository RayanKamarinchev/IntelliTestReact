using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliTest.Core.Models;
using IntelliTest.Core.Models.Lessons;
using IntelliTest.Data.Entities;

namespace IntelliTest.Core.Contracts
{
    public interface ILessonService
    {
        Task<QueryModel<LessonViewModel>> GetAll(int? teacherId, QueryModel<LessonViewModel> query, string userId);
        Task<LessonViewModel?>? GetById(int lessonId);
        Task<LessonViewModel?>? GetByName(string name);
        Task<bool> ExistsById(int? teacherId, int lessonId);
        Task Edit(int lessonId, EditLessonViewModel model);
        EditLessonViewModel ToEdit(LessonViewModel model);
        Task Create(EditLessonViewModel model, int teacherId);
        Task LikeLesson(int lessonId, string userId);
        Task UnlikeLesson(int lessonId, string userId);
        Task<bool> IsLiked(int lessonId, string userId);
        Task Read(int lessonId, string userId);
        Task<IEnumerable<LessonViewModel>> ReadLessons(string userId);
        Task<IEnumerable<LessonViewModel>> LikedLessons(string userId);

        public Task<QueryModel<LessonViewModel>> Filter(IQueryable<Lesson> lessonQuery,
                                                        QueryModel<LessonViewModel> query, string userId);

        public Task<bool> IsLessonCreator(int lessonId, int teacherId);

        public Task<QueryModel<LessonViewModel>> GetAllAdmin(QueryModel<LessonViewModel> query);
    }
}