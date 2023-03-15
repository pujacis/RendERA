using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IProjectSrv
    {
        void Insert(DB.ViewModels.ProjectVM model);
        void Update(DB.ViewModels.ProjectVM model);
        void Delete(int id);
        List<DB.ViewModels.ProjectVM> GetAll();
        List<DB.ViewModels.ProjectVM> GetProjectListByPartnerId(Guid id);
        DB.ViewModels.ProjectVM GetById(int id);
    }
}
