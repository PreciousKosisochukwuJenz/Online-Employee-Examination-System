using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Helpers;
using OnlineExaminationSystem.DAL.Entity;
using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using OnlineExaminationSystem.Areas.Admin.Helpers;

namespace OnlineExaminationSystem.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        // Instanciation Process
        #region Instanciation
        readonly OnlineExaminationDatabaseEntities _db;
        public UserService()
        {
            _db = new OnlineExaminationDatabaseEntities();
        }
        public UserService(OnlineExaminationDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        //User
        public List<UserVM> GetUsers()
        {
            var model = _db.Users.Where(x => x.IsDeleted == false).Select(b => new UserVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Email = b.Email,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Role = b.Role.Description,
                Username = b.Username,
                DateCreated = b.DateCreated,
                IsActive = b.IsActive
            }).ToList();
            return model;
        }
        public bool CreateUser(UserVM vmodel)
        {
            bool HasSaved = false;
            User model = new User()
            {
                Username = vmodel.Username,
                Firstname = vmodel.Firstname,
                Lastname = vmodel.Lastname,
                Email = vmodel.Email,
                PhoneNumber = vmodel.PhoneNumber,
                RoleID = vmodel.RoleID,
                Address = vmodel.Address,
                IsActive = true,
                IsDeleted = false,
                Password = CustomEncrypt.Encrypt(vmodel.Password),
                PasswordSalt = CustomEncrypt.Encrypt(vmodel.PasswordSalt),
                DateCreated = DateTime.Now,
            };
            _db.Users.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
        public UserVM GetUser(int ID)
        {
            var model = _db.Users.Where(x => x.Id == ID).Select(b => new UserVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Email = b.Email,
                Username = b.Username,
                PhoneNumber = b.PhoneNumber,
                Address = b.Address,
                RoleID = b.RoleID,
            }).FirstOrDefault();
            return model;
        }
        public bool EditUser(UserVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == vmodel.Id);
            model.Firstname = vmodel.Firstname;
            model.Lastname = vmodel.Lastname;
            model.Email = vmodel.Email;
            model.PhoneNumber = vmodel.PhoneNumber;
            model.Address = vmodel.Address;
            model.RoleID = vmodel.RoleID;
            model.DateModified = DateTime.Now;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
        public bool DeleteUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public bool DeactivateUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsActive = false;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public bool ActivateUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsActive = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        //Role
        public List<RoleVM> GetRoles()
        {
            var model = _db.Roles.Where(x => x.IsDeleted == false).Select(b => new RoleVM()
            {
                Id = b.Id,
                Description = b.Description
            }).ToList();
            return model;
        }
        public bool CreateRole(RoleVM vmodel)
        {
            bool HasSaved = false;
            Role role = new Role()
            {
                Description = vmodel.Description,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.Roles.Add(role);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
        public RoleVM GetRole(int ID)
        {
            var model = _db.Roles.Where(x => x.Id == ID).Select(b => new RoleVM()
            {
                Id = b.Id,
                Description = b.Description
            }).FirstOrDefault();
            return model;
        }
        public bool EditRole(RoleVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Roles.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Description = vmodel.Description;
            model.DateModified = DateTime.Now;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
        public bool DeleteRole(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Roles.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public RolePermissionVM GetAssignedPermission(int ID)
        {
            var tableData = new List<RolePermissionList>();
            var RoleName = _db.Roles.Where(x => x.Id == ID).FirstOrDefault().Description;
            var CheckPermissionExists = _db.RolePermissions.Where(x => x.RoleID == ID).Count();
            if (CheckPermissionExists == 0)
            {
                tableData = _db.Permissions.Where(x => x.IsDeleted == false && x.ParentID != null).Select(o => new RolePermissionList()
                {
                    Id = o.Id,
                    RoleID = ID,
                    RoleName = RoleName,
                    PermissionID = o.Id,
                    PermissionName = o.Description,
                    PermissionParentID = o.ParentID,
                    IsAssigned = false
                }).ToList();
            }
            else
            {
                var permission = _db.Permissions.Where(x => x.IsDeleted == false);
                var rolepermission = _db.RolePermissions.Where(x => x.RoleID == ID).Select(o => o.Permission);

                var unavailablePermission = permission.Except(rolepermission);
                foreach (var item in unavailablePermission)
                {
                    var Newpermission = new RolePermission();
                    Newpermission.RoleID = ID;
                    Newpermission.PermissionID = item.Id;
                    Newpermission.PermissionParentID = item.ParentID;
                    Newpermission.IsDeleted = false;
                    Newpermission.IsAssigned = false;
                    Newpermission.DateCreated = DateTime.Now;
                    _db.RolePermissions.Add(Newpermission);
                }
                _db.SaveChanges();
                tableData = _db.RolePermissions.Where(x => x.RoleID == ID && x.PermissionParentID != null).Select(o => new RolePermissionList()
                {
                    Id = o.Id,
                    RoleID = ID,
                    RoleName = o.Role.Description,
                    PermissionID = o.Permission.Id,
                    PermissionName = o.Permission.Description,
                    PermissionParentID = o.PermissionParentID,
                    IsAssigned = o.IsAssigned == true ? true : false,
                }).ToList();
            }
            var model = new RolePermissionVM(tableData);
            model.RoleName = RoleName;
            return model;
        }

        //Role Permission
        public RolePermission SavePermission(RolePermissionList rolePermissionVM)
        {
            var rolePermission = new RolePermission();
            rolePermission.PermissionID = rolePermissionVM.PermissionID;
            rolePermission.RoleID = rolePermissionVM.RoleID;
            rolePermission.PermissionParentID = rolePermissionVM.PermissionParentID;
            rolePermission.IsAssigned = rolePermissionVM.IsAssigned;
            rolePermission.IsDeleted = false;
            rolePermission.DateCreated = DateTime.Now;
            _db.RolePermissions.Add(rolePermission);
            _db.SaveChanges();
            return rolePermission;
        }
        public void AssignPermission(RolePermissionList rolePermissionVM)
        {
            var RolePermission = _db.RolePermissions.Where(p => p.RoleID == rolePermissionVM.RoleID && p.PermissionID == rolePermissionVM.PermissionID).FirstOrDefault();
            RolePermission.IsAssigned = rolePermissionVM.IsAssigned;
            _db.Entry(RolePermission).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public int CheckPermissionExist(int RoleID, int PermissionID)
        {
            int result;
            result = _db.RolePermissions.Where(x => x.RoleID == RoleID && x.PermissionID == PermissionID).Count();
            return result;
        }
        //Account
        public List<User> CheckCreditials(UserVM userVM)
        {
            var ecryptedPassword = CustomEncrypt.Encrypt(userVM.Password);
            var user = _db.Users.Where(x => x.Username == userVM.Username && x.Password == ecryptedPassword && x.IsDeleted == false && x.IsActive == true).ToList();
            return user;
        }
        public List<Applicant> CheckApplicantCreditials(UserVM userVM)
        {
            var ecryptedPassword = CustomEncrypt.Encrypt(userVM.Password);
            var user = _db.Applicants.Where(x => x.EmailAddress == userVM.Username && x.Password == ecryptedPassword && x.IsDeleted == false).ToList();
            return user;
        }

        public List<ApplicantVM> GetApplicants()
        {
            var model = _db.Applicants.Where(x => x.IsDeleted == false).Select(b => new ApplicantVM() { 
                Id = b.Id,
                Surname = b.Surname,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Gender = b.Gender,
                DateOfBirth = b.DateOfBirth,
                PhoneNumber = b.PhoneNumber,
                EmailAddress = b.EmailAddress,
                Address = b.Address,
                LGA  = b.LGA,
                Nationality = b.Nationality,
                State = b.State,
                VaccanyID = b.VaccancyID,
                Vaccancy = b.Vaccancy.Description,
                Photo = b.Photo
            }).OrderByDescending(o => o.Id).ToList();
            return model;
        }
        public ApplicantVM GetApplicant(int id)
        {
            var model = _db.Applicants.Where(x => x.Id == id).Select(b => new ApplicantVM()
            {
                Id = b.Id,
                Surname = b.Surname,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Gender = b.Gender,
                DateOfBirth = b.DateOfBirth,
                PhoneNumber = b.PhoneNumber,
                EmailAddress = b.EmailAddress,
                Address = b.Address,
                LGA = b.LGA,
                Nationality = b.Nationality,
                State = b.State,
                VaccanyID = b.VaccancyID,
                Vaccancy = b.Vaccancy.Description
            }).FirstOrDefault();
            return model;
        }
        public bool RegisterApplicant(ApplicantVM vmodel, HttpPostedFileBase passport,HttpPostedFileBase doc)
        {
            var model = new Applicant()
            {
                Surname = vmodel.Surname,
                Firstname = vmodel.Firstname,
                Lastname = vmodel.Lastname,
                Gender = vmodel.Gender,
                DateOfBirth = vmodel.DateOfBirth,
                VaccancyID = vmodel.VaccanyID,
                Address = vmodel.Address,
                PhoneNumber = vmodel.PhoneNumber,
                Nationality = vmodel.Nationality,
                State = vmodel.State,
                LGA = vmodel.LGA,
                EmailAddress = vmodel.EmailAddress,
                Password = CustomEncrypt.Encrypt(vmodel.Password),
                PasswordSalt = CustomEncrypt.Encrypt(vmodel.PasswordSalt),
                IsDeleted = false,
                DateCreated = DateTime.Now.Date
            };
            if(passport != null)
                model.Photo = CustomSerializer.Serialize(passport);
            if (doc != null)
                model.CV = CustomSerializer.Serialize(doc);

            _db.Applicants.Add(model);
            _db.SaveChanges();
            return true;
        }
        public bool DeleteApplicant(int id)
        {
            var model = _db.Applicants.FirstOrDefault(x => x.Id == id);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
    }
}