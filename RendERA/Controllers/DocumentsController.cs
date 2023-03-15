using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.Models;
using RendERA.ServiceManager.IServices;
using RendERA.Common;

namespace RendERA.Controllers
{
    public class DocumentsController : Controller
    {

        private static Random _random = new Random();
        private readonly IConfigurationSettingsSrv _ConfigurationSettingsSrvice;
        private ServiceManager.IServices.IDocumentSrv _documentService;
        private readonly IMessagesSrv _messagesService;
        private readonly IMessageReplySrv _messageReplysService;
        private readonly IParameterSrv _parameterService;
        private readonly ITeamSrv _teamService;
        private readonly IJobLogSrv _jobLogService;
        private readonly IProjectSrv _projectService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session
        private int _alphanumericLength = 8;
        public DocumentsController(IConfigurationSettingsSrv ConfigurationSettingsSrvice,ServiceManager.IServices.IDocumentSrv documentService, RendERA.ServiceManager.IServices.IAccountSrv accountService, IHttpContextAccessor contextAccessor, IMessagesSrv messagesService, IMessageReplySrv messageReplysService, IParameterSrv parameterService, ITeamSrv teamService, IProjectSrv projectService, IJobLogSrv jobLogService)
        {
            _jobLogService = jobLogService;
            _ConfigurationSettingsSrvice = ConfigurationSettingsSrvice;
            _messagesService = messagesService;
            _messageReplysService = messageReplysService;
             _accountService = accountService;
            _parameterService = parameterService;
            _contextAccessor = contextAccessor;
            _documentService = documentService;
            _teamService = teamService;
            _projectService = projectService;
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 20009715200)] // 200 MB limit
        [RequestSizeLimit(20009715200)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                      RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var User = _accountService.GetUser(new Guid(value));
                if (file == null || file.Length == 0)
                {
                    TempData["Msg"] = "Faild to Upload";
                    return RedirectToAction("Assets", "Home");
                }
                //upload file
                var url = await new Common.Files().UploadFile(file);

                if (url == null)
                {
                    TempData["Msg"] = "Faild to Upload";
                    return RedirectToAction("Assets", "Home");
                }

                //Add Document details to Db
                var doc = new DB.ViewModels.DocumentVM()
                {
                    FileName = file.GetFilename(),
                    DatePosted = DateTime.Now,
                    UploadedBy = User.UserId,FileUploadUrl = url,
                    TrackingId = GetUniqueAlphaNumeric()
                };
                int docId = _documentService.InsertDoc(doc);

                //check need to remove any file
                CheckRemoveFile();

                //Add job log
                if (docId > 0) {
                    SetJobLog(new JobLogVM { DocumentId = docId,
                                             Status = (int)RendERA.Infrastructure.Enum.JobTrackingStatus.NewJob,
                                             LogDetail = "Job Created by "+ User.UserName
                    });
                }

                TempData["Msg"] = "Uploaded successfully";
                return RedirectToAction("Assets", "Home");
            }
            catch
            {
                TempData["Msg"] = "Faild to Upload";
                return RedirectToAction("Assets", "Home");
            }
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 20009715200)]    
        [RequestSizeLimit(809715200)]
        public async Task<IActionResult> UploadTargetFile(IFormFile file, int documentId)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                      RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var User = _accountService.GetUser(new Guid(value));
                if (file == null || file.Length == 0 || documentId <= 0)
                {
                    TempData["Msg"] = "Faild to Upload";
                    return RedirectToAction("DocumentList", "Documents");
                }

                //upload file
                CheckRemoveFile();
                var url = await new Common.Files().UploadFile(file);

                if (url == null)
                {
                    TempData["Msg"] = "Faild to Upload";
                    return RedirectToAction("DocumentList", "Documents");
                }

                //update Document details 
                var doc = _documentService.GetDocById(documentId);
                doc.FileDownloadUrl = url;
                _documentService.UpdateDoc(doc);
                try{

                //genrate email to admin
                string mailId = _accountService.GetAdminEmail();
                var domainurl = "http://" + _contextAccessor.HttpContext.Request.Host.Value + "/Documents/Download?filename=" + url;
                var subject = "Job-" + doc.Id + " done ";
                var msg = "Job-" + doc.Id + " done , Download <a href='" + domainurl + "' >here</a> <br><br>Regards,<br> RendERA Team";
                Helpers.Utility.SendEmail(mailId, subject, msg);

                //send mail to user
                var user = _accountService.GetUser(doc.UploadedBy);
                mailId = user.Email;
                Helpers.Utility.SendEmail(mailId, subject, msg);

                    //Add job log
                SetJobLog(new JobLogVM
                {
                    DocumentId = documentId,
                    Status = (int)RendERA.Infrastructure.Enum.JobTrackingStatus.Completed,
                    LogDetail = "job Completed"
                });

                }
            catch
            {
                TempData["Msg"] = "Failed to send email";
            }

            if (TempData["Msg"] != null)
                TempData["Msg"] = "Assigned successfully" + TempData["Msg"] as string;
            else
                TempData["Msg"] = "Uploaded successfully";
                return RedirectToAction("DocumentList", "Documents");
            }
            catch
            {
                TempData["Msg"] = "Faild to Upload";
                return RedirectToAction("DocumentList", "Documents");
            }
        }

        public async Task<IActionResult> AssetsDownload(string filename)
        {
            try { 
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (filename != null)
                {
                    var file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    {
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Assets", "Home");
            }            
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Assets", "Home");
            }
        }

        public async Task<IActionResult> AnalyticsDownload(string filename)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (filename != null)
                {
                    var file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    {
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Analytics", "Home");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Analytics", "Home");
            }
        }

        public async Task<IActionResult> RenderDownload(string filename)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (filename != null)
                {
                    var file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    {
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Render", "Home");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Render", "Home");
            }
        }
   
        public async Task<IActionResult> AdminDownload(string filename)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (filename != null)
                {
                    var file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    {
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            } 
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            }
        }

        public async Task<IActionResult> AdminDetailsDownload(string filename)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (filename != null)
                {
                    var file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    {
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Details", "Documents");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Details", "Documents");
            }
        }

        public async Task<IActionResult> DownloadGuide(string filename)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var User = _accountService.GetUser(new Guid(value));
                if (filename != null)
                {
                    FileStreamResult file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    { 
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentGuide", "Documents");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentGuide", "Documents");
            }
        }
        public async Task<IActionResult> UserDownloadGuide(string filename)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                         RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (filename != null)
                {
                    FileStreamResult file = await new Common.Files().DownloadFile(filename);
                    if (file != null)
                    { 
                        return file;
                    }
                }
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDocumentGuide", "Documents");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDocumentGuide", "Documents");
            }
        }

        public async Task<IActionResult> Download(string filename)
        {
            
            if (filename == null) {
                    return RedirectToAction("DocumentList", new { text = "Document not Uploaded" });
            }

            var file = await new Common.Files().DownloadFile(filename);

            if (file == null) {
                
                    return RedirectToAction("DocumentList", new { text = "Something went wrong" });
             
            }
            return file;
        }

        public IActionResult DocumentGuide()
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                            RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            ViewBag.usertype = User.UserType;
            return View();
        }
        
        public IActionResult ProceedPayment(int id)
        {
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            var doc = _documentService.GetDocById(id);
            doc.IsSubmitted = true;
            _documentService.UpdateDoc(doc);

            try
            {
                //genrate email
                string mailId = _accountService.GetAdminEmail();
                var subject = "New Job-" + doc.Id + "  Created";
                var msg = "New Job-" + doc.Id + " Created by " + doc.UploadedByName + ", Check <a href=' http://" + _contextAccessor.HttpContext.Request.Host.Value + "/Account/Login'> here </a> <br><br>Regards,<br> RendERA Team";
                Helpers.Utility.SendEmail(mailId, subject, msg);
            }
            catch {
                TempData["Msg"] = "Failed to send email";
            }

            if (TempData["Msg"] != null)
                TempData["Msg"] = "Assigned successfully" + TempData["Msg"] as string;
            else
            TempData["Msg"] = "Payment Done!";
            return RedirectToAction("Analytics", "Home");
        }
        public IActionResult UserDocumentGuide()
        {
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View();
        }


        // GET: Documents
        public IActionResult DocumentList()
        {
            try
            {
                List<RendERA.DB.ViewModels.DocumentVM> documentList = new List<RendERA.DB.ViewModels.DocumentVM>();
                var value = _contextAccessor.HttpContext.Session.GetString(
                       RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var User = _accountService.GetUser(new Guid(value));
                ViewBag.UserType = User.UserType;

                //admin access all documnet
                if (User.UserType == (int)RendERA.Infrastructure.Enum.UserType.Admin)
                {
                    documentList = _documentService.GetAllDocs();
                    foreach (var doc in documentList)
                    {
                        doc.Partners = GetUserListItemsByUserType((int)RendERA.Infrastructure.Enum.UserType.Partner);
                    }
                }
                //partner access only their related  document
                else {
                    documentList = _documentService.GetAllDocsByUserId(User.UserId);
                    foreach (var doc in documentList)
                    {
                        doc.Teams = GetTeamListItemsByPartner(User.UserId);
                        doc.Projects = GetProjectListItemsByPartner(User.UserId);
                    }
                }

                if (TempData["Msg"] != null)
                    ViewBag.Msg = TempData["Msg"] as string;
                ViewBag.usertype = User.UserType;
                return View(documentList);
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("AdminDashboard", "Home");
            }
           
        }

        // GET: Documents/Details/5
        public IActionResult Details(int id)
        {
            
            var value = _contextAccessor.HttpContext.Session.GetString(
                RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            try
            {
                var document = _documentService.GetDocById(id);
             //   document.Parameters = _parameterService.GetSelectedParameterByDocId(id);
                if (document == null)
                {
                    return NotFound();
                }
                ViewBag.usertype = User.UserType;
                return View(document);
            }
            catch
            {
                ViewBag.usertype = User.UserType;
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            }
        }


        public IActionResult AssignPartner(int documentId, Guid PartnerId)
        {
            try {
                if (PartnerId != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    var value = _contextAccessor.HttpContext.Session.GetString(
                            RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                    if (value == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    var User = _accountService.GetUser(new Guid(value));
                    var doc = _documentService.GetDocById(documentId);
                    if (doc.AssignedPartnerId == null || doc.AssignedPartnerId != PartnerId)
                    {
                        if (doc.AssignedPartnerId != PartnerId && doc.AssignedPartnerId != null)
                        {
                            //Genrate reply Message
                            MessageReplyModel _reply = new MessageReplyModel();
                            _reply.ReplyDateTime = DateTime.Now;
                            _reply.MessageId = (int)doc.MessageId;
                            _reply.ReplyMessage = "This 'Job-" + doc.Id + "' Assigned someone else.";
                            _reply.FromId = User.UserId;
                            _messageReplysService.InsertMsg(_reply);

                        }

                        var partner = _accountService.GetUser(PartnerId);
                        //Genrate  Message
                        MessageModel messagetoPost = new MessageModel()
                        {
                            DatePosted = DateTime.Now,
                            Subject = "Assigned Job-" + doc.Id,
                            MessageToPost = "Admin assign 'Job-" + doc.Id + "' to "+ partner.UserName,
                            To = PartnerId,
                            From = User.UserId,
                            JobUploadedBy = doc.UploadedBy
                        };
                        //genrate email
                        try
                        {
                            string mailId = partner.Email;
                            var subject = "New Job-" + doc.Id + "  Assigned to you";
                            var msg = "New Job-" + doc.Id + " Created by " + doc.UploadedByName + " assigned to you , Check  - <a  href='http://" + _contextAccessor.HttpContext.Request.Host.Value + "/Account/Login' >here</a> <br><br>Regards,<br> RendERA Team";
                            Helpers.Utility.SendEmail(mailId, subject, msg);
                        
                        }
                        catch{
                            TempData["Msg"] = ", Failed to send  email";
                        }

                        //Add job log
                        SetJobLog(new JobLogVM
                        {
                            DocumentId = documentId,
                            Status = (int)RendERA.Infrastructure.Enum.JobTrackingStatus.UnderProcess,
                            LogDetail = "job is assigned to Partner"
                        });

                        var msgId = _messagesService.GenrateMsgByDoc(messagetoPost);
                        doc.AssignedPartnerId = PartnerId;
                        doc.MessageId = msgId;
                        _documentService.UpdateDoc(doc);
                        if (TempData["Msg"] != null)
                            TempData["Msg"] = "Assigned successfully" + TempData["Msg"] as string;
                        else
                            TempData["Msg"] = "Assigned successfully"; 
                        return RedirectToAction("DocumentList", "Documents");
                    }
                    TempData["Msg"] = "Already assigned";
                    return RedirectToAction("DocumentList", "Documents");
                }
                TempData["Msg"] = "Please select Partner";
                return RedirectToAction("DocumentList", "Documents");
            }
            catch{
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            }
        }

        public IActionResult AssignTeamProject(int documentId, int teamId,int projectId)
        {
            try
            {
                if (teamId > 0 && projectId > 0)
                {
                    var value = _contextAccessor.HttpContext.Session.GetString(
                            RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                    if (value == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    var User = _accountService.GetUser(new Guid(value));
                    var doc = _documentService.GetDocById(documentId);
                    if (doc.AssignedTeamId == null || doc.AssignedTeamId != teamId)
                    {
                        doc.AssignedTeamId = teamId;
                        doc.AssignedProjectId = projectId;
                        _documentService.UpdateDoc(doc);
                      
                        TempData["Msg"] = "Assigned successfully";
                        return RedirectToAction("DocumentList", "Documents");
                    }
                    TempData["Msg"] = "Already assigned";
                    return RedirectToAction("DocumentList", "Documents");
                }
                TempData["Msg"] = "Please select Team";
                return RedirectToAction("DocumentList", "Documents");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            }
        }

        // GET: Documents/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var document = _documentService.GetDocById(id);
                if (document == null)
                {
                    return NotFound();
                }
                return View(document);
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            }
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            { 
                var value = _contextAccessor.HttpContext.Session.GetString(
                          RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                _documentService.DeleteDoc(id);
                TempData["Msg"] = "Deleted successfully";
                return RedirectToAction("DocumentList", "Documents");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("DocumentList", "Documents");
            }
        }

        public IActionResult SetParameter(int id)
        {
            var model = new DocParametersMapVM();
            model.DocumentId = id;
          //  model.List = _parameterService.GetSelectedParameterByDocId(id);
            return PartialView("_SetParameter", model);
        }

        private void SetJobLog(JobLogVM model) {
            _jobLogService.Insert(model);
      }

        public string GetUniqueAlphaNumeric() 
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, _alphanumericLength).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

    #region Utility
    private IList<SelectListItem> GetUserListItemsByUserType(int userType)
        {
            var elements = _accountService.GetAllUserByUserType(userType);
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.UserId.ToString(),
                    Text = element.user
                });
            }
            return selectList;
        }
        
        private IList<SelectListItem> GetTeamListItemsByPartner(Guid id)
        {
            var elements = _teamService.GetTeamListByPartnerId(id);
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.Id.ToString(),
                    Text = element.Name
                });
            }
            return selectList;
        }
        private IList<SelectListItem> GetProjectListItemsByPartner(Guid id)
        {
            var elements = _projectService.GetProjectListByPartnerId(id);
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.Id.ToString(),
                    Text = element.Name
                });
            }
            return selectList;
        }

        public void CheckRemoveFile()
        {
            int BeginnerDays = _ConfigurationSettingsSrvice.GetNumberOfDaysDurationByUserType((int)RendERA.Infrastructure.Enum.UserCategory.Beginner);
            int Expert = _ConfigurationSettingsSrvice.GetNumberOfDaysDurationByUserType((int)RendERA.Infrastructure.Enum.UserCategory.Expert);
            int Pro = _ConfigurationSettingsSrvice.GetNumberOfDaysDurationByUserType((int)RendERA.Infrastructure.Enum.UserCategory.Pro);
            if (BeginnerDays > 0)
            {
                RemoveFile((int)RendERA.Infrastructure.Enum.UserCategory.Beginner, BeginnerDays);
            }
            if (Expert > 0)
            {
                RemoveFile((int)RendERA.Infrastructure.Enum.UserCategory.Expert, Expert);
            }
            if (Pro > 0)
            {
                RemoveFile((int)RendERA.Infrastructure.Enum.UserCategory.Pro, Pro);
            }
        }

        public void RemoveFile(int UserType, int days)
        {
            var doclist = _documentService.GetAllDocs();
            foreach (var doc in doclist)
            {
                DateTime futurDate = doc.DatePosted;
                DateTime TodayDate = DateTime.Now;
                var numberOfDays = (TodayDate - futurDate).TotalDays;
                if ( numberOfDays > days)
                {
                    string result = "";
                    var fileurl = doc.FileUploadUrl;
                    int index = fileurl.IndexOf("\\"+doc.FileName);
                    if (index != -1)
                    {
                         result = fileurl.Remove(index);
                    }
                    new Common.Files().RemoveFile(doc.FileName,result);
                    if (doc.FileDownloadUrl != null) {
                        new Common.Files().RemoveFile(doc.FileName, result);
                    }
                }
            }

        }
        #endregion
    }
}
