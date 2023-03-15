using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class MessagesReplyRepo : GenericRepo<DB.Models.MessageReply>, IRepositories.IMessagesReplyRepo
    {
        public MessagesReplyRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
