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
    public class MasterCategoryController : Controller
    {
        private readonly IMasterCategorySrv _masterCategoryService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public MasterCategoryController(IMasterCategorySrv masterCategoryService, IAccountSrv accountService, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _masterCategoryService = masterCategoryService;
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
            var masterCategorys = new List<MasterCategoryVM>();
            var User = _accountService.GetUser(new Guid(value));
            masterCategorys = _masterCategoryService.GetAll();
            ViewBag.usertype = User.UserType;
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View(masterCategorys);
        }

        public IActionResult Create()
        {
            var model = new MasterCategoryVM();
            return PartialView("_Create", model);
        }

        public IActionResult Edit(int id)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                    RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = _masterCategoryService.GetById(id);
            return PartialView("_Edit", model);}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MasterCategoryVM model)
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
                    if (IsAlreadyContain(model))
                    {
                        TempData["Msg"] = model.Name + " name is already exist";
                        return RedirectToAction("Index");
                    }
                    _masterCategoryService.Insert(model);
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
        public ActionResult Edit(MasterCategoryVM model)
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
                    var masterCategory = _masterCategoryService.GetById(model.Id);
                    if (IsAlreadyContain(model))
                    {
                        TempData["Msg"] = model.Name + " name is already exist";
                        return RedirectToAction("Index");
                    }
                    masterCategory.Name = model.Name;
                    _masterCategoryService.Update(masterCategory);
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
                _masterCategoryService.Delete(id);
                TempData["Msg"] = "Deleted Successfully";
            }
            catch
            {
                TempData["Msg"] = "Failed to Delete";
            }
            return RedirectToAction("Index");
        }

      
        private bool IsAlreadyContain(MasterCategoryVM model)
        {
            var list = _masterCategoryService.GetAll();
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
