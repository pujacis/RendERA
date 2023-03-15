using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RendERA.ServiceManager.IServices;
using RendERA.DB.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.CodeAnalysis;

namespace RendERA.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectSrv _projectService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public ProjectsController(IProjectSrv projectService, IAccountSrv accountService, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _projectService = projectService;
            _contextAccessor = contextAccessor;
        }

        public  IActionResult Index()
        {
             var value = _contextAccessor.HttpContext.Session.GetString(
                  RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var projects = new List<ProjectVM>();
            var User = _accountService.GetUser(new Guid(value));           
            projects = _projectService.GetProjectListByPartnerId(User.UserId); 
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View(projects);
        }

        public IActionResult ProjectFilter()
        {
            ViewBag.PartnerList = _accountService.GetAllUserByUserType((int)RendERA.Infrastructure.Enum.UserType.Partner);
            return View();
        }

        public IActionResult PartnerProjectList(string userId)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var projects = new List<ProjectVM>();
            if (userId == "0")
            {
                projects = _projectService.GetAll();
            }
            else
            {
                var User = _accountService.GetUser(new Guid(userId));
                projects = _projectService.GetProjectListByPartnerId(User.UserId);
            }
            return PartialView("_PartnerProjectList", projects);
        }

        public IActionResult CreateProject()
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new ProjectVM();
            model.StatusList = GetStatusListItems();
            return PartialView("_Create", model);
        }

        public IActionResult EditProject(int id)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                    RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = _projectService.GetById(id);
            model.StatusList = GetStatusListItems();
            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(ProjectVM model)
        {
            try { 
                var value = _contextAccessor.HttpContext.Session.GetString(
                      RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var User = _accountService.GetUser(new Guid(value));
                if (ModelState.IsValid)
                {
                    if (IsAlreadyContain(model)){
                        TempData["Msg"] = model.Name+" name is already exist";
                        return RedirectToAction("Index");
                    }
                    model.PartnerId = User.UserId;
                    _projectService.Insert(model);
                    TempData["Msg"] = "Added Successfully";
                }
            }
            catch
            {
                TempData["Msg"] = "Failed to added";
            }
            return RedirectToAction("Index");
        }
        
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(ProjectVM model)
        {
            try { 
            var value = _contextAccessor.HttpContext.Session.GetString(
                  RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            if (ModelState.IsValid)
                {
                    if (IsAlreadyContain(model)){
                        TempData["Msg"] = model.Name + " name is already exist";
                        return RedirectToAction("Index");
                    }
                    var project = _projectService.GetById(model.Id);
                project.Status = model.Status;
                project.Name = model.Name;
                _projectService.Update(project);
                    TempData["Msg"] = "Updated successfully";
                }

            }
            catch
            {
                TempData["Msg"] = "Failed to update";
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteProject(int id)
        {
            try { 
                var value = _contextAccessor.HttpContext.Session.GetString(
                        RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                _projectService.Delete(id);
                TempData["Msg"] = "Deleted successfully";
            }
            catch
            {
                TempData["Msg"] = "Failed to Delete";
            }
            return RedirectToAction("Index");
        }

        private IList<SelectListItem> GetStatusListItems()
        {
           

            var selectList = new List<SelectListItem>();

            
            var elements = Enum.GetValues(typeof(RendERA.Infrastructure.Enum.Status)).Cast<RendERA.Infrastructure.Enum.Status>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = ((int)element).ToString(),
                    Text =  RendERA.Infrastructure.Enum.GetEnumDescription(element)
            });
            }
            return selectList;
        }

        private bool IsAlreadyContain(ProjectVM model)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                 RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            var User = _accountService.GetUser(new Guid(value));
            var list = _projectService.GetProjectListByPartnerId(User.UserId);
            foreach (var obj in list)
            {
                if (model.Name == obj.Name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
