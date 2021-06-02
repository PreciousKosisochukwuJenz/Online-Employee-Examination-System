using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.ViewModels
{
    public class ApplicantAssessmentScoreVM
    {
        public int Id { get; set; }
        public int? ApplicantID { get; set; }
        public int? TestID { get; set; }
        public float Score { get; set; }
        public DateTime DateTimeSubmitted { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public byte[] ApplicantPhoto { get; set; }
    }
}