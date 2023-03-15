using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class UsersRepo : GenericRepo<DB.Models.Users>, IRepositories.IUsersRepo
    {
        public UsersRepo(RendERA.DB.Models.RendERAContext  context)
        {
            this.context = context;
        }

    }
}
