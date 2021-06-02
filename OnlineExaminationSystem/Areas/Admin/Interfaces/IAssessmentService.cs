using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.Areas.Admin.Interfaces
{
    interface IAssessmentService
    {
        List<AssessmentVM> GetAssessments();
        bool CreateAssessment(AssessmentVM vmodel);
        AssessmentVM GetAssessment(int id);
        bool UpdateAssessment(AssessmentVM vmodel);
        bool DeleteAssessment(int id);
        int CheckIfApplicantHasTakenTest(int testID, int applicantID);
        bool CheckIfApplicantQuestionExistsinPath(string path);
        List<TestXPaperVM> ReadSavedQuestion(string path);
        double GetTimeRemaining(string path);
        void SaveRandomQuestion(List<TestXPaperVM> vmodel, int applicantID, int AssessmentID, string path);
        void SavedApplicantAnwser(string path, TestAnswersVM vmodel);
        void UpdateTime(string path, string time);
        float GetAssessmentMarkForQuestion(int questionID, int VaccancyID);

        List<ApplicantAssessmentScoreVM> GetAssessmentLog(int TestID);
        List<TestXPaperVM> GetApplicantTest(int applicantID, int testID);
    }
}
