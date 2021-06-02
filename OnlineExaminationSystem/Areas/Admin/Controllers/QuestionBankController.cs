using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Entity;
using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.Services;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Areas.Admin.Controllers
{
    public class QuestionBankController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            filterContext.ExceptionHandled = true;

            //Log the error!!
            log.Error("Error trying to do something", filterContext.Exception);
            Global.ErrorMessage = filterContext.Exception.ToString();

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Error", "Error", new { area = "" });

        }

        #region Instantiation
        private OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        private IQuestionBankService _questionBankService;
        private IApplicationSettingsService _settingsService;
        public QuestionBankController()
        {
            _questionBankService = new QuestionBankService(new OnlineExaminationDatabaseEntities());
            _settingsService = new ApplicationSettingsService(new OnlineExaminationDatabaseEntities());
        }
        public QuestionBankController(QuestionBankService questionBankService,ApplicationSettingsService settingsService)
        {
            _questionBankService = questionBankService;
            _settingsService = settingsService;
        }
        OnlineExaminationDatabaseEntities _entities = new OnlineExaminationDatabaseEntities();
        #endregion



        [HttpPost]
        public ActionResult UploadQuestion(HttpPostedFileBase file, int VaccancyID)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var status = UploadQuestionCSVFile(file, VaccancyID);
                if (status == true)
                    hasSaved = true;
            }
            return RedirectToAction("ManageQuestion", new { VaccancyID = VaccancyID, Saved = hasSaved });
        }

        public ActionResult AddQuestion(int vaccancyID, bool? saved)
        {
            if (saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Questions added successfully.";
                TempData["icon"] = "fa fa-check";
            }
            ViewBag.VaccanyID = _settingsService.GetVaccany(vaccancyID);

            var model = new QuestionBankVM()
            {
                QuestionCategoryID = vaccancyID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(QuestionBankVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var status = _questionBankService.SaveNewQuestion(vmodel);
                if (status.Equals(true))
                    hasSaved = true;
                return RedirectToAction("AddQuestion", new { VaccancyID = vmodel.QuestionCategoryID, saved = hasSaved });
            }
            return View();
        }

        public ActionResult EditQuestion(int QuestionID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Questions updated successfully.";
                TempData["icon"] = "fa fa-check";
            }
            var model = _questionBankService.GetQuestion(QuestionID);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(QuestionBankVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var status = _questionBankService.SaveEditedQuestion(vmodel);
                if (status.Equals(true))
                    hasSaved = true;
            }
            return RedirectToAction("EditQuestion", "QuestionBank", new { Saved = hasSaved, QuestionID = vmodel.Id });
        }

        public JsonResult DeleteQuestion(int id)
        {
            _questionBankService.DeleteQuestion(id);
            return Json(JsonRequestBehavior.AllowGet);
        }
        #region Functions

        public QuestionBankVM ReadBatchQuestionCSVFile(HttpPostedFileBase file)
        {
            var model = new QuestionBankVM();
            string data = "";
            if (file != null && file.ContentLength > 0)
            {
                string filename = file.FileName;
                if (file.FileName.EndsWith(".csv"))
                {
                    List<QuestionBankVM> vmodel = new List<QuestionBankVM>();
                    Stream stream = file.InputStream;
                    DataTable csvTable = new DataTable();
                    using (CsvReader csvReader = new CsvReader(new StreamReader(stream), true))
                    {
                        csvTable.Load(csvReader);
                    }
                    foreach (DataRow item in csvTable.Rows)
                    {
                        var questionVM = new QuestionBankVM()
                        {
                            Question = item.ItemArray[0].ToString(),
                            QuestionTypeUpload = item.ItemArray[1].ToString(),
                            OptionOne = item.ItemArray[2].ToString(),
                            OptionTwo = item.ItemArray[3].ToString(),
                            OptionThree = item.ItemArray[4].ToString(),
                            OptionFour = item.ItemArray[5].ToString(),
                            CorrectAnswer = item.ItemArray[6].ToString(),
                        };
                        vmodel.Add(questionVM);
                    }
                    model = new QuestionBankVM(vmodel);
                    return model;
                }
                else
                {
                    data = "File, This file format is not supported";
                    ViewBag.ErrorMessage = data;
                    return model;
                }
            }
            else
            {
                data = "File, Please Upload Your file";
                ViewBag.ErrorMessage = data;
                return model;
            }
        }

        public bool UploadQuestionCSVFile(HttpPostedFileBase file, int? QuestionCategoryID)
        {
            bool hasSaved = false;
            var items = ReadBatchQuestionCSVFile(file);
            foreach (var item in items.TableData)
            {
                item.QuestionCategoryID = QuestionCategoryID;
                _questionBankService.SaveNewQuestionUpload(item);
                hasSaved = true;
            }
            return hasSaved;
        }

        public ActionResult ManageQuestion(int VaccancyID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Question map updated successfully.";
                TempData["icon"] = "fa fa-check";
            }
            ViewBag.VaccancyID = VaccancyID;
            return View(_questionBankService.GetSelectedQuestions(VaccancyID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageQuestion(VaccancyQuestionMappingVM assessmentQuestionVM)
        {
            bool hasSaved = false;
            var model = new VaccancyQuestionMappingVM();
            int VaccancyID = 0;
            if (ModelState.IsValid)
            {
                var assessmentQuestionMapping = new VaccancyQuestionMapping();
                foreach (var item in assessmentQuestionVM.TableDataSource)
                {
                    VaccancyID = item.VaccancyID;
                    if (_questionBankService.CheckQuestionExist(item.VaccancyID, item.QuestionID) == 0)
                    {
                        assessmentQuestionMapping = _questionBankService.SaveQuestion(item);
                    }
                    else
                    {
                        _questionBankService.AssignQuestion(item);
                    }
                }
            }
            hasSaved = true;
            return ManageQuestion(VaccancyID, hasSaved);
        }
        #endregion
    }
}