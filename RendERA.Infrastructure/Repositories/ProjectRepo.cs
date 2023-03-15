using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    class ProjectRepo : GenericRepo<DB.Models.Project>, IRepositories.IProjectRepo
    {
        public ProjectRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
