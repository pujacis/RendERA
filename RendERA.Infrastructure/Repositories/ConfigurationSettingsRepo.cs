using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class ConfigurationSettingsRepo : GenericRepo<DB.Models.ConfigurationSettings>, IRepositories.IConfigurationSettingsRepo
    {
        public ConfigurationSettingsRepo(RendERA.DB.Models.RendERAContext context)
        {
            this.context = context;
        }

    }
}
