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

namespace RendERA.Controllers
{
    
    public class TeamsController : Controller
    {
        private readonly ITeamSrv _teamService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public TeamsController(ITeamSrv teamService, IAccountSrv accountService, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _teamService = teamService;
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
            var teams = new List<TeamVM>();
            var User = _accountService.GetUser(new Guid(value));
            teams = _teamService.GetTeamListByPartnerId(User.UserId);            
            ViewBag.usertype = User.UserType;
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View(teams);
        }
       
        public IActionResult TeamFilter() { 
            ViewBag.PartnerList = _accountService.GetAllUserByUserType((int)RendERA.Infrastructure.Enum.UserType.Partner);
            return View();
        }

        public IActionResult PartnerTeamList(string userId) {
            var value = _contextAccessor.HttpContext.Session.GetString(
                RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var teams = new List<TeamVM>();
            if (userId == "0")
            {
                teams = _teamService.GetAll();
            }
            else {
                var User = _accountService.GetUser(new Guid(userId));
                teams = _teamService.GetTeamListByPartnerId(User.UserId);
            }
            return PartialView("_PartnerTeamList", teams);
        }

        public IActionResult CreateTeam()
        {
            var model = new TeamVM();
            return PartialView("_Create", model);
        }

        public IActionResult EditTeam(int id)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                    RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = _teamService.GetById(id);
            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeam(TeamVM model)
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
                if (ModelState.IsValid)
                {
                    if (IsAlreadyContain(model)){
                        TempData["Msg"] =  model.Name + " name is already exist";
                        return RedirectToAction("Index");
                    }
                    if (model.ProfileImage != null)
                    {
                        string uniqueFileName = UploadedFile(model);
                        model.ProfilePicture = uniqueFileName;
                    }

                    model.PartnerId = User.UserId;
                    _teamService.Insert(model);
                    TempData["Msg"] = "Added successfully";
                }
            }
            catch
            {

                TempData["Msg"] = "Failed to add";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeam(TeamVM model)
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
                if (ModelState.IsValid)
                {
                    var team = _teamService.GetById(model.Id);
                    if (IsAlreadyContain(model)){
                        TempData["Msg"] =  model.Name + " name is already exist";
                        return RedirectToAction("Index");
                    }
                    if (model.ProfileImage != null)
                    {
                        string uniqueFileName = UploadedFile(model);
                        team.ProfilePicture = uniqueFileName;
                    }
                    team.Degination = model.Degination;
                    team.Name = model.Name;
                    _teamService.Update(team);
                    TempData["Msg"] = "Updated successfully";
                }
            }
            catch
            {
                TempData["Msg"] = "Failed to Update";
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTeam(int id)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                        RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                _teamService.Delete(id);
                TempData["Msg"] = "Deleted Successfully";
            }
            catch
            {
                TempData["Msg"] = "Failed to Delete";
            }
            return RedirectToAction("Index");
        }

        private string UploadedFile(TeamVM model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProfileImg");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private bool IsAlreadyContain(TeamVM model)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                 RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            var User = _accountService.GetUser(new Guid(value));
            var list = _teamService.GetTeamListByPartnerId(User.UserId);
            foreach (var obj in list)
            {
                if (model.Name == obj.Name) {
                    return true;
                }
            }
            return false;
        }

    }
}
