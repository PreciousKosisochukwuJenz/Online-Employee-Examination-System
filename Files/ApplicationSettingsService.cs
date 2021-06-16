using OnlineExaminationSystem.Areas.Admin.Helpers;
using OnlineExaminationSystem.Areas.Admin.Interfaces;
using OnlineExaminationSystem.Areas.Admin.ViewModels;
using OnlineExaminationSystem.DAL.DataConnection;
using OnlineExaminationSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        // Instanciation Process
        #region Instanciation
        readonly OnlineExaminationDatabaseEntities _db;
        public ApplicationSettingsService()
        {
            _db = new OnlineExaminationDatabaseEntities();
        }
        public ApplicationSettingsService(OnlineExaminationDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        public ApplicationSettingsVM GetApplicationSettings()
        {
            byte[] emptyArr = { 4, 3 };
            var model = _db.ApplicationSettings.FirstOrDefault();
            var Vmodel = new ApplicationSettingsVM()
            {
                ID = model.Id,
                AppName = model.AppName,
                Logo = model.Logo == null ? emptyArr : model.Logo,
                Favicon = model.Favicon == null ? emptyArr : model.Favicon,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                LinkedInHandle = model.LinkedInHandle,
                FacebookHandle = model.FacebookHandle,
                InstagramHandle = model.InstagramHandle,
                TwitterHandle = model.TwitterHandle,
            };
            return Vmodel;
        }
        public bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Favicon)
        {
            bool hasSucceed = false;
            var model = _db.ApplicationSettings.FirstOrDefault(x => x.Id == Vmodel.ID);
            model.AppName = Vmodel.AppName;
            if (Logo != null)
                model.Logo = CustomSerializer.Serialize(Logo);
            if (Favicon != null)
                model.Favicon = CustomSerializer.Serialize(Favicon);
            model.Address = Vmodel.Address;
            model.PhoneNumber = Vmodel.PhoneNumber;
            model.InstagramHandle = Vmodel.InstagramHandle;
            model.FacebookHandle = Vmodel.FacebookHandle;
            model.TwitterHandle = Vmodel.TwitterHandle;
            model.LinkedInHandle = Vmodel.LinkedInHandle;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            var state = _db.SaveChanges();
            if (state > 0)
            {
                hasSucceed = true;
            }
            return hasSucceed;
        }

        //Role
        public List<VacancyVM> GetVaccanies()
        {
            var model = _db.Vaccancies.Where(x => x.IsDeleted == false).Select(b => new VacancyVM()
            {
                Id = b.Id,
                Description = b.Description
            }).ToList();
            return model;
        }
        public bool CreateVaccany(VacancyVM vmodel)
        {
            bool HasSaved = false;
            Vaccancy model = new Vaccancy()
            {
                Description = vmodel.Description,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.Vaccancies.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
        public VacancyVM GetVaccany(int ID)
        {
            var model = _db.Vaccancies.Where(x => x.Id == ID).Select(b => new VacancyVM()
            {
                Id = b.Id,
                Description = b.Description
            }).FirstOrDefault();
            return model;
        }
        public bool EditVaccany(VacancyVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Vaccancies.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Description = vmodel.Description;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
        public bool DeleteVaccany(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Vaccancies.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
    }
}