using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class TeamRepo : GenericRepo<DB.Models.Team>, IRepositories.ITeamRepo
    {
        public TeamRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
