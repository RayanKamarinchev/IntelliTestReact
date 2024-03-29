﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IntelliTest.Data.Enums;

namespace IntelliTest.Data.Entities
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        [Required]
        public Subject Subject { get; set; }
        [Required]
        [Range(0, 12)]
        public int Grade { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public IList<OpenQuestion> OpenQuestions { get; set; } = new List<OpenQuestion>();
        public IList<ClosedQuestion> ClosedQuestions { get; set; } = new List<ClosedQuestion>();
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public bool MultiSubmission { get; set; }
        [Required]
        public Teacher Creator { get; set; }
        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        public List<ClassTest> ClassesWithAccess { get; set; }
        public PublicityLevel PublicyLevel { get; set; }
        public string PhotoPath { get; set; }
        public string QuestionsOrder { get; set; }

        public IEnumerable<TestLike> TestLikes { get; set; }
        public IEnumerable<TestResult> TestResults { get; set; }
    }
}
