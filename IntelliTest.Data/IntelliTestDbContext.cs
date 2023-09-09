﻿using Duende.IdentityServer.EntityFramework.Options;
using IntelliTest.Data.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IntelliTestReact.Data
{
    public class IntelliTestDbContext : ApiAuthorizationDbContext<User>
    { 
        public IntelliTestDbContext(DbContextOptions options, IOptions<OperationalStoreOptions>? operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentClass>()
                   .HasKey(p => new { p.ClassId, p.StudentId });
            builder.Entity<LessonLike>()
                   .HasKey(p => new { p.LessonId, p.UserId });
            builder.Entity<TestLike>()
                   .HasKey(p => new { p.TestId, p.UserId });
            builder.Entity<Read>()
                   .HasKey(p => new { p.LessonId, p.UserId });
            builder.Entity<TestResult>()
                   .HasKey(p => new { p.TestId, p.StudentId });
            builder.Entity<ClassTest>()
                   .HasKey(p => new { p.TestId, p.ClassId });
            builder.Entity<RoomUser>()
                   .HasKey(p => new { p.RoomId, p.UserId });

            base.OnModelCreating(builder);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<ClosedQuestionAnswer> ClosedQuestionAnswers { get; set; }
        public DbSet<OpenQuestionAnswer> OpenQuestionAnswers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<OpenQuestion> OpenQuestions { get; set; }
        public DbSet<ClosedQuestion> ClosedQuestions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Read> Reads { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ClassTest> ClassTests { get; set; }
        public DbSet<RoomUser> RoomUsers { get; set; }
    }
}