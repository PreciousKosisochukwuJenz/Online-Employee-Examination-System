using OnlineExaminationSystem.DAL.Entity;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.Areas.Admin.Interfaces
{
    interface IQuestionBankService
    {
        List<QuestionBankVM> GetQuestions(int? QuestionCategoryID);
        List<OptionBankVM> GetOptions(int QuestionID);
        AnswerVM GetAnswer(int QuestionID);
        bool SaveNewQuestion(QuestionBankVM Vmodel);
        bool SaveNewQuestionUpload(QuestionBankVM Vmodel);
        QuestionBankVM GetQuestion(int questionId);
        bool SaveEditedQuestion(QuestionBankVM vmodel);
        void DeleteQuestion(int questionId);

        VaccancyQuestionMappingVM GetSelectedQuestions(int ID);
        int CheckQuestionExist(int ArticleContentID, int QuestionID);
        VaccancyQuestionMapping SaveQuestion(QuestionMappingList questionMapping);
        void AssignQuestion(QuestionMappingList questionMapping);
    }
}
