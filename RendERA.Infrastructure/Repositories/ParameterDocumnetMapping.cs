using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class ParameterDocumnetMappingRepo : GenericRepo<DB.Models.ParameterDocumnetMapping>, IRepositories.IParameterDocumnetMappingRepo
    { 
        public ParameterDocumnetMappingRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
