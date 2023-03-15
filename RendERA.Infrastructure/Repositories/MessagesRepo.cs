using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class MessagesRepo : GenericRepo<DB.Models.Message>, IRepositories.IMessagesRepo
    {
        public MessagesRepo(RendERA.DB.Models.RendERAContext  context)
        {
            this.context = context;
        }

    }
}
