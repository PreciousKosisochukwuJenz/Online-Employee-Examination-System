using OnlineExaminationSystem.DAL.Entity;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.Interfaces
{
    interface IUserService
    {
        List<UserVM> GetUsers();
        bool CreateUser(UserVM vmodel);
        UserVM GetUser(int ID);
        bool EditUser(UserVM vmodel);
        bool DeleteUser(int ID);
        bool DeactivateUser(int ID);
        bool ActivateUser(int ID);

        //Roles
        List<RoleVM> GetRoles();
        bool CreateRole(RoleVM vmodel);
        RoleVM GetRole(int ID);
        bool EditRole(RoleVM vmodel);
        bool DeleteRole(int ID);
        RolePermissionVM GetAssignedPermission(int ID);


        //Role Permission
        RolePermission SavePermission(RolePermissionList rolePermissionVM);
        void AssignPermission(RolePermissionList rolePermissionVM);
        int CheckPermissionExist(int RoleID, int PermissionID);

        List<User> CheckCreditials(UserVM userVM);
        List<Applicant> CheckApplicantCreditials(UserVM userVM);

        List<ApplicantVM> GetApplicants();
        ApplicantVM GetApplicant(int id);
        bool RegisterApplicant(ApplicantVM vmodel, HttpPostedFileBase passport, HttpPostedFileBase doc);
        bool DeleteApplicant(int id);

    }
}
