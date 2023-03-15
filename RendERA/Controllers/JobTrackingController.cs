using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RendERA.DB.ViewModels;
using RendERA.ServiceManager.IServices;

namespace RendERA.Controllers
{
    public class JobTrackingController : Controller
    {
        private ServiceManager.IServices.IDocumentSrv _documentService;
        private readonly IMessagesSrv _messagesService;
        private readonly IMessageReplySrv _messageReplysService;
        private readonly IConfigurationSettingsSrv _ConfigurationSettingsSrvice;
        private readonly IParameterSrv _ParameterService;
        private readonly IJobLogSrv _JobLogService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public JobTrackingController(IConfigurationSettingsSrv ConfigurationSettingsSrvice, ServiceManager.IServices.IDocumentSrv documentService, RendERA.ServiceManager.IServices.IAccountSrv accountService, IHttpContextAccessor contextAccessor, IMessagesSrv messagesService, IMessageReplySrv messageReplysService, IParameterSrv ParameterService, IJobLogSrv JobLogService)
        {
            _JobLogService = JobLogService;
            _ParameterService = ParameterService;
            _messagesService = messagesService;
            _messageReplysService = messageReplysService;
            _accountService = accountService;
            _contextAccessor = contextAccessor;
            _documentService = documentService;
            _ConfigurationSettingsSrvice = ConfigurationSettingsSrvice;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult GetTrackingDetails(string trackingId, int docId)
        {
            var jobTrackingList = new List<JobLogVM>();
            jobTrackingList = _JobLogService.GetTrackingListByDocumentId(docId);
            return PartialView("_GetTrackingDetails", jobTrackingList);
        }

        public IActionResult GetTrackingNotification()
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                      RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            var jobTrackingList = new List<JobLogVM>();
            jobTrackingList = _JobLogService.GetLast10TrackingLogByUserId(User.UserId);
            ChangeToReadNotification(User.UserId);
            return PartialView("_GetTrackingNotification", jobTrackingList);
        }


        private void ChangeToReadNotification(Guid userId) {
            var user = _accountService.GetUser(userId);
            //only for user type
            if (user.UserType == (int)RendERA.Infrastructure.Enum.UserType.User) {
                var jobTrackingList = _JobLogService.GetLast10TrackingLogByUserId(userId);
                foreach (var jobTrac in jobTrackingList)
                {
                    if (jobTrac.IsRead == false)
                    {
                        jobTrac.IsRead = true;
                        _JobLogService.Update(jobTrac);
                    }
                }
            }
        }
    }
}
