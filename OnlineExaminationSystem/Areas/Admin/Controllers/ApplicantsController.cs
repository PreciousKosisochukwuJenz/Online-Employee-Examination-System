using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.Services;
using OnlineExaminationSystem.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Areas.Admin.Controllers
{
    public class ApplicantsController : Controller
    {
        #region Instanciation
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        IUserService _userService;
        public ApplicantsController()
        {
            _userService = new UserService(new OnlineExaminationDatabaseEntities());
        }
        public ApplicantsController(UserService userService)
        {
            _userService = userService;
        }
        #endregion     
        public ActionResult ManageApplicants()
        {
            ViewBag.Applicants = _userService.GetApplicants();
            return View();
        }

        public ActionResult DeleteApplicant(int id)
        {
            var model = _userService.DeleteApplicant(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}