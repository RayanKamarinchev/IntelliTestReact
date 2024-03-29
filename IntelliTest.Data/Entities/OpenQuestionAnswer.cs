﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliTest.Data.Entities
{
    public class OpenQuestionAnswer
    {
        [Key]
        public int Id { get; set; }
        
        public string? Answer { get; set; }
        public OpenQuestion Question { get; set; }
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }
        public Student Student { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }

        public decimal Points { get; set; }
        public string? Explanation { get; set; }
    }
}
