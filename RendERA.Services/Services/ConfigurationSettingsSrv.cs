using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class ConfigurationSettingsSrv : IConfigurationSettingsSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public ConfigurationSettingsSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void Delete(int id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.IConfigurationSettingsRepo.Remove(id);
                _unitOfWork.IConfigurationSettingsRepo.Save();
            }
        }

        public List<ConfigrationSettingsVM> GetAll()
        {
            throw new NotImplementedException();
        }

        //public List<ConfigrationSettingsVM> GetAll()
        //{
        //    var query = from t in _unitOfWork.IProjectRepo.Table.OrderByDescending(a => a.Name)
        //                select new ProjectVM
        //                {
        //                    Id = t.Id,
        //                    Name = t.Name,
        //                    Status = t.Status,
        //                    PartnerId = t.PartnerId,
        //                    CreatedDate = t.CreatedDate,
        //                    ModifiedDate = t.ModifiedDate,
        //                    PartnerName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == t.PartnerId).FirstOrDefault().UserName

        //                };
        //    if (query != null)
        //    {
        //        return query.ToList();
        //    }
        //    return null;
        //}

        public int GetNumberOfDaysDurationByUserType(int id){
            var obj = GetRemoveFileDuration();
            if (id == (int)RendERA.Infrastructure.Enum.UserCategory.Beginner)
            {
                return obj.Beginner;
            }
            else if (id == (int)RendERA.Infrastructure.Enum.UserCategory.Expert) { 
                return obj.Expert;
            }
            else {
                return obj.Pro;
            }
        }

        public ConfigrationSettingsVM GetById(int id)
        {
            var model = _unitOfWork.IConfigurationSettingsRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                var vm = new ConfigrationSettingsVM()
                {
                    Id = model.Id,
                    Beginner = model.Beginner,
                    Pro = model.Pro,
                    Expert = model.Expert
                };
                return vm;
            }
            return null;
        }

        public void Insert(ConfigrationSettingsVM model)
        {
            if (model != null)
            {
                var m = new ConfigurationSettings()
                {
                     Beginner = model.Beginner,
                     Pro = model.Pro,
                     Expert = model.Expert,
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.IConfigurationSettingsRepo.Add(m);
                _unitOfWork.IConfigurationSettingsRepo.Save();
            }
        }

        public void Update(ConfigrationSettingsVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.IConfigurationSettingsRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.Beginner = model.Beginner;
                m.Pro = model.Pro;
                m.Expert = model.Expert;
                m.ModifiedDate = DateTime.Now;
                _unitOfWork.IConfigurationSettingsRepo.Update(m);
                _unitOfWork.IConfigurationSettingsRepo.Save();
            }
        }

        public ConfigrationSettingsVM GetRemoveFileDuration() { 
            var obj =  GetById(1);
            if (obj == null) {
                obj = new ConfigrationSettingsVM()
                {
                    Beginner = 0,
                    Expert = 0,
                    Pro = 0
                };
                Insert(obj);
            }
            return obj;
        }

    }
}
