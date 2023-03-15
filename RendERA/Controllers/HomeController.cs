using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RendERA.DB.Models;
using RendERA.DB.ViewModels;
using RendERA.Models;
using RendERA.ServiceManager.IServices;

namespace RendERA.Controllers
{
    public class HomeController : Controller
    {
        private ServiceManager.IServices.IDocumentSrv _documentService;
        private readonly IMessagesSrv _messagesService;
        private readonly IMessageReplySrv _messageReplysService;
        private readonly IConfigurationSettingsSrv _ConfigurationSettingsSrvice;
        private readonly IParameterSrv _ParameterService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public HomeController(IConfigurationSettingsSrv ConfigurationSettingsSrvice,ServiceManager.IServices.IDocumentSrv documentService, RendERA.ServiceManager.IServices.IAccountSrv accountService, IHttpContextAccessor contextAccessor, IMessagesSrv messagesService, IMessageReplySrv messageReplysService, IParameterSrv ParameterService)
        {
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
            return RedirectToAction("Login", "Account");
        }

        #region User
        public IActionResult UserDashboard()
        {
           
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View();
        }

   

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Statistics()
        {
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View();
        }

        public IActionResult Analytics()
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

                var docList = _documentService.GetAllDocsByUserId(User.UserId);
                foreach (var doc in docList)
                {
                    if (doc.IsParameter == true)
                    {
                        documentList.Add(doc);
                    }
                }
                ViewBag.usertype = User.UserType;
                if (TempData["Msg"] != null)
                    ViewBag.Msg = TempData["Msg"] as string;
                return View(documentList);
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Render()
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

                var docList = _documentService.GetAllDocsByUserId(User.UserId);
                foreach (var doc in docList)
                {
                    if (doc.FileDownloadUrl != null)
                    {
                        documentList.Add(doc);
                    }
                }
                if (TempData["Msg"] != null)
                    ViewBag.Msg = TempData["Msg"] as string;
                return View(documentList);
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        public IActionResult NewJob(string text)
        {
            List<RendERA.DB.ViewModels.DocumentVM> documentList = new List<RendERA.DB.ViewModels.DocumentVM>();
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            //parameter list


            ViewBag.doclist = GetDocumenList(User.UserId);
            ViewBag.Parameters = GetDocumentParameters();
            ViewData["usertype"] = "User";
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;

            return View();
        }

        public JsonResult GetDocumentListBySoftwareType(int id) {
            
            if((int)RendERA.Infrastructure.Enum.RenderaSoftware.Maya == id) {

                string[] Maya = { ".ma", ".mb", ".obj", ".anim" };
                var data = GetDocumenListByFileType(Maya);
                return Json( data);
            }

            if((int)RendERA.Infrastructure.Enum.RenderaSoftware.ThreeDMax == id) {

                string[] _3dmax = { ".max", ".3ds", ".fbx" };
                var data = GetDocumenListByFileType(_3dmax);
                return Json( data);
            }
            if((int)RendERA.Infrastructure.Enum.RenderaSoftware.Cinema4D == id) {

                string[] Cinema4D = { ".cd4" };
                var data = GetDocumenListByFileType(Cinema4D);
                return Json( data);
            }
            if((int)RendERA.Infrastructure.Enum.RenderaSoftware.Blender == id) {

                string[] Blender = { ".blend" };
                var data = GetDocumenListByFileType(Blender);
                return Json( data);
            }

            if((int)RendERA.Infrastructure.Enum.RenderaSoftware.Nuke == id) {

                string[] Nuke = { ".nuke", ".nkpla", ".nk" };
                var data = GetDocumenListByFileType(Nuke);
                return Json( data);
            }

            if((int)RendERA.Infrastructure.Enum.RenderaSoftware.Clarisse == id) {
                string[] Clarisse = { ".ple", ".ste", ".obj" };
                var data = GetDocumenListByFileType(Clarisse);
                return Json( data);
            }
            else
            { 

                string[] Houdini = { ".hip", "hipnc", ".bgeo", ".geo", ".picnc", ".vfl" };
                var data = GetDocumenListByFileType(Houdini);
                return Json( data);
            }
        }




        public IActionResult Assets(string text, string filter)
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
                ViewBag.Msg = text;
                var User = _accountService.GetUser(new Guid(value));
                ViewBag.UserType = User.UserType;
                ViewBag.Msg = text;
                ViewBag.usertype = User.UserType;
                documentList = _documentService.GetAllDocsByUserId(User.UserId);
                if (documentList.Count == 0)
                {
                    return View(documentList);
                }
                if (TempData["Msg"] != null)
                    ViewBag.Msg = TempData["Msg"] as string;
                return View(documentList);
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        public IActionResult GetListofUserDocument(string filter) {
            
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<RendERA.DB.ViewModels.DocumentVM> documentList = new List<RendERA.DB.ViewModels.DocumentVM>();
            var User = _accountService.GetUser(new Guid(value));
            if (filter == "0" || filter == null)
            {
                documentList = _documentService.GetAllDocsByUserId(User.UserId);
            }
            else {
                 documentList = _documentService.GetAllDocsByFilter(filter, User.UserId);
            }
        
            return PartialView("_GetListofUserDocument", documentList);
        }

        public IActionResult UserProfile()
        {
            ViewData["usertype"] = "User";
            return View();
        }        
        #endregion

        #region Admin
        public IActionResult AdminDashboard()
        {
            ViewData["usertype"] = "Admin";
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View();
        }

        public IActionResult GetFileRemoveDuration()
        {

            var obj = _ConfigurationSettingsSrvice.GetRemoveFileDuration();
            return PartialView("_GetFileRemoveDuration", obj);
        }

        [HttpPost]
        public IActionResult InsertFileRemoveDuration(ConfigrationSettingsVM model)
        {
           _ConfigurationSettingsSrvice.Update(model);
            return RedirectToAction("AdminDashboard");
        }
        #endregion

        #region Partner

        public IActionResult PartnerDashboard()
        {
            ViewData["usertype"] = "Partner";
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View();
        }
        #endregion

        #region Utility

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<ParametersVM> GetDocumentParameters()
        {
            var parameterList = _ParameterService.GetAll();
            var list = new List<ParametersVM>();

            if (parameterList != null)
            {
                foreach (var parameter in parameterList)
                {
                    list.Add(parameter);
                }
            }
            return list;
        }

        private IList<SelectListItem> GetDocumenList(Guid userId)
        {
            var doclist = _documentService.GetAllDocsByUserId(userId);

            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "/", Value = "0" });
            foreach (var doc in doclist)
            {
                if (doc.IsParameter == false)
                {
                    list.Add(new SelectListItem { Text = "Job-" + doc.Id, Value = doc.Id.ToString() });
                }
            }
            return list;
        }
        private IList<SelectListItem> GetDocumenListByFileType(string[] extenstionArray)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                     RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            var User = _accountService.GetUser(new Guid(value));
            var doclist = _documentService.GetAllDocsByUserId(User.UserId);
            
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "/", Value = "0" });
            foreach (var doc in doclist)
            {
                if (doc.IsParameter == false && extenstionArray.Contains(doc.FileName.Substring(doc.FileName.LastIndexOf('.'))))
                {
                    list.Add(new SelectListItem { Text =doc.FileName +" - "+ doc.DatePosted.ToString("yyyy/MM/dd hh:mm tt"), Value = doc.Id.ToString() });
                }
            }
            return list;
        }

     
        #endregion

    }
}
