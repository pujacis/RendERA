using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.Infrastructure.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
       
       // IRepositories.ITestAtheleteRepo  TestAthelete {get;}
        IRepositories.IUsersRepo IUsersRepo { get; }
        IRepositories.IErrorLogsRepo IErrorLogsRepo { get; }
        IRepositories.IMessagesRepo IMessagesRepo { get; }
        IRepositories.IMessagesReplyRepo IMessagesReplyRepo { get; }
        IRepositories.IDocumentRepo IDocumentRepo { get; }
        IRepositories.IParameterRepo IParameterRepo { get; }
        IParameterCostvaluesRepo IParameterCostvaluesRepo { get; }
        IConfigurationSettingsRepo IConfigurationSettingsRepo { get; }

        IRepositories.ITeamRepo ITeamRepo { get; }
        IRepositories.IJobLogRepo IJobLogRepo { get; }
        IRepositories.IMasterCategoryRepo IMasterCategoryRepo { get; }
        IRepositories.IServerListRepo IServerListRepo { get; }
        IRepositories.IProjectRepo IProjectRepo { get; }
        IRepositories.IPaymentTransactionRepo IPaymentTransactionRepo { get; }
        IParameterDocumnetMappingRepo IParameterDocumnetMappingRepo { get; }
        IParameterSoftwareMappingRepo IParameterSoftwareMappingRepo { get; }
        IParameterUserCategoryMappingRepo IParameterUserCategoryMappingRepo { get; }
        IRepositories.IDefaultRenderSoftwaresRepo IDefaultRenderSoftwaresRepo { get; }

        void SaveChanges();
        void Rollback();
    }
}