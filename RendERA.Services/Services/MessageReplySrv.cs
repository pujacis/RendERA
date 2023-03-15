using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RendERA.DB.ViewModels;

namespace RendERA.ServiceManager.Services
{
    public class MessageReplySrv : IServices.IMessageReplySrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public MessageReplySrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public void DeleteMsg(int id)
        {
            if (GetMsgById(id) != null)
            {
                _unitOfWork.IMessagesReplyRepo.Remove(id);
                _unitOfWork.IMessagesReplyRepo.Save();
            }
        }

       


    public IEnumerable<MessageReplyModel> GetAllMsgs()
        {
            var query = from u in _unitOfWork.IMessagesReplyRepo.Table.OrderByDescending(a => a.ReplyDateTime)
                        select new MessageReplyModel
                        {
                            Id = u.Id,
                            MessageId = u.MessageId,
                            ReplyDateTime = u.ReplyDateTime,
                            FromId = u.FromId,
                            ReplyFrom = _unitOfWork.IUsersRepo.Table.Where(x => x.UserId == u.FromId).FirstOrDefault().UserName,
                            ReplyMessage = u.ReplyMessage
                        };

            if (query == null)
            {
                return null;
            }
            return query.ToList();

        }

        public MessageReplyModel GetMsgById(int Id)
        {

            var msgModel = _unitOfWork.IMessagesReplyRepo.Get(Id);
            if (msgModel == null) { return null; }
            else
            {
                MessageReplyModel model = new MessageReplyModel()
                {
                    Id = msgModel.Id,
                    MessageId = msgModel.MessageId,
                    ReplyDateTime = msgModel.ReplyDateTime,
                    FromId = msgModel.FromId,
                    ReplyFrom = _unitOfWork.IUsersRepo.Table.Where(x => x.UserId == msgModel.FromId).FirstOrDefault().UserName,
                    ReplyMessage = msgModel.ReplyMessage
                };
                return model;
            }
        }

        public IEnumerable<MessageReplyModel> GetMsgsByMsgId(int msgId)
        {

            var query = from u in _unitOfWork.IMessagesReplyRepo.Table.Where(x => x.MessageId == msgId).OrderByDescending(a => a.ReplyDateTime)
                        select new MessageReplyModel
                        {
                            Id = u.Id,
                            MessageId = u.MessageId,
                            ReplyDateTime = u.ReplyDateTime,
                            FromId = u.FromId,
                            ReplyFrom = _unitOfWork.IUsersRepo.Table.Where(x => x.UserId == u.FromId).FirstOrDefault().UserName,
                            ReplyMessage = u.ReplyMessage
                        };

            if (query == null)
            {
                return null;
            }
            return query.ToList();

        }

        public void InsertMsg(MessageReplyModel msgModel)
        {

            DB.Models.MessageReply model = new DB.Models.MessageReply()
            {
                Id = msgModel.Id,
                MessageId = msgModel.MessageId,
                ReplyDateTime = msgModel.ReplyDateTime,
                FromId = msgModel.FromId,
                ReplyMessage = msgModel.ReplyMessage
            };
            _unitOfWork.IMessagesReplyRepo.Add(model);
            _unitOfWork.IMessagesReplyRepo.Save();
        }

    }
}
