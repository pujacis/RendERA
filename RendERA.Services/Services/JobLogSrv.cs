using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.ServiceManager.Services
{
    public class JobLogSrv : IJobLogSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public JobLogSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void Delete(int id)
        {
            if (GetById(id) != null)
            {
                _unitOfWork.IJobLogRepo.Remove(id);
                _unitOfWork.IJobLogRepo.Save();
            }
        }

        public JobLogVM GetById(int id)
        {
            var model = _unitOfWork.IJobLogRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (model != null)
            {
                var vm = new JobLogVM()
                {
                    Id = model.Id,
                    DocumentId =model.DocumentId,
                    LogDetail =model.LogDetail,
                    CreatedDate = model.CreatedDate,
                    Status =model.Status,
                    IsRead = model.IsRead
                };
                return vm;
            }
            return null;
        }
        public List<JobLogVM> GetAll()
        {
            var query = from t in _unitOfWork.IJobLogRepo.Table.OrderBy(a => a.CreatedDate)
                        select new JobLogVM
                        {
                            Id = t.Id,
                            DocumentId = t.DocumentId,
                            LogDetail = t.LogDetail,
                            CreatedDate = t.CreatedDate,
                            Status = t.Status,
                            IsRead =t.IsRead
                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }

        public List<JobLogVM> GetTrackingListByDocumentId(int docId)
        {
            var query = from t in _unitOfWork.IJobLogRepo.Table.Where(a => a.DocumentId == docId).OrderBy(a => a.CreatedDate)
                        select new JobLogVM
                        {
                            Id = t.Id,
                            DocumentId = t.DocumentId,
                            LogDetail = t.LogDetail,
                            CreatedDate = t.CreatedDate,
                            Status = t.Status,
                            IsRead =t.IsRead
                        };
            if (query != null)
            {
                return query.ToList();
            }
            return null;
        }

        public List<DB.ViewModels.JobLogVM> GetLast10TrackingLogByUserId(Guid userId) {

            var list = (from t in _unitOfWork.IJobLogRepo.Table.Where(a => a.Document.UploadedBy == userId && a.IsRead == false).OrderBy(a => a.Document.DatePosted).Take(10)
                select new JobLogVM
                {
                    Id = t.Id,
                    DocumentId = t.DocumentId,
                    LogDetail = t.LogDetail,
                    CreatedDate = t.CreatedDate,
                    Status = t.Status,
                    IsRead = t.IsRead,
                    TrackingId = t.Document.TrackingId
                }).ToList(); 


            if (list.Count < 10) {
                var left = 10 - list.Count;
                var list1 = (from t in _unitOfWork.IJobLogRepo.Table.Where(a => a.Document.UploadedBy == userId && a.IsRead == true).OrderByDescending(a => a.Document.DatePosted).Take(left)
                select new JobLogVM
                {
                    Id = t.Id,
                    DocumentId = t.DocumentId,
                    LogDetail = t.LogDetail,
                    CreatedDate = t.CreatedDate,
                    Status = t.Status,
                    IsRead = t.IsRead,
                    TrackingId = t.Document.TrackingId
                }).ToList();
                var result = list.Union(list1).OrderByDescending(x => x.CreatedDate).ToList();
                return result;
            }
            return list;
        }

        public void Insert(JobLogVM model)
        {
            if (model != null)
            {
                var m = new JobLog()
                {
                    DocumentId = model.DocumentId,
                    LogDetail = model.LogDetail,
                    CreatedDate = DateTime.Now,
                    IsRead =false,
                    Status = model.Status
                };
                _unitOfWork.IJobLogRepo.Add(m);
                _unitOfWork.IJobLogRepo.Save();
            }
        }
        public void Update(JobLogVM model)
        {
            if (model != null)
            {
                var m = _unitOfWork.IJobLogRepo.Table.Where(a => a.Id == model.Id).FirstOrDefault();
                m.DocumentId = model.DocumentId;
                m.LogDetail = model.LogDetail;
                m.IsRead = model.IsRead;
                m.Status = model.Status;
                _unitOfWork.IJobLogRepo.Update(m);
                _unitOfWork.IJobLogRepo.Save();
            }
        }
    }
}
