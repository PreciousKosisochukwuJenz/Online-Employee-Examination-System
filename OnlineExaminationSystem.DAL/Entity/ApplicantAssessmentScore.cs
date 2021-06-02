using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class ApplicantAssessmentScore
    {
        public int Id { get; set; }
        public int? ApplicantID { get; set; }
        public int? TestID { get; set; }
        public float Score { get; set; }
        public DateTime DateTimeSubmitted { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("ApplicantID")]
        public Applicant Applicant { get; set; }
        [ForeignKey("TestID")]
        public Assessment Test { get; set; }
    }
}
