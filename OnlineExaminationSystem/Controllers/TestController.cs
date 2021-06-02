using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.Services;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class TestController : Controller
    {
        #region Instanciation
        IApplicationSettingsService _settingService;
        IAssessmentService _assessmentService;
        public TestController()
        {
            _settingService = new ApplicationSettingsService(new OnlineExaminationDatabaseEntities());
            _assessmentService = new AssessmentService(new OnlineExaminationDatabaseEntities(), new QuestionBankService());
        }
        public TestController(ApplicationSettingsService settingsService, AssessmentService assessmentService)
        {
            _settingService = settingsService;
            _assessmentService = assessmentService;
        }
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        #endregion
        public ActionResult Start()
        {
            if (Session["ApplicantID"] == null)
                return RedirectToAction("Login", "Account");

            var applicantID = Global.AuthenticatedApplicantID;
            var applcant = new Global().ApplicantInfo();
            var model = new AssessmentVM();

            var assessment = db.Assessments.Where(x => x.VaccancyID == applcant.VaccancyID && x.IsDeleted == false).ToList();
            if (assessment != null)
            {
                foreach (var each in assessment)
                {
                    if ((each.StartDateTime.Date >= DateTime.Now.Date && each.StartDateTime.ToLocalTime() >= DateTime.Now.ToLocalTime()) && (each.EndDateTime.Date <= DateTime.Now.Date && each.EndDateTime.ToLocalTime() <= each.EndDateTime.ToLocalTime()))
                    {
                        model.Id = each.Id;
                        model.Description = each.Description;
                        model.Duration = each.Duration;
                        model.EndDateTime = each.EndDateTime;
                        model.StartDateTime = each.StartDateTime;
                        model.NumberOfQuestions = each.NumberOfQuestions;
                        model.VaccancyID = each.VaccancyID;

                        ViewBag.Assessment = model;
                        ViewBag.ApplicantInfo = new Global().ApplicantInfo();
                        ViewBag.Message = null;
                        return View();
                    }
                }
            }
            ViewBag.Message = "They is no test currently available";
            ViewBag.Assessment = model;
            ViewBag.ApplicantInfo = new Global().ApplicantInfo();
            return View();
        }

        public ActionResult EvalPage(int TestID)
        {
            if (Session["ApplicantID"] == null)
                Response.Redirect("~/Account/Login");

            var applicantID = Global.AuthenticatedApplicantID;
            var applicant = new Global().ApplicantInfo();

            int NoOfQuestions = 0;
            var returnModel = new TestXPaperVM();

            var assessment = _assessmentService.GetAssessment(TestID);
            NoOfQuestions = (int)assessment.NumberOfQuestions;
            returnModel.TestID = TestID;
            returnModel.TestTitle = assessment.Description;
            returnModel.TestTime = (double)assessment.Duration;

            ViewBag.ReturnModel = returnModel;
            ViewBag.ApplicantInfo = applicant;
            Session["TestID"] = TestID;

            string path = Server.MapPath("~/TempQuestionBank/");
            var applicantQuestionPath = path + applicant.Id + "_" + TestID + "_RandomQuestions.xml";
            var applicantQuestionTimePath = path + applicant.Id + "_" + TestID + "_RandomQuestionsTime.xml";

            if (_assessmentService.CheckIfApplicantHasTakenTest(TestID, applicantID) == 0)
            {

                IQueryable<TestXPaperVM> model = null;
                List<TestXPaperVM> tempModel = new List<TestXPaperVM>();

                // check if student has started exam before
                if (_assessmentService.CheckIfApplicantQuestionExistsinPath(applicantQuestionPath))
                {
                    tempModel = _assessmentService.ReadSavedQuestion(applicantQuestionPath);
                    model = tempModel.AsQueryable();
                    returnModel.Duration = _assessmentService.GetTimeRemaining(applicantQuestionTimePath);
                }
                else
                {
                    returnModel.Duration = (int)assessment.Duration;
                    tempModel = db.VaccancyQuestionMappings.Where(x => x.IsDeleted == false
                                                                     && x.VaccancyID == assessment.VaccancyID && x.IsSelected == true)
                        .Select(b => new TestXPaperVM()
                        {
                            Id = b.Id,
                            Question = b.Question.Question,
                            QuestionType = b.Question.QuestionType,
                            QuestionID = b.QuestionID,
                            Options = b.Question.Options.Select(o => new OptionBankVM()
                            {
                                Id = o.Id,
                                OptionTxt = o.OptionText
                            }).OrderBy(o => Guid.NewGuid()).ToList()
                        }).OrderBy(o => Guid.NewGuid()).Take(NoOfQuestions).ToList();
                    _assessmentService.SaveRandomQuestion(tempModel, applicantID, TestID, path);
                    model = tempModel.AsQueryable();
                }
                return View(model);
            }
            else
            {
                ViewBag.EMessage = "You have taken this test";
                return View();
            }
        }


        [HttpPost]
        public ActionResult SaveQuestionProgress(List<TestAnswersVM> vmodel)
        {
            int testID = Convert.ToInt32(Session["TestID"].ToString());
            int applicantID = Global.AuthenticatedApplicantID;
            var applicant = new Global().ApplicantInfo();
            var assessment = db.Assessments.FirstOrDefault(x => x.Id == testID);
            string path = Server.MapPath("~/TempQuestionBank/");
            var applicantQuestionPath = path + applicant.Id + "_" + testID + "_RandomQuestions.xml";
            var applicantQuestionTimePath = path + applicant.Id + "_" + testID + "_RandomQuestionsTime.xml";
            foreach (var questionrow in vmodel)
            {
                _assessmentService.SavedApplicantAnwser(applicantQuestionPath, questionrow);
            }
            string msg = "Progress saved";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveQuestionProgressTime(float timeremaining)
        {
            int testID = Convert.ToInt32(Session["TestID"].ToString());
            int applicantID = Global.AuthenticatedApplicantID;
            var applicant = new Global().ApplicantInfo();
            var assessment = db.Assessments.FirstOrDefault(x => x.Id == testID);
            string path = Server.MapPath("~/TempQuestionBank/");
            var applicantQuestionTimePath = path + applicant.Id + "_" + testID + "_RandomQuestionsTime.xml";

            _assessmentService.UpdateTime(applicantQuestionTimePath, timeremaining.ToString());
            string msg = "Progress saved";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PostAnswers(List<TestAnswersVM> Vmodel)
        {
            List<TestAnswersVM> finalResultQuiz = new List<TestAnswersVM>();
            //Analysing the test
            foreach (TestAnswersVM answer in Vmodel)
            {
                TestAnswersVM result = db.AnswerBanks.Where(a => a.QuestionID == answer.QuestionID).Select(a => new TestAnswersVM
                {
                    QuestionID = a.QuestionID.Value,
                    AnswerQ = answer.AnswerQ,
                    isCorrect = (answer.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))
                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            int testID = Convert.ToInt32(Session["TestID"].ToString());
            int applicantID = Global.AuthenticatedApplicantID;
            var assessment = db.Assessments.FirstOrDefault(x => x.Id == testID);

            float TotalScore = 0;
            // Saving test to database
            foreach (var quiz in finalResultQuiz)
            {
                var model = new TestXPaper()
                {
                    TestID = testID,
                    ApplicantID = applicantID,
                    QuestionID = quiz.QuestionID,
                    AnswerText = quiz.AnswerQ,
                    IsCorrect = quiz.isCorrect,
                };

                model.MarkGotten = model.IsCorrect.Equals(true)
                 ? _assessmentService.GetAssessmentMarkForQuestion((int)model.QuestionID, (int)assessment.VaccancyID)
                 : 0;
                TotalScore += model.MarkGotten;

                db.TestXPapers.Add(model);
            }
            // Saving the total score
            var applicantAssessmentScore = new ApplicantAssessmentScore()
            {
                ApplicantID = applicantID,
                TestID = testID,
                Score = TotalScore,
                DateTimeSubmitted = DateTime.Now,
                IsDeleted = false,
            };
            db.ApplicantAssessmentScores.Add(applicantAssessmentScore);

            //Save all
            db.SaveChanges();
            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Finish()
        {
            ViewBag.ApplicantInfo = new Global().ApplicantInfo();
            return View("_Finish");
        }
    }
}