using RendERA.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class DocumentRepo : GenericRepo<DB.Models.Document>, IRepositories.IDocumentRepo
    {
        public DocumentRepo(RendERA.DB.Models.RendERAContext  context)
        {
            this.context = context;
        }

    }
}
