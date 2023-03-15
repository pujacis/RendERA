using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IConfigurationSettingsSrv
    {
        void Insert(DB.ViewModels.ConfigrationSettingsVM model);
        void Update(DB.ViewModels.ConfigrationSettingsVM model);
        void Delete(int id);
        List<DB.ViewModels.ConfigrationSettingsVM> GetAll();
        DB.ViewModels.ConfigrationSettingsVM GetById(int id);
        DB.ViewModels.ConfigrationSettingsVM GetRemoveFileDuration();
        int GetNumberOfDaysDurationByUserType(int id);
    }
}
