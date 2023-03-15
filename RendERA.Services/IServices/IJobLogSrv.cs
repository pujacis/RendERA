using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public interface IJobLogSrv
    {
        void Insert(DB.ViewModels.JobLogVM model);
        void Update(DB.ViewModels.JobLogVM model);
        void Delete(int id);
        List<DB.ViewModels.JobLogVM> GetAll();
        List<DB.ViewModels.JobLogVM> GetTrackingListByDocumentId(int docId);
        List<DB.ViewModels.JobLogVM> GetLast10TrackingLogByUserId(Guid userId);
    }
}
