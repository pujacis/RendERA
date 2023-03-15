using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IMasterCategorySrv
    {
        void Insert(DB.ViewModels.MasterCategoryVM model);
        void Update(DB.ViewModels.MasterCategoryVM model);
        void Delete(int id);
        List<DB.ViewModels.MasterCategoryVM> GetAll();
        DB.ViewModels.MasterCategoryVM GetById(int id);
    }
}
