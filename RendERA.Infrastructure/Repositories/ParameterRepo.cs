using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class ParameterRepo : GenericRepo<DB.Models.Parameter>, IRepositories.IParameterRepo { 
        public ParameterRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
