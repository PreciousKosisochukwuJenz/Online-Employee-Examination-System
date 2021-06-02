using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.ViewModels
{
    public class TestXPaperVM
    {
        public int Id { get; set; }
        public int? TestID { get; set; }
        public int? QuestionID { get; set; }
        public QuestionType QuestionType { get; set; }
        public double Duration { get; set; }
        public double TestTime { get; set; }
        public string TestTitle { get; set; }
        public float Mark { get; set; }
        public string Question { get; set; }
        public string Anwser { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public List<OptionBankVM> Options { get; set; }
        public TestXPaperVM() { }
    }
}