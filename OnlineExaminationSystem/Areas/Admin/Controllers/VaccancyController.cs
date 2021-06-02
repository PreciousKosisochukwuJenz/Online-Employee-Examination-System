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


    public class VaccancyController : Controller
    {
        #region Instanciation
        IApplicationSettingsService _settingService;
        public VaccancyController()
        {
            _settingService = new ApplicationSettingsService(new OnlineExaminationDatabaseEntities());
        }
        public VaccancyController(ApplicationSettingsService settingsService)
        {
            _settingService = settingsService;
        }
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        #endregion

        // Vaccany
        public ActionResult ManageVaccancies(bool? Added, bool? Editted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Vaccany added successfully.";
                TempData["icon"] = "fa fa-check";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Vaccany updated successfully.";
                TempData["icon"] = "fa fa-check";
            }
            ViewBag.Vaccanies = _settingService.GetVaccanies();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateVaccancy(VacancyVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.CreateVaccany(vmodel);
            }
            return RedirectToAction("ManageVaccancies", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditVaccancy(VacancyVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.EditVaccany(vmodel);
            }
            return RedirectToAction("ManageVaccancies", new { Editted = hasSaved });
        }
        public JsonResult GetVaccancy(int id)
        {
            var model = _settingService.GetVaccany(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteVaccancy(int id)
        {
            var model = _settingService.DeleteVaccany(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}