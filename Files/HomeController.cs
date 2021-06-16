using OnlineExaminationSystem.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var newApplicants = db.Applicants.Where(x => x.IsDeleted == false && DateTime.Today == x.DateCreated).ToList();
            ViewBag.UserCount = db.Users.Where(x => x.IsActive == true && x.IsDeleted == false).Count();
            ViewBag.ApplicantCount = db.Applicants.Where(x => x.IsDeleted == false).Count();
            ViewBag.RoleCount = db.Roles.Where(x => x.IsDeleted == false).Count();
            ViewBag.VaccancyCount = db.Vaccancies.Where(x => x.IsDeleted == false).Count();
            ViewBag.NewApplicants = newApplicants;
            return View();
        }

        public DateTime parsedDate(DateTime dateTime)
        {
            return dateTime.Date;
        }
    }
}