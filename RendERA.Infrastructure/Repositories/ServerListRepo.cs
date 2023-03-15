using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    class ServerListRepo : GenericRepo<DB.Models.ServerList>, IRepositories.IServerListRepo
    {
        public ServerListRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
