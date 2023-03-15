using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class ParameterCostvaluesRepo : GenericRepo<DB.Models.ParameterCostvalues>, IRepositories.IParameterCostvaluesRepo
    { 
        public ParameterCostvaluesRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
