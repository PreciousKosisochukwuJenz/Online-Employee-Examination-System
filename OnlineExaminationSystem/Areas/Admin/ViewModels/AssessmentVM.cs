using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.ViewModels
{
    public class AssessmentVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? VaccancyID { get; set; }
        public int? NumberOfQuestions { get; set; }
        public int? Duration { get; set; }
        public string Vaccancy { get; set; }
    }
}