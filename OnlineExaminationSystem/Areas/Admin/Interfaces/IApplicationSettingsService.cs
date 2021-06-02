using OnlineExaminationSystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.Interfaces
{
    interface IApplicationSettingsService
    {
        ApplicationSettingsVM GetApplicationSettings();
        bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Favicon);

        List<VacancyVM> GetVaccanies();
        bool CreateVaccany(VacancyVM vmodel);
        VacancyVM GetVaccany(int ID);
        bool EditVaccany(VacancyVM vmodel);
        bool DeleteVaccany(int ID);

    }
}
