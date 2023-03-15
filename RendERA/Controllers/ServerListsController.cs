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
    public class ServerListsController : Controller
    {
        private readonly IServerListSrv _serverListService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public ServerListsController(IAccountSrv accountService, IServerListSrv serverListService, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _serverListService = serverListService;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            var serverList = _serverListService.GetAll();
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View(serverList);
        }

        //public JsonResult GetServerList(int userId)
        //{
        //    var serverList = _serverListService.GetAll();

        //    return Json(new { serverList = serverList }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetServerList()
        {
            var serverList = _serverListService.GetAll();
            return Json(serverList);
        }

        public IActionResult CreateServerList()
        {
            var model = new ServerListVM();
            return PartialView("_Create", model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult Create(ServerListVM model)
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
                    _serverListService.Insert(model);
                    TempData["Msg"] = "Added successfully";
                }
            }
            catch
            {
                TempData["Msg"] = "Failed to add";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                    RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = _serverListService.GetById(id);
            return PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServerListVM model)
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
                    var serverList = _serverListService.GetById(model.Id);
                    serverList.IpAddress = model.IpAddress;
                    serverList.Name = model.Name;
                    _serverListService.Update(serverList);
                    TempData["Msg"] = "Updated successfully";
                }
            }
            catch
            {
                TempData["Msg"] = "Failed to Update";
            }
            return RedirectToAction("Index");
        }

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
                _serverListService.Delete(id);
                TempData["Msg"] = "Deleted Successfully";
            }
            catch
            {
                TempData["Msg"] = "Failed to Delete";
            }
            return RedirectToAction("Index");
        }
    }
}