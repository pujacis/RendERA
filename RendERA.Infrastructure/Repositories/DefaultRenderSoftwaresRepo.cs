using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class DefaultRenderSoftwaresRepo: GenericRepo<DB.Models.DefaultRenderSoftwares>, IRepositories.IDefaultRenderSoftwaresRepo
    {
        public DefaultRenderSoftwaresRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
