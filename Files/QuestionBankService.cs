using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Entity;
using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.Services
{
    public class QuestionBankService : IQuestionBankService
    {
        // Instanciation Process
        #region Instanciation
        readonly OnlineExaminationDatabaseEntities _db;
        public QuestionBankService()
        {
            _db = new OnlineExaminationDatabaseEntities();
        }
        public QuestionBankService(OnlineExaminationDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        // Fetching all questions for a category
        public List<QuestionBankVM> GetQuestions(int? QuestionCategoryID)
        {
            var model = _db.QuestionBanks.Where(x => x.VaccancyID == QuestionCategoryID && x.IsDeleted == false).Select(b => new QuestionBankVM()
            {
                Id = b.Id,
                Question = b.Question,
                QuestionType = b.QuestionType,
            }).ToList();
            foreach (var question in model)
            {
                question.OptionBank = GetOptions(question.Id);
                question.Answer = GetAnswer(question.Id);
            }
            return model;
        }

        // Fetching the options for a question
        public List<OptionBankVM> GetOptions(int QuestionID)
        {
            var options = _db.OptionBanks.Where(x => x.QuestionID == QuestionID).Select(b => new OptionBankVM()
            {
                Id = b.Id,
                OptionTxt = b.OptionText,
            }).ToList();
            return options;
        }

        // Fetching the answer of a question
        public AnswerVM GetAnswer(int QuestionID)
        {
            var answer = _db.AnswerBanks.Where(x => x.QuestionID == QuestionID)
                .Select(b => new AnswerVM() { Id = b.Id, AnswerText = b.AnswerText }).FirstOrDefault();
            return answer;
        }

        // Saving question created using a single upload method
        public bool SaveNewQuestion(QuestionBankVM Vmodel)
        {
            bool HasSaved = false;
            var QuestionModel = new QuestionBank()
            {
                Question = Vmodel.Question,
                VaccancyID = Vmodel.QuestionCategoryID,
                QuestionType = Vmodel.QuestionType,
                IsDeleted = false,
            };
            _db.QuestionBanks.Add(QuestionModel);
            _db.SaveChanges();

            // Saving the options
            if (Vmodel.OptionBank != null)
            {
                foreach (var option in Vmodel.OptionBank)
                {
                    var OptionModel = new OptionBank()
                    {
                        QuestionID = QuestionModel.Id,
                        OptionText = option.OptionTxt,
                        IsDeleted = false
                    };
                    _db.OptionBanks.Add(OptionModel);
                }
                _db.SaveChanges();
            }

            // Saving the answer
            var answer = new AnswerBank()
            {
                QuestionID = QuestionModel.Id,
                AnswerText = Vmodel.Answer.AnswerText
            };
            _db.AnswerBanks.Add(answer);
            _db.SaveChanges();

            HasSaved = true;
            return HasSaved;
        }

        // Saving the upload question into the question bank
        public bool SaveNewQuestionUpload(QuestionBankVM Vmodel)
        {
            bool HasSaved = false;
            var QuestionModel = new QuestionBank()
            {
                Question = Vmodel.Question,
                VaccancyID = Vmodel.QuestionCategoryID,
                QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), Vmodel.QuestionTypeUpload),
                IsDeleted = false,
            };
            _db.QuestionBanks.Add(QuestionModel);
            _db.SaveChanges();

            // Saving the options
            var OptionOne = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionOne,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionOne);

            var OptionTwo = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionTwo,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionTwo);

            var OptionThree = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionThree,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionThree);

            var OptionFour = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionFour,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionFour);

            var answer = new AnswerBank()
            {
                QuestionID = QuestionModel.Id,
                AnswerText = Vmodel.CorrectAnswer
            };
            _db.AnswerBanks.Add(answer);

            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Fetching Question according the question category
        public QuestionBankVM GetQuestion(int questionId)
        {
            var model = _db.QuestionBanks.Where(x => x.Id == questionId).Select(b => new QuestionBankVM()
            {
                Id = b.Id,
                Question = b.Question,
                QuestionCategoryID = b.VaccancyID,
                QuestionType = b.QuestionType,
            }).FirstOrDefault();
            model.OptionBank = GetOptions(questionId);
            model.Answer = GetAnswer(questionId);
            return model;
        }

        // Saving a edited question to the question bank
        public bool SaveEditedQuestion(QuestionBankVM vmodel)
        {
            bool hasSaved = false;

            // Saving Question
            var questionModel = _db.QuestionBanks.FirstOrDefault(x => x.Id == vmodel.Id);
            questionModel.Question = vmodel.Question;
            questionModel.VaccancyID = vmodel.QuestionCategoryID;
            _db.Entry(questionModel).State = EntityState.Modified;
            _db.SaveChanges();

            //Saving Options
            if (vmodel.OptionBank != null)
            {
                foreach (var option in vmodel.OptionBank)
                {
                    var model = _db.OptionBanks.FirstOrDefault(x => x.Id == option.Id);
                    model.OptionText = option.OptionTxt;
                    _db.Entry(model).State = EntityState.Modified;
                }
                _db.SaveChanges();
            }

            var answer = _db.AnswerBanks.FirstOrDefault(x => x.Id == vmodel.Answer.Id);
            answer.AnswerText = vmodel.Answer.AnswerText;
            _db.Entry(answer).State = EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;

            return hasSaved;
        }

        // Deleting a question from the question bank
        public void DeleteQuestion(int questionId)
        {
            var model = _db.QuestionBanks.FirstOrDefault(x => x.Id == questionId);
            model.IsDeleted = true;
            var questionMappingModel = _db.VaccancyQuestionMappings.Where(x => x.QuestionID == questionId).ToList();
            foreach(var each in questionMappingModel)
            {
                each.IsDeleted = true;
            }

            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }

        // Fetching all question for a test in the already selected one
        public VaccancyQuestionMappingVM GetSelectedQuestions(int ID)
        {
            var tableData = new List<QuestionMappingList>();
            var vaccancy = _db.Vaccancies.Where(x => x.Id == ID).FirstOrDefault();

            var CheckQuestionExists = _db.VaccancyQuestionMappings.Where(x => x.VaccancyID == ID).Count();
            if (CheckQuestionExists == 0)
            {
                tableData = _db.QuestionBanks.Where(x => x.IsDeleted == false && x.VaccancyID == ID).Select(o => new QuestionMappingList()
                {
                    Id = o.Id,
                    VaccancyID = ID,
                    Vaccancy = vaccancy.Description,
                    QuestionID = o.Id,
                    Question = o.Question,
                    QuestionType = o.QuestionType,
                    IsSelected = false
                }).ToList();
                foreach (var item in tableData)
                {
                    item.Options = GetOptions(item.QuestionID);
                    item.Answer = GetAnswer(item.QuestionID);
                }
            }
            else
            {
                var Questions = _db.QuestionBanks.Where(x => x.IsDeleted == false);
                var VaccancyQuestionMapping = _db.VaccancyQuestionMappings.Where(x => x.VaccancyID == ID).Select(o => o.Question);

                var unavailableQuestions = Questions.Except(VaccancyQuestionMapping);
                foreach (var item in unavailableQuestions)
                {
                    var NewQuestion = new VaccancyQuestionMapping();
                    NewQuestion.VaccancyID = ID;
                    NewQuestion.QuestionID = item.Id;
                    NewQuestion.IsDeleted = false;
                    NewQuestion.DateCreated = DateTime.Now;
                    NewQuestion.IsSelected = false;
                    _db.VaccancyQuestionMappings.Add(NewQuestion);
                }
                _db.SaveChanges();
                tableData = _db.VaccancyQuestionMappings.Where(x => x.VaccancyID == ID).Select(o => new QuestionMappingList()
                {
                    Id = o.Id,
                    VaccancyID = ID,
                    Vaccancy = o.Vaccancy.Description,
                    QuestionID = o.Question.Id,
                    Question = o.Question.Question,
                    QuestionType = o.Question.QuestionType,
                    Mark = o.Mark,
                    IsSelected = o.IsSelected == true ? true : false,
                }).ToList();
                foreach (var item in tableData)
                {
                    item.Options = GetOptions(item.QuestionID);
                    item.Answer = GetAnswer(item.QuestionID);
                }
            }
            var model = new VaccancyQuestionMappingVM(tableData);
            model.Vaccancy = vaccancy.Description;
            return model;
        }

        // Checking if a question already exists in a question bank
        public int CheckQuestionExist(int vaccancyID, int QuestionID)
        {
            int result;
            result = _db.VaccancyQuestionMappings.Where(x => x.VaccancyID == vaccancyID && x.QuestionID == QuestionID).Count();
            return result;
        }

        // Saving the question in the question bank in respect to assessment tests
        public VaccancyQuestionMapping SaveQuestion(QuestionMappingList questionMapping)
        {
            var assessmentQuestionMapping = new VaccancyQuestionMapping();
            assessmentQuestionMapping.QuestionID = questionMapping.QuestionID;
            assessmentQuestionMapping.VaccancyID = questionMapping.VaccancyID;
            assessmentQuestionMapping.IsSelected = questionMapping.IsSelected;
            assessmentQuestionMapping.Mark = questionMapping.Mark;
            assessmentQuestionMapping.IsDeleted = false;
            assessmentQuestionMapping.DateCreated = DateTime.Now;
            assessmentQuestionMapping.AssignedUserID = Global.AuthenticatedUserID; ;
            _db.VaccancyQuestionMappings.Add(assessmentQuestionMapping);
            _db.SaveChanges();
            return assessmentQuestionMapping;
        }

        // Updating the assignment of question for assessment test
        public void AssignQuestion(QuestionMappingList questionMapping)
        {
            var assessmentQuestionMapping = _db.VaccancyQuestionMappings.Where(p => p.VaccancyID == questionMapping.VaccancyID && p.QuestionID == questionMapping.QuestionID).FirstOrDefault();
            assessmentQuestionMapping.IsSelected = questionMapping.IsSelected;
            assessmentQuestionMapping.Mark = questionMapping.Mark;
            assessmentQuestionMapping.EditedUserID = Global.AuthenticatedUserID;
            _db.Entry(assessmentQuestionMapping).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
    }
}