using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class ParameterSoftwareMappingRepo : GenericRepo<DB.Models.ParameterSoftwareRenderMapping>, IRepositories.IParameterSoftwareMappingRepo
    { 
        public ParameterSoftwareMappingRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
