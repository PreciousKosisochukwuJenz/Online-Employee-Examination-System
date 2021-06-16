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
    public class ApplicationSettingsController : Controller
    {
        #region Instanciation
        IApplicationSettingsService _settingService;
        public ApplicationSettingsController()
        {
            _settingService = new ApplicationSettingsService(new OnlineExaminationDatabaseEntities());
        }
        public ApplicationSettingsController(ApplicationSettingsService settingsService)
        {
            _settingService = settingsService;
        }
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        #endregion

        public ActionResult Manage()
        {
            return View(_settingService.GetApplicationSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ApplicationSettingsVM Vmodel, HttpPostedFileBase LogoData, HttpPostedFileBase FaviconData)
        {
            if (ModelState.IsValid)
            {
                bool saveState = _settingService.UpdateApplicationSettings(Vmodel, LogoData, FaviconData);
                if (saveState == true)
                {
                    ViewBag.ShowAlert = true;
                    TempData["message"] = "  Application settings updated successfully.";
                    TempData["icon"] = "fa fa-check";
                }
            }
            return View(_settingService.GetApplicationSettings());
        }
        
      
    }
}