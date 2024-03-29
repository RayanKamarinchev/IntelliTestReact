﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliTest.Core.Models.Users
{
    public class StudentViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public List<decimal> TestResults { get; set; }
    }
}
