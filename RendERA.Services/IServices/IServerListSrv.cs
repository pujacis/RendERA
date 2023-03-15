using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IServerListSrv
    {
        void Insert(DB.ViewModels.ServerListVM model);
        void Update(DB.ViewModels.ServerListVM model);
        void Delete(int id);
        List<DB.ViewModels.ServerListVM> GetAll();
        DB.ViewModels.ServerListVM GetById(int id);
    }
}
