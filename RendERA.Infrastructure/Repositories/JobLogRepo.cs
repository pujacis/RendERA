using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class JobLogRepo : GenericRepo<DB.Models.JobLog>, IRepositories.IJobLogRepo
    {
        public JobLogRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
