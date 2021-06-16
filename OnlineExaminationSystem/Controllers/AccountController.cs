using OnlineExaminationSystem.Areas.Admin.Services;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AccountController : Controller
    {       // Instantiation
        #region Instantiation
        private OnlineExaminationDatabaseEntities db = new OnlineExaminationDatabaseEntities();
        public readonly UserService _userService;
        public AccountController()
        {
            _userService = new UserService(new OnlineExaminationDatabaseEntities());
        }
        public AccountController(UserService userService)
        {
            _userService = userService;
        }
        #endregion
        // Action Methods
        #region Action Methods
        [AllowAnonymous]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AdminLogin(UserVM userVM)
        {
            Session.Clear();
            var user = _userService.CheckCreditials(userVM);
            if (user.Count > 0)
            {
                LoginSession(user);
                if (Session["UserId"] != null)
                {
                    Global.AuthenticatedUserID = Convert.ToInt32(Session["UserId"].ToString());
                    var UserID = Convert.ToInt32(Session["UserId"]);
                    var RoleID = db.Users.Where(x => x.Id == UserID).FirstOrDefault().RoleID;
                    var PermissionList = db.RolePermissions.Where(x => x.RoleID == RoleID).ToList();
                    // User management
                    Session["ManageUsers"] = PermissionList.Where(p => p.Permission.Description == "Manage users").Count() == 0 ? "false" : PermissionList.Where(x => x.Permission.Description == "Manage users").FirstOrDefault().IsAssigned.ToString();
                    Session["ManageRoles"] = PermissionList.Where(p => p.Permission.Description == "Manage roles").Count() == 0 ? "false" : PermissionList.Where(x => x.Permission.Description == "Manage roles").FirstOrDefault().IsAssigned.ToString();

                    if (Session["ManageUsers"].ToString() == "True" || Session["ManageRoles"].ToString() == "True")
                    {
                        Session["UserManagement"] = "True";
                    }
                    else
                    {
                        Session["UserManagement"] = "False";
                    }
                    Session["ManageAssessment"] = PermissionList.Where(p => p.Permission.Description == "Manage assessments").Count() == 0 ? "false" : PermissionList.Where(x => x.Permission.Description == "Manage assessments").FirstOrDefault().IsAssigned.ToString();
                    if (Session["ManageAssessment"].ToString() == "True")
                    {
                        Session["AssessmentManagement"] = "True";
                    }
                    else
                    {
                        Session["AssessmentManagement"] = "False";
                    }

                    Session["ManageApplicants"] = PermissionList.Where(p => p.Permission.Description == "Manage Applicants").Count() == 0 ? "false" : PermissionList.Where(x => x.Permission.Description == "Manage Applicants").FirstOrDefault().IsAssigned.ToString();
                    if (Session["ManageApplicants"].ToString() == "True")
                    {
                        Session["ApplicantManagement"] = "True";
                    }
                    else
                    {
                        Session["ApplicantManagement"] = "False";
                    }

                    Session["ManageVaccancy"] = PermissionList.Where(p => p.Permission.Description == "Manage Vaccancy").Count() == 0 ? "false" : PermissionList.Where(x => x.Permission.Description == "Manage Vaccancy").FirstOrDefault().IsAssigned.ToString();
                    if (Session["ManageVaccancy"].ToString() == "True")
                    {
                        Session["VaccancyManagement"] = "True";
                    }
                    else
                    {
                        Session["VaccancyManagement"] = "False";
                    }
                    // Settings management
                    Session["ApplicationSettings"] = PermissionList.Where(p => p.Permission.Description == "Application settings").Count() == 0 ? "false" : PermissionList.Where(x => x.Permission.Description == "Application settings").FirstOrDefault().IsAssigned.ToString();
                    if (Session["ApplicationSettings"].ToString() == "True")
                    {
                        Session["SettingsManagement"] = "True";
                    }
                    else
                    {
                        Session["SettingsManagement"] = "False";
                    }
                 
                }
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Invalid username and password";
                TempData["icon"] = "fa fa-times";
            }
            return View(userVM);
        }
        [AllowAnonymous]
        public ActionResult ClientLogin()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ClientLogin(UserVM userVM)
        {
            Session.Clear();
            var applicant = _userService.CheckApplicantCreditials(userVM);
            if (applicant.Count > 0)
            {
                ApplicantLoginSession(applicant);
                Global.AuthenticatedApplicantID = Convert.ToInt32(Session["ApplicantID"].ToString());
                return RedirectToAction("Start", "Test");
            }
            else
            {
                ViewBag.ShowAlert = true;
                TempData["message"] = "  Invalid username and password";
                TempData["icon"] = "fa fa-times";
            }
            return View(userVM);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                Session.Clear();
                return RedirectToAction("AdminLogin", "Account", new { area = "" });

            }
            if (Session["ApplicantID"] != null)
            {
                Session.Clear();
                return RedirectToAction("ClientLogin", "Account", new { area = "" });

            }
            return View();
        }
        [AllowAnonymous]
        public JsonResult CheckSessionExists()
        {
            bool IsExisting = false;
            if (Session["UserId"] == null)
                IsExisting = false;
            else
                IsExisting = true;

            return Json(IsExisting, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Registration()
        {
            ViewBag.Vaccancy = new SelectList(db.Vaccancies.Where(x => x.IsDeleted == false), "Id", "Description");

            return View();
        }
        
        [AllowAnonymous,HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(ApplicantVM vmodel,HttpPostedFileBase passport, HttpPostedFileBase doc)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.RegisterApplicant(vmodel, passport, doc);
                if (hasSaved)
                    return View("_Success");
            }
            ViewBag.Vaccancy = new SelectList(db.Vaccancies.Where(x => x.IsDeleted == false), "Id", "Description");
            return View(vmodel);
        }
        #endregion
        // Methods
        #region Methods
        public void LoginSession(List<User> user)
        {
            var Firstname = user.FirstOrDefault().Firstname;
            var Lastname = user.FirstOrDefault().Lastname;
            var DateCreated = user.FirstOrDefault().DateCreated;
            var Email = user.FirstOrDefault().Email;
            var Username = user.FirstOrDefault().Username;
            var ID = user.FirstOrDefault().Id;
            var Role = user.FirstOrDefault().Role.Description;

            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, Firstname),
                    new Claim(ClaimTypes.Name, Lastname),
                    new Claim(ClaimTypes.DateOfBirth, DateCreated.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.PrimarySid, ID.ToString()),
                    new Claim(ClaimTypes.Name, Username)
                },
                "ApplicationCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
            Session["UserId"] = ID.ToString();
            Session["Username"] = Username;
            Session["DateCreated"] = Convert.ToDateTime(DateCreated).ToLongDateString();
            Session["Name"] = Firstname + " " + Lastname;
            Session["Role"] = Role;
        }
        public void ApplicantLoginSession(List<Applicant> user)
        {
            var Surname = user.FirstOrDefault().Surname;
            var Firstname = user.FirstOrDefault().Firstname;
            var Lastname = user.FirstOrDefault().Lastname;
            var DateCreated = user.FirstOrDefault().DateCreated;
            var Email = user.FirstOrDefault().EmailAddress;
            var ID = user.FirstOrDefault().Id;

            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, Surname),
                    new Claim(ClaimTypes.Name, Firstname),
                    new Claim(ClaimTypes.Name, Lastname),
                    new Claim(ClaimTypes.DateOfBirth, DateCreated.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.PrimarySid, ID.ToString()),
                },
                "ApplicationCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
            Session["ApplicantID"] = ID.ToString();
            Session["DateCreated"] = Convert.ToDateTime(DateCreated).ToLongDateString();
            Session["Name"] = Surname + " " + Firstname + " " + Lastname;
            Session["Email"] = Email; 
        }

        #endregion
    }
}