
using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public partial interface IMessageReplySrv
    {
        
        void DeleteMsg(int id);
        DB.ViewModels.MessageReplyModel GetMsgById(int Id);
        void InsertMsg(DB.ViewModels.MessageReplyModel msgModel);
        IEnumerable<DB.ViewModels.MessageReplyModel> GetAllMsgs();

        IEnumerable<DB.ViewModels.MessageReplyModel> GetMsgsByMsgId(int msgId);
    }
}
