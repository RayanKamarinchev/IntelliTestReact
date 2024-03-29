﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliTest.Core.Models.Questions
{
    public class QuestionViewModel
    {
        [Required]
        public string Text { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
