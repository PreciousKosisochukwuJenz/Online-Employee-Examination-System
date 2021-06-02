﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class QuestionBank
    {
        public int Id { get; set; }
        public int? VaccancyID { get; set; }

        public string Question { get; set; }
        public QuestionType QuestionType { get; set; }


        public bool IsDeleted { get; set; }
        public ICollection<OptionBank> Options { get; set; }
    }
}
