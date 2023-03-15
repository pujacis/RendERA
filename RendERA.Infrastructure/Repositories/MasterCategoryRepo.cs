using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class MasterCategoryRepo : GenericRepo<DB.Models.MasterCategory>, IRepositories.IMasterCategoryRepo
    {
        public MasterCategoryRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }
    }
}
