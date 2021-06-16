using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace OnlineExaminationSystem.Areas.Admin.Services
{
    public class AssessmentService : IAssessmentService
    {
        // Instanciation Process
        #region Instanciation
        readonly OnlineExaminationDatabaseEntities _db;
        readonly QuestionBankService _questionBankService;
        public AssessmentService()
        {
            _db = new OnlineExaminationDatabaseEntities();
            _questionBankService = new QuestionBankService(new OnlineExaminationDatabaseEntities());
        }
        public AssessmentService(OnlineExaminationDatabaseEntities db,QuestionBankService questionBankService)
        {
            _db = db;
            _questionBankService = questionBankService;
        }
        #endregion

        public List<AssessmentVM> GetAssessments()
        {
            var model = _db.Assessments.Where(x => x.IsDeleted == false)
                .Select(b => new AssessmentVM()
                {
                    Id = b.Id,
                    Description = b.Description,
                    Duration = b.Duration,
                    StartDateTime = b.StartDateTime,
                    EndDateTime = b.EndDateTime,
                    NumberOfQuestions = b.NumberOfQuestions,
                    Vaccancy = b.Vaccancy.Description,
                    VaccancyID = b.VaccancyID
                }).ToList();
            return model;
        }
        public bool CreateAssessment(AssessmentVM vmodel)
        {
            var model = new Assessment()
            {
                Description = vmodel.Description,
                Duration = vmodel.Duration,
                StartDateTime = vmodel.StartDateTime,
                EndDateTime = vmodel.EndDateTime,
                VaccancyID = vmodel.VaccancyID,
                NumberOfQuestions = vmodel.NumberOfQuestions,
                IsDeleted = false,
                DateCreated = DateTime.Now
            };
            _db.Assessments.Add(model);
            _db.SaveChanges();
            return true;
        }
        public AssessmentVM GetAssessment(int id)
        {
            var model = _db.Assessments.Where(x => x.Id == id).Select(b => new AssessmentVM()
            {
                Id = b.Id,
                Description = b.Description,
                Duration = b.Duration,
                StartDateTime = b.StartDateTime,
                EndDateTime = b.EndDateTime,
                NumberOfQuestions = b.NumberOfQuestions,
                Vaccancy = b.Vaccancy.Description,
                VaccancyID = b.VaccancyID
            }).FirstOrDefault();
            return model;
        }
        public bool UpdateAssessment(AssessmentVM vmodel)
        {
            var model = _db.Assessments.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Description = vmodel.Description;
            model.Duration = vmodel.Duration;
            model.StartDateTime = vmodel.StartDateTime;
            model.EndDateTime = vmodel.EndDateTime;
            model.NumberOfQuestions = vmodel.NumberOfQuestions;
            model.VaccancyID = vmodel.VaccancyID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        public bool DeleteAssessment(int id)
        {
            var model = _db.Assessments.FirstOrDefault(x => x.Id == id);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public int CheckIfApplicantHasTakenTest(int testID, int applicantID)
        {
            var check = _db.TestXPapers.Count(x => x.TestID == testID && x.ApplicantID == applicantID);
            return check;
        }

        public bool CheckIfApplicantQuestionExistsinPath(string path)
        {
            if (System.IO.File.Exists(path))
                return true;
            else
                return false;

        }
        public List<TestXPaperVM> ReadSavedQuestion(string path)
        {

            XDocument xmlDoc = XDocument.Load(path);
            var list = xmlDoc.Root.Elements("RandomQuestions")
                                       .Select(element => new TestXPaperVM
                                       {
                                           Question = element.Element("Question").Value,
                                           QuestionID = Convert.ToInt32(element.Element("QuestionID").Value),
                                           Id = Convert.ToInt32(element.Element("Id").Value),
                                           QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), element.Element("QuestionType").Value),
                                           Options = element.Elements("Options").Descendants("Option").Select(opt => new OptionBankVM() { OptionTxt = opt.Value }).ToList(),
                                           UserAnswer = element.Element("UserAnswer").Value
                                       }).ToList();
            return list;
        }

        public double GetTimeRemaining(string path)
        {
            XDocument xmlDoc = XDocument.Load(path);
            var time = xmlDoc.Element("Time").Value;
            return Convert.ToDouble(time);
        }
        public void SaveRandomQuestion(List<TestXPaperVM> vmodel, int applicantID, int AssessmentID, string path)
        {
            var assessment = GetAssessment(AssessmentID);
            XDocument xdoc = new XDocument(
                      new XDeclaration("1.0", "utf-8", "yes"),

                          // This is the root of the document
                          new XElement("RandomQuestions",
                          from ques in vmodel
                          select
                              new XElement("RandomQuestions",
                              new XElement("Question", ques.Question),
                              new XElement("QuestionID", ques.QuestionID),
                              new XElement("Id", ques.Id),
                              new XElement("QuestionType", ques.QuestionType),
                              new XElement("Options", from opt in ques.Options
                                                      select new XElement("Option", opt.OptionTxt)),
                              new XElement("UserAnswer", "")
                              )));
            xdoc.Save(path + applicantID + "_" + AssessmentID + "_RandomQuestions.xml");


            XDocument xbasicdoc = new XDocument(
                      new XDeclaration("1.0", "utf-8", "yes"),
                      // This will hold the generic information of the students assessment
                      new XElement("Time", assessment.Duration));
            xbasicdoc.Save(path + applicantID + "_" + AssessmentID + "_RandomQuestionsTime.xml");
        }

        public void SavedApplicantAnwser(string path, TestAnswersVM vmodel)
        {
            if(vmodel.AnswerQ != null)
            {
                XDocument xmlDoc = XDocument.Load(path);
                var questionRow = xmlDoc.Root.Elements("RandomQuestions").Where(x => x.Element("QuestionID").Value == vmodel.QuestionID.ToString()).FirstOrDefault();
                questionRow.Element("UserAnswer").Value = vmodel.AnswerQ;
                xmlDoc.Save(path);
            }
        }

        public void UpdateTime(string path, string time)
        {
            XDocument xmlDoc = XDocument.Load(path);
            var Time = xmlDoc.Element("Time").Value = time;
            xmlDoc.Save(path);
        }
        public float GetAssessmentMarkForQuestion(int questionID, int VaccancyID)
        {
            float mark = _db.VaccancyQuestionMappings.Where(x => x.QuestionID == questionID && x.VaccancyID == VaccancyID).FirstOrDefault().Mark;
            return mark;
        }

        public List<ApplicantAssessmentScoreVM> GetAssessmentLog(int TestID)
        {
            var model = _db.ApplicantAssessmentScores.Where(x => x.TestID == TestID && x.IsDeleted == false).Select(b => new ApplicantAssessmentScoreVM()
            {
                Id = b.Id,
                ApplicantID = b.ApplicantID,
                TestID = b.TestID,
                ApplicantName = b.Applicant.Surname + " " + b.Applicant.Firstname + " " + b.Applicant.Lastname,
                ApplicantEmail = b.Applicant.EmailAddress,
                Score = b.Score,
                DateTimeSubmitted = b.DateTimeSubmitted,
                ApplicantPhoto = b.Applicant.Photo
            }).OrderBy(x => x.Score).ToList();
            return model;
        }
        public string UserAnswer(int QuestionID, int ApplicantID, int TestID)
        {
            var answer = _db.TestXPapers
                .Where(x => x.QuestionID == QuestionID && x.TestID == TestID && x.ApplicantID == ApplicantID)
                .FirstOrDefault().AnswerText;
            return answer;
        }

        public List<TestXPaperVM> GetApplicantTest(int applicantID, int testID)
        {
            List<TestXPaperVM> model = null;

            model = _db.TestXPapers.Where(x => x.ApplicantID == applicantID && x.TestID == testID)
              .Select(b => new TestXPaperVM()
              {
                  Id = b.Id,
                  Question = b.Question.Question,
                  QuestionType = b.Question.QuestionType,
                  QuestionID = b.QuestionID,
                  IsCorrect = b.IsCorrect,
                  Options = b.Question.Options.Select(o => new OptionBankVM()
                  {
                      Id = o.Id,
                      OptionTxt = o.OptionText
                  }).OrderBy(o => Guid.NewGuid()).ToList(),
              }).OrderBy(o => o.Id).ToList();

            foreach (var item in model)
            {
                item.Anwser = _questionBankService.GetAnswer(Convert.ToInt32(item.QuestionID)).AnswerText;
                item.UserAnswer = UserAnswer((int)item.QuestionID, applicantID, testID);
            }
            return model;
        }

    }
}