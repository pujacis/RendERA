using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IDocumentSrv
    {
        int InsertDoc(DB.ViewModels.DocumentVM docModel);
        void UpdateDoc(DB.ViewModels.DocumentVM docModel);
        void DeleteDoc(int id);
        DB.ViewModels.DocumentVM  GetDocById(int id);
        IQueryable<DB.ViewModels.DocumentVM> GetAllDocumnetsById(Guid userid);
        List<DB.ViewModels.DocumentVM> GetAllDocsByUserId(Guid userid);
        List<DB.ViewModels.DocumentVM> GetAllDocsByFilter(string filter, Guid userid);


        List<DB.ViewModels.DocumentVM> GetAllDocs();
    }
}
