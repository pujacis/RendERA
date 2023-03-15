using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class ParameterUserCategoryMappingRepo : GenericRepo<DB.Models.ParameterUserCategoryMapping>, IRepositories.IParameterUserCategoryMappingRepo
    { 
        public ParameterUserCategoryMappingRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
