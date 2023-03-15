using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
   public class ErrorLogsRepo: GenericRepo<DB.Models.ErrorLogs>,IRepositories.IErrorLogsRepo
    {
        public ErrorLogsRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
