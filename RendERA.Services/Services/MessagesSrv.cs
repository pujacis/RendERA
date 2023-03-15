using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagedList.Core;
using RendERA.DB.ViewModels;

namespace RendERA.ServiceManager.Services
{
    public class MessagesSrv : IServices.IMessagesSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public MessagesSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public int Count()
        {
            return _unitOfWork.IMessagesRepo.Table.Count();
        }

        public void DeleteMsg(int id)
        {
            if (GetMsgById(id) != null) {
                _unitOfWork.IMessagesRepo.Remove(id);
                _unitOfWork.IMessagesRepo.Save();
            }

        }

        public MessagesList GetAllMsgsbyPage(string query, Guid UserId, int pageNumber, int pageSize)
        {
            MessagesList ml = new MessagesList();
            if (query == null) { query = "   "; }
            var result = from data in _unitOfWork.IMessagesRepo.GetAll().Where(x => x.Subject.Contains(query, System.StringComparison.CurrentCultureIgnoreCase) || x.To == UserId || x.From == UserId || x.JobUploadedBy == UserId).OrderByDescending(x => x.DatePosted)
                         select new MessageModel
                         {
                             Id = data.Id,
                             DatePosted = data.DatePosted,
                             From = data.From,
                             MessageToPost = data.MessageToPost,
                             Subject = data.Subject,
                             To = data.To,
                             JobUploadedBy = data.JobUploadedBy,
                             FromName = _unitOfWork.IUsersRepo.Table.Where(x => x.UserId == data.From).FirstOrDefault().UserName,
                             ToName = _unitOfWork.IUsersRepo.Table.Where(x => x.UserId == data.To).FirstOrDefault().UserName,
                             JobUploadedByName = data.JobUploadedBy != null ? _unitOfWork.IUsersRepo.Table.Where(x => x.UserId == data.JobUploadedBy).FirstOrDefault().UserName:""
                         };
            var r = result.FirstOrDefault();
            if (r != null)
            {
                ml.Messages = new StaticPagedList<MessageModel>(result.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, result.Count());
                ml.TotalCount = result.Count();
            }
            return ml;
        }

        public MessageModel GetMsgById(int Id)
        {
            var data = _unitOfWork.IMessagesRepo.Get(Id);
            if (data == null) { return null; }
            else
            {
                MessageModel model = new MessageModel()
                {
                    Id = data.Id,
                    DatePosted = data.DatePosted,
                    From = data.From,
                    MessageToPost = data.MessageToPost,
                    Subject = data.Subject,
                    To = data.To,
                    JobUploadedBy = data.JobUploadedBy
                };
                return model;
            }
        }

        public int GenrateMsgByDoc(DB.ViewModels.MessageModel msgModel) {
            DB.Models.Message model = new DB.Models.Message()
            {
                Id = msgModel.Id,
                DatePosted = msgModel.DatePosted,
                From = msgModel.From,
                MessageToPost = msgModel.MessageToPost,
                Subject = msgModel.Subject,
                To = msgModel.To,
                JobUploadedBy = msgModel.JobUploadedBy
            };
            _unitOfWork.IMessagesRepo.Add(model);
            _unitOfWork.IMessagesRepo.Save();
            return model.Id;
        }

        public void InsertMsg(MessageModel msgModel)
        {
            DB.Models.Message model = new DB.Models.Message()
            {
                Id = msgModel.Id,
                DatePosted = msgModel.DatePosted,
                From = msgModel.From,
                MessageToPost = msgModel.MessageToPost,
                Subject = msgModel.Subject,
                To = msgModel.To,
                JobUploadedBy = msgModel.JobUploadedBy
            };
            _unitOfWork.IMessagesRepo.Add(model);
            _unitOfWork.IMessagesRepo.Save();
        }
    }
}
