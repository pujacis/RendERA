using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface ITeamSrv
    {
        void Insert(DB.ViewModels.TeamVM model);
        void Update(DB.ViewModels.TeamVM model);
        void Delete(int id);
        List<DB.ViewModels.TeamVM> GetAll();
        List<DB.ViewModels.TeamVM> GetTeamListByPartnerId(Guid id);
        DB.ViewModels.TeamVM GetById(int id);
    }
}
