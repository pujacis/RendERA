using System;
using System.Collections.Generic;
using System.Text;
using RendERA.Infrastructure.IRepositories;

namespace RendERA.Infrastructure.Repositories 
{
    public class UnitOfWork :IRepositories.IUnitOfWork
    { 

        public DB.Models.RendERAContext  dbcontext;
        //private IRepositories.ITestAtheleteRepo _AtheleteRepository;
        private IRepositories.IUsersRepo _IUsersRepo;
        private IRepositories.IErrorLogsRepo _IErrorLogsRepo;
        private IRepositories.IMessagesRepo _IMessagesRepo;
        private IRepositories.IMessagesReplyRepo _IMessagesReplyRepo;
        private IRepositories.IDocumentRepo _IDocumentRepo;
        private IRepositories.IParameterRepo _IParameterRepo;
        private IRepositories.IParameterCostvaluesRepo _IParameterCostvaluesRepo;
        private IRepositories.ITeamRepo _ITeamRepo;
        private IRepositories.IJobLogRepo _IJobLogRepo;
        private IRepositories.IMasterCategoryRepo _IMasterCategoryRepo;
        private IRepositories.IServerListRepo _IServerListRepo;
        private IRepositories.IConfigurationSettingsRepo _IConfigurationSettingsRepo;
        private IRepositories.IProjectRepo _IProjectRepo;
        private IRepositories.IPaymentTransactionRepo _IPaymentTransactionRepo;
        private IRepositories.IParameterDocumnetMappingRepo _IParameterDocumnetMappingRepo;
        private IRepositories.IParameterSoftwareMappingRepo _IParameterSoftwareMappingRepo;
        private IRepositories.IParameterUserCategoryMappingRepo _IParameterUserCategoryMappingRepo;
        private IRepositories.IDefaultRenderSoftwaresRepo _IDefaultRenderSoftwaresRepo;

        public UnitOfWork(DB.Models.RendERAContext context)
        {
            this.dbcontext  = context;
        }


        public IRepositories.IUsersRepo IUsersRepo
        {
            get
            {
                return _IUsersRepo = _IUsersRepo ?? new Repositories.UsersRepo(dbcontext);
            }
        }
        public IRepositories.IErrorLogsRepo IErrorLogsRepo
        {
            get {
                return _IErrorLogsRepo = _IErrorLogsRepo ?? new Repositories.ErrorLogsRepo(dbcontext);
            }
        }
        public IRepositories.IDefaultRenderSoftwaresRepo IDefaultRenderSoftwaresRepo
        {
            get
            {
                return _IDefaultRenderSoftwaresRepo = _IDefaultRenderSoftwaresRepo ?? new Repositories.DefaultRenderSoftwaresRepo(dbcontext);
            }
        }

        public IMessagesRepo IMessagesRepo
        {
            get
            {
                return _IMessagesRepo = _IMessagesRepo ?? new Repositories.MessagesRepo(dbcontext);
            }
        }

        public IConfigurationSettingsRepo IConfigurationSettingsRepo
        {
            get
            {
                return _IConfigurationSettingsRepo = _IConfigurationSettingsRepo ?? new Repositories.ConfigurationSettingsRepo(dbcontext);
            }
        }

        public IMessagesReplyRepo IMessagesReplyRepo
        {
            get
            {
                return _IMessagesReplyRepo = _IMessagesReplyRepo ?? new Repositories.MessagesReplyRepo(dbcontext);
            }
        }

        public IParameterRepo IParameterRepo
        {
            get
            {
                return _IParameterRepo = _IParameterRepo ?? new Repositories.ParameterRepo(dbcontext);
            }
        } 
        public IPaymentTransactionRepo IPaymentTransactionRepo
        {
            get
            {
                return _IPaymentTransactionRepo = _IPaymentTransactionRepo ?? new Repositories.PaymentTransactionRepo(dbcontext);
            }
        } 
        
        public IParameterCostvaluesRepo IParameterCostvaluesRepo
        {
            get
            {
                return _IParameterCostvaluesRepo = _IParameterCostvaluesRepo ?? new Repositories.ParameterCostvaluesRepo(dbcontext);
            }
        }
        public ITeamRepo ITeamRepo
        {
            get
            {
                return _ITeamRepo = _ITeamRepo ?? new Repositories.TeamRepo(dbcontext);
            }
        }
        public IJobLogRepo IJobLogRepo
        {
            get
            {
                return _IJobLogRepo = _IJobLogRepo ?? new Repositories.JobLogRepo(dbcontext);
            }
        }
        public IServerListRepo IServerListRepo
        {
            get
            {
                return _IServerListRepo = _IServerListRepo ?? new Repositories.ServerListRepo(dbcontext);
            }
        }
        public IProjectRepo IProjectRepo
        {
            get
            {
                return _IProjectRepo = _IProjectRepo ?? new Repositories.ProjectRepo(dbcontext);
            }
        }
        public IMasterCategoryRepo IMasterCategoryRepo
        {
            get
            {
                return _IMasterCategoryRepo = _IMasterCategoryRepo ?? new Repositories.MasterCategoryRepo(dbcontext);
            }
        }
        public IParameterDocumnetMappingRepo IParameterDocumnetMappingRepo
        {
            get
            {
                return _IParameterDocumnetMappingRepo = _IParameterDocumnetMappingRepo ?? new Repositories.ParameterDocumnetMappingRepo(dbcontext);
            }
        }
        public IParameterSoftwareMappingRepo IParameterSoftwareMappingRepo
        {
            get
            {
                return _IParameterSoftwareMappingRepo = _IParameterSoftwareMappingRepo ?? new Repositories.ParameterSoftwareMappingRepo(dbcontext);
            }
        }

        public IDocumentRepo IDocumentRepo
        {
            get
            {
                return _IDocumentRepo = _IDocumentRepo ?? new Repositories.DocumentRepo(dbcontext);
            }
        }

        public IParameterUserCategoryMappingRepo IParameterUserCategoryMappingRepo
        {
            get
            {
                return _IParameterUserCategoryMappingRepo = _IParameterUserCategoryMappingRepo ?? new Repositories.ParameterUserCategoryMappingRepo(dbcontext);
            }
        }
        
        public void SaveChanges()
        {
            dbcontext.SaveChanges();
        }
        public void Dispose()
        {
            dbcontext.Dispose();
        }
        public void Rollback()
        {
            throw new NotImplementedException();
        }


    }

}