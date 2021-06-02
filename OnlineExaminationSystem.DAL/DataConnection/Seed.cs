using OnlineExaminationSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.DataConnection
{
    public class Seed
    {
        public static void DatabaseSeed(OnlineExaminationDatabaseEntities context)
        {
            byte[] empty = { 4,3 };
            context.ApplicationSettings.AddOrUpdate(x => x.Id,
          new Entity.ApplicationSettings()
          {
              Id = 1,
              AppName = "Online Examination System",
              Logo = empty,
              Favicon = empty
          });
            context.Permissions.AddOrUpdate(x => x.Id, new Entity.Permission()
            {
                Id = 1,
                Description = "Settings module",
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 2,
                Description = "User module",
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 3,
                Description = "Application settings",
                ParentID = 1,
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 4,
                Description = "Manage roles",
                ParentID = 2,
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 5,
                Description = "Manage users",
                ParentID = 2,
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 6,
                Description = "Vaccancy module",
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 7,
                Description = "Manage Vaccancy",
                ParentID = 6,
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 8,
                Description = "Assessment module",
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 9,
                Description = "Manage assessments",
                ParentID = 8,
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 10,
                Description = "Applicant module",
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            },
            new Entity.Permission()
            {
                Id = 11,
                Description = "Manage Applicants",
                ParentID = 10,
                IsDeleted = false,
                DateCreated = DateTime.Now.Date,
            });
            context.Roles.AddOrUpdate(x => x.Id, new Entity.Role()
            {
                Id = 1,
                Description = "Super Administrator",
                DateCreated = DateTime.Now.Date,
                IsDeleted = false
            });
            context.Users.AddOrUpdate(x => x.Id,
                new Entity.User() {
                    Id = 1,
                    RoleID = 1,
                    DateCreated = DateTime.Now.Date,
                    PhoneNumber = "09033333333",
                    Username = "Administrator",
                    Lastname = "Admin",
                    Firstname = "Main",
                    IsActive = true,
                    IsDeleted = false,
                    Password = CustomEncrypt.Encrypt("Admin"),
                    PasswordSalt = CustomEncrypt.Encrypt("Admin"),
                    Email = "admin@site.com",
                    Address = "St, Theresa's parish"
                });
            context.RolePermissions.AddOrUpdate(x => x.Id, new Entity.RolePermission()
            {
                Id = 1,
                RoleID = 1,
                IsAssigned = true,
                DateCreated = DateTime.Now.Date,
                IsDeleted = false,
                PermissionID = 4,
                PermissionParentID = 2,
            });
          
            context.SaveChanges();
        }
    }
}
