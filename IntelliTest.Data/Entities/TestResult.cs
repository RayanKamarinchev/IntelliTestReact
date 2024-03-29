﻿using System.ComponentModel.DataAnnotations.Schema;
using IntelliTest.Data.Enums;

namespace IntelliTest.Data.Entities
{
    public class TestResult
    {
        [ForeignKey(nameof(Test))]
        public int TestId { get; set; }
        public Test Test { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public Mark Mark { get; set; }
        public decimal Score { get; set; }
        public DateTime TakenOn { get; set; }
    }
}
