
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.ServiceManager.IServices
{
    public partial interface  IMessagesSrv
    {

        void DeleteMsg(int id);
        int Count();
        DB.ViewModels.MessageModel GetMsgById(int Id);
        //DB.ViewModels.MessageModel GetMsgByEmail(Guid userid);
        void InsertMsg(DB.ViewModels.MessageModel msgModel);
        int GenrateMsgByDoc(DB.ViewModels.MessageModel msgModel);
        
        DB.ViewModels.MessagesList GetAllMsgsbyPage(string query,Guid UserId,int pageNumber,int pageSize);
    }
}
