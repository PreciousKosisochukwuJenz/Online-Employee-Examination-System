using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class TestXPaper
    {
        public int Id { get; set; }
        public int? ApplicantID { get; set; }
        public int? TestID { get; set; }
        public int? QuestionID { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public float MarkGotten { get; set; }

        [ForeignKey("ApplicantID")]
        public Applicant Applicant { get; set; }
        [ForeignKey("TestID")]
        public Assessment Test { get; set; }
        [ForeignKey("QuestionID")]
        public QuestionBank Question { get; set; }
    }
}
