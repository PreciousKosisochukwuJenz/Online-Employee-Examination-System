using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.ViewModels
{
    public class VaccancyQuestionMappingVM
    {
        public string Vaccancy { get; set; }

        public IList<dynamic> TableData { get; set; }
        public IList<QuestionMappingList> TableDataSource { get; set; }
        public VaccancyQuestionMappingVM()
        {

        }
        public VaccancyQuestionMappingVM(IEnumerable<dynamic> TData)
        {
            TableData = TData.ToArray();
        }
        public VaccancyQuestionMappingVM(IEnumerable<QuestionMappingList> TData)
        {
            TableDataSource = TData.ToArray();
        }
    }
    public class QuestionMappingList
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int VaccancyID { get; set; }
        public int QuestionID { get; set; }
        public QuestionType QuestionType { get; set; }
        public float Mark { get; set; }
        public string Vaccancy { get; set; }
        public string Question { get; set; }
        public List<OptionBankVM> Options { get; set; }
        public AnswerVM Answer { get; set; }
    }
}