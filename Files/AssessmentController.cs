using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.Services;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using OnlineExaminationSystem.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Areas.Admin.Controllers
{
    public class AssessmentController : Controller
    {
        #region Instanciation
        IApplicationSettingsService _settingService;
        IAssessmentService _assessmentService;
        public AssessmentController()
        {
            _settingService = new ApplicationSettingsService(new OnlineExaminationDatabaseEntities());
            _assessmentService = new AssessmentService(new OnlineExaminationDatabaseEntities(),new QuestionBankService());
        }
        public AssessmentController(ApplicationSettingsService settingsService,AssessmentService assessmentService)
        {
            _settingService = settingsService;
            _assessmentService = assessmentService;
        }
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        #endregion

        public ActionResult ManageAssessments(bool? Added, bool? Editted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Assessment added successfully.";
                TempData["icon"] = "fa fa-check";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Assessment updated successfully.";
                TempData["icon"] = "fa fa-check";
            }
            ViewBag.Vaccancy = new SelectList(db.Vaccancies.Where(x => x.IsDeleted == false), "Id", "Description");
            ViewBag.Assessments = _assessmentService.GetAssessments();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateAssessment(AssessmentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _assessmentService.CreateAssessment(vmodel);
            }
            return RedirectToAction("ManageAssessments", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditAssessment(AssessmentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _assessmentService.UpdateAssessment(vmodel);
            }
            return RedirectToAction("ManageAssessments", new { Editted = hasSaved });
        }
        public JsonResult GetAssessment(int id)
        {
            var model = _assessmentService.GetAssessment(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAssessment(int id)
        {
            var model = _assessmentService.DeleteAssessment(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssessmentLog(int id)
        {
            ViewBag.Log = _assessmentService.GetAssessmentLog(id);
            return View();
        }

        public ActionResult ViewApplicantAnswerLog(int ApplicantID, int TestID)
        {
            return View(_assessmentService.GetApplicantTest(ApplicantID, TestID));
        }
    }
}