using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RendERA.ServiceManager.Services
{
    public class DocumentSrv : IServices.IDocumentSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public DocumentSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void DeleteDoc(int id)
        {
            if (GetDocById(id) != null)
            {
                _unitOfWork.IDocumentRepo.Remove(id);
                _unitOfWork.IDocumentRepo.Save();
            }
        }

        public List<DocumentVM> GetAllDocs()
        {
            var query = from d in _unitOfWork.IDocumentRepo.Table.Where(a => a.IsSubmitted == true).OrderByDescending(a => a.DatePosted)
                        select new DocumentVM {
                            Id = d.Id,
                            DatePosted = d.DatePosted,
                            AssignedPartnerId = d.AssignedPartnerId,
                            AssignedTeamId = d.AssignedTeamId,
                            AssignedProjectId = d.AssignedProjectId,
                            FileDownloadUrl = d.FileDownloadUrl,
                            FileName = d.FileName,
                            FileUploadUrl = d.FileUploadUrl,
                            IsParameter = d.IsParameter,
                            IsSubmitted = d.IsSubmitted,
                            ModifiedDate = d.ModifiedDate,
                            UploadedBy = d.UploadedBy,
                            UploadedByName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == d.UploadedBy).FirstOrDefault().UserName,
                            MessageId = d.MessageId,
                            TransactionId = d.TransactionId,
                            TrackingId = d.TrackingId,
                            status = _unitOfWork.IJobLogRepo.Table.Where(a => a.DocumentId == d.Id).OrderByDescending(a => a.CreatedDate).FirstOrDefault().Status

                        };
            return query.ToList();
        }

        public List<DocumentVM> GetAllDocsByUserId(Guid userid)
        {
            var query = from d in _unitOfWork.IDocumentRepo.Table.Where(a=> a.UploadedBy == userid || a.AssignedPartnerId == userid ).OrderByDescending(a => a.DatePosted)
                        select new DocumentVM
                        {
                            Id = d.Id,
                            DatePosted = d.DatePosted,
                            AssignedPartnerId = d.AssignedPartnerId,
                            AssignedTeamId = d.AssignedTeamId,
                            AssignedProjectId = d.AssignedProjectId,
                            FileDownloadUrl = d.FileDownloadUrl,
                            FileName = d.FileName,
                            FileUploadUrl = d.FileUploadUrl,
                            IsParameter = d.IsParameter,
                            IsSubmitted = d.IsSubmitted,
                            ModifiedDate = d.ModifiedDate,
                            UploadedBy = d.UploadedBy,
                            UploadedByName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == d.UploadedBy).FirstOrDefault().UserName,
                            MessageId = d.MessageId,
                            TransactionId = d.TransactionId,
                            TrackingId = d.TrackingId,
                            status = _unitOfWork.IJobLogRepo.Table.Where(a => a.DocumentId == d.Id).OrderByDescending(a => a.CreatedDate).FirstOrDefault().Status
                        };
            return query.ToList();
        }

        public IQueryable<DB.ViewModels.DocumentVM> GetAllDocumnetsById(Guid userid)
        {
            var query = from d in _unitOfWork.IDocumentRepo.Table.Where(a => a.AssignedPartnerId == userid || a.UploadedBy == userid).OrderByDescending(a => a.DatePosted)
                        select new DocumentVM
                        {
                            Id= d.Id,
                            DatePosted = d.DatePosted,
                            AssignedPartnerId = d.AssignedPartnerId,
                            AssignedTeamId = d.AssignedTeamId,
                            AssignedProjectId = d.AssignedProjectId,
                            FileDownloadUrl = d.FileDownloadUrl,
                            FileName = d.FileName,
                            FileUploadUrl = d.FileUploadUrl,
                            IsParameter = d.IsParameter,
                            IsSubmitted = d.IsSubmitted,
                            ModifiedDate = d.ModifiedDate,
                            UploadedBy = d.UploadedBy,
                            UploadedByName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == d.UploadedBy).FirstOrDefault().UserName,
                            MessageId = d.MessageId,
                            TransactionId = d.TransactionId,
                            TrackingId = d.TrackingId,
                            status = _unitOfWork.IJobLogRepo.Table.Where(a => a.DocumentId == d.Id).OrderByDescending(a => a.CreatedDate).FirstOrDefault().Status
                        };
            return query;
        }

        public List<DB.ViewModels.DocumentVM> GetAllDocsByFilter(string filter, Guid userid)
        {
            

            var query = from d in _unitOfWork.IDocumentRepo.Table.Where(a => a.TrackingId.Contains(filter) && a.UploadedBy == userid)
                        select new DocumentVM
                        {
                            Id = d.Id,
                            DatePosted = d.DatePosted,
                            AssignedPartnerId = d.AssignedPartnerId,
                            AssignedTeamId = d.AssignedTeamId,
                            AssignedProjectId = d.AssignedProjectId,
                            FileDownloadUrl = d.FileDownloadUrl,
                            FileName = d.FileName,
                            FileUploadUrl = d.FileUploadUrl,
                            IsParameter = d.IsParameter,
                            IsSubmitted = d.IsSubmitted,
                            ModifiedDate = d.ModifiedDate,
                            UploadedBy = d.UploadedBy,
                            MessageId = d.MessageId,
                            TransactionId = d.TransactionId,
                            TrackingId = d.TrackingId,
                            status = _unitOfWork.IJobLogRepo.Table.Where(a => a.DocumentId == d.Id).OrderByDescending(a => a.CreatedDate).FirstOrDefault().Status
                        };
            return query.ToList();
        }
        public DocumentVM GetDocById(int id)
        {
            var d = _unitOfWork.IDocumentRepo.Table.Where(a => a.Id == id).FirstOrDefault();
            if (d != null)
            {
                var docvm = new DocumentVM() {
                    Id =d.Id,
                    DatePosted = d.DatePosted,
                    AssignedPartnerId = d.AssignedPartnerId,
                    AssignedTeamId = d.AssignedTeamId,
                    AssignedProjectId = d.AssignedProjectId,
                    FileDownloadUrl = d.FileDownloadUrl,
                    FileName = d.FileName,
                    FileUploadUrl = d.FileUploadUrl,
                    IsParameter = d.IsParameter,
                    IsSubmitted = d.IsSubmitted,
                    ModifiedDate = d.ModifiedDate,
                    UploadedBy = d.UploadedBy,
                    UploadedByName = _unitOfWork.IUsersRepo.Table.Where(a => a.UserId == d.UploadedBy).FirstOrDefault().UserName,
                    MessageId = d.MessageId,
                    TrackingId = d.TrackingId,
                    status = _unitOfWork.IJobLogRepo.Table.Where(a => a.DocumentId == d.Id).OrderByDescending(a => a.CreatedDate).FirstOrDefault().Status
                };
                return docvm;
            }
            return null;
        }

        //public void InsertDoc(DocumentVM docModel)
        //{
        //    if (docModel != null)
        //    {
        //        var doc = new Document()
        //        {
        //            Id = docModel.Id,
        //            DatePosted = docModel.DatePosted,
        //            AssignedPartnerId = docModel.AssignedPartnerId,
        //            AssignedProjectId = docModel.AssignedProjectId,
        //            FileDownloadUrl = docModel.FileDownloadUrl,
        //            FileName = docModel.FileName,
        //            FileUploadUrl = docModel.FileUploadUrl,
        //            IsParameter = docModel.IsParameter,
        //            IsSubmitted = docModel.IsSubmitted,
        //            ModifiedDate = docModel.ModifiedDate,
        //            UploadedBy = docModel.UploadedBy,
        //            MessageId = docModel.MessageId,
        //            TransactionId = docModel.TransactionId,
        //            TrackingId = docModel.TrackingId
        //        };
        //        _unitOfWork.IDocumentRepo.Add(doc);
        //        _unitOfWork.IDocumentRepo.Save();
        //    }
        //}
        public int InsertDoc(DocumentVM docModel)
        {
            if (docModel != null)
            {
                var doc = new Document()
                {
                    Id = docModel.Id,
                    DatePosted = docModel.DatePosted,
                    AssignedPartnerId = docModel.AssignedPartnerId,
                    AssignedProjectId = docModel.AssignedProjectId,
                    FileDownloadUrl = docModel.FileDownloadUrl,
                    FileName = docModel.FileName,
                    FileUploadUrl = docModel.FileUploadUrl,
                    IsParameter = docModel.IsParameter,
                    IsSubmitted = docModel.IsSubmitted,
                    ModifiedDate = docModel.ModifiedDate,
                    UploadedBy = docModel.UploadedBy,
                    MessageId = docModel.MessageId,
                    TransactionId = docModel.TransactionId,
                    TrackingId = docModel.TrackingId
                };
                _unitOfWork.IDocumentRepo.Add(doc);
                _unitOfWork.IDocumentRepo.Save();
                return doc.Id;
            }
            return 0;
        }
        public void UpdateDoc(DocumentVM docModel)
        {
            if (docModel != null)
            {
                var doc = _unitOfWork.IDocumentRepo.Get(docModel.Id);
                doc.DatePosted = docModel.DatePosted;
                doc.AssignedPartnerId = docModel.AssignedPartnerId;
                doc.AssignedProjectId = docModel.AssignedProjectId;
                doc.AssignedTeamId = docModel.AssignedTeamId;
                doc.FileDownloadUrl = docModel.FileDownloadUrl;
                doc.FileName = docModel.FileName;
                doc.FileUploadUrl = docModel.FileUploadUrl;
                doc.IsParameter = docModel.IsParameter;
                doc.ModifiedDate = DateTime.Now;
                doc.UploadedBy = docModel.UploadedBy;
                doc.MessageId = docModel.MessageId;
                doc.IsSubmitted = docModel.IsSubmitted;
                doc.TrackingId = docModel.TrackingId;
                _unitOfWork.IDocumentRepo.Update(doc);
                _unitOfWork.IDocumentRepo.Save();
            }
        }

    }
}
