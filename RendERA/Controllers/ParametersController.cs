using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RendERA.Common;
using RendERA.DB.ViewModels;
using RendERA.Models;
using RendERA.ServiceManager.IServices;

namespace RendERA.Controllers
{
    public class ParametersController : Controller
    {
        private ServiceManager.IServices.IDocumentSrv _documentService;
        private readonly IParameterSrv _parameterService;
        private readonly IJobLogSrv _jobLogService;
        private readonly IMasterCategorySrv _masterCategoryService;
        private readonly IPaymentTransactionSrv _PaymentTransactionService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session

        public ParametersController(ServiceManager.IServices.IDocumentSrv documentService, IPaymentTransactionSrv PaymentTransactionService, RendERA.ServiceManager.IServices.IAccountSrv accountService, IHttpContextAccessor contextAccessor, IParameterSrv ParameterService, IMasterCategorySrv masterCategoryService, IJobLogSrv jobLogService)
        {
            _parameterService = ParameterService;
            _masterCategoryService = masterCategoryService;
            _PaymentTransactionService = PaymentTransactionService;
            _accountService = accountService;
            _contextAccessor = contextAccessor;
            _documentService = documentService;
            _jobLogService = jobLogService;
        }
        public IActionResult Index()
        {
            var model = GetDocumentParameters();
            ViewBag.CategoryList = GetCategoryListItems();
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View(model);
        }

        public IActionResult SetParameter(int id)
        {
            try
            {
                var model = _parameterService.GetSelectedParameterByDocId(id);
                model.DocumentId = id;
                var parmeter = _parameterService.GetById(model.SelectedCPU);
                var transaction = _PaymentTransactionService.GetByDocId(model.DocumentId);
                if (transaction != null) {
                    model.Transaction = transaction;
                } else {
                    model.Transaction = new PaymentTransactionVM();
                }
                if (parmeter != null)
                {
                    model.Is2XCPU = parmeter.Category == (int)RendERA.Infrastructure.Enum.Category.CPU2X ? true : false;
                }
                model.CPUList = _parameterService.GetCPUList();
                model.TwoXCPUList = _parameterService.Get2XCPUList();
                model.ProcessorModelList = _parameterService.GetProcessorModelList();
                model.ProcessorTypeList = _parameterService.GetProcessorTypeList();
                var nodesForProjectRederding = _parameterService.GetNodesForProjectRederding();
                var renderTimeOfFrame = _parameterService.GetRenderTimeOfFrame();
                if (nodesForProjectRederding != null)
                {
                    model.PriceOfRenderTimePerSec = nodesForProjectRederding.Price;
                }
                if (renderTimeOfFrame != null)
                {
                    model.PriceOfPerNodeForProjectRender = renderTimeOfFrame.Price;
                }
                var doc = _documentService.GetDocById(model.DocumentId);
                model.IsSubmitted = doc.IsSubmitted == null ? false : (bool)doc.IsSubmitted;
                model.IsParameter = doc.IsParameter;
                if (transaction != null)
                {
                    return PartialView("_SubmittedJobDoc", model);
                }
                else
                {
                    return PartialView("_SetParameter", model);
                }
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        [HttpPost]
        public IActionResult SetParameter(DocParametersMapVM model)
        {
            try
            {
                if (model != null && model.DocumentId > 0)
                {
                    _parameterService.SetParameterByDocId(model);
                    //save Transaction details
                    if (model.Transaction.Status != null) {
                        _PaymentTransactionService.Insert(model.Transaction);
                        if (model.Transaction.Status == "COMPLETED") {
                            ProceedPayment(model.DocumentId);
                        }
                    }
 

                    // set paramter is true
                    var doc = _documentService.GetDocById(model.DocumentId);
                    doc.IsParameter = true;
                    TempData["Msg"] = "Parameter set successfully";
                    if (model.IsSubmitted == true) {
                       // doc.IsSubmitted = true;
                       // doc.TransactionId = model.TransactionId;

                        TempData["Msg"] = "Submitted successfully";
                    }
                    _documentService.UpdateDoc(doc);
                }
                return RedirectToAction("Analytics", "Home");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        public IActionResult AdditionalParameter(int id)
        {
            try
            {
                var model = GetAdditionalParaByDocId(id);
                model.DocumentId = id;

                return View(model);
                // return PartialView("_SetAdditionalParameter", model);
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        public IActionResult SetAdditionalParameter(int id)
        {
            try
            {
                var model = GetAdditionalParaByDocId(id);
                model.DocumentId = id;
                var doc = _documentService.GetDocById(model.DocumentId);
                var transaction = _PaymentTransactionService.GetByDocId(model.DocumentId);
                model.IsSubmitted = doc.IsSubmitted == null ? false : (bool)doc.IsSubmitted;
                if (transaction!= null)
                {
                    return PartialView("_SubmittedAdditionalParameter", model);
                }
                else
                {
                    return PartialView("_SetAdditionalParameter", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserDashboard", "Home");
            }
        }

        [HttpPost]
        public JsonResult AdditionalParameter(AdditionalParameterVM model)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                
                var User = _accountService.GetUser(new Guid(value));
                if (model != null)
                {
                    var dropdownlist = model.DropDownFields;
                    if (dropdownlist != null) {
                        foreach (var dropdown in dropdownlist)
                        {
                            _parameterService.InsertDropdownDocParaMapping(model.DocumentId, dropdown);
                        }
                    }
                    var inputFields = model.InputFields;
                    if (inputFields != null)
                    {
                        foreach (var input in inputFields)
                        {
                            _parameterService.InsertInputDocParaMapping(model.DocumentId, input);
                        }

                    }

                       //Add job log
                        SetJobLog(new JobLogVM
                        {
                            DocumentId = model.DocumentId,
                            Status = (int)RendERA.Infrastructure.Enum.JobTrackingStatus.InProgress,
                            LogDetail = "Parameters changed  for the Job by "+ User.UserName
                        });

                    return Json(new { status = "Success", message = "Success" });
                }
                return Json(new { status = "failed", message = "failed" });
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something went wrong";
                // return Json("ok");
                return Json(new { status = "failed", message = "failed" });
            }
        }

        public JsonResult ValidateCouponCode(string code)
        {
            try
            {
                var discountParameter = _parameterService.validateCouponCode(code);
                if (discountParameter == null) {
                    return Json(new { status = "failed", discount = 0, couponId = 0 });
                }
                return Json(new { status = "Success", discount = discountParameter.Price, couponId = discountParameter.Id });

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something went wrong";
                // return Json("ok");
                return Json(new { status = "failed", discount = 0, couponId = 0 });
            }
        }

        [HttpPost]
        public IActionResult InsertParameter(ParametersVM model)
        {
            if (model != null)
            {
                model.Id = _parameterService.InsertParameter(model);
                _parameterService.AddRemoveSelectedParameterOfSoftMap(model);
                _parameterService.AddRemoveSelectedParameterOfUserCategory(model);
                TempData["Msg"] = "Added successfully";
            }
            return RedirectToAction("Index");
        }

        public IActionResult CreateParameter()
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ParametersVM();
            model.CategoryList = GetCategoryListItems();
            model.UserCategoryList = _parameterService.GetAllUserCategorylist();
            model.RenderSoftwareList = _parameterService.GetAllSoftwareParameterlist();
            return PartialView("_Create", model);
        }

        public IActionResult EditParameter(int id)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                    RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = _parameterService.GetById(id);

            if (model.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding || model.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame)
            {
                if (model.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding)
                {
                    model.CategoryList.Add(new SelectListItem
                    {
                        Value = ((int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding).ToString(),
                        Text = RendERA.Infrastructure.Enum.GetEnumDescription(RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding)
                    });
                }
                else
                {
                    model.CategoryList.Add(new SelectListItem
                    {
                        Value = ((int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame).ToString(),
                        Text = RendERA.Infrastructure.Enum.GetEnumDescription(RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame)
                    });
                }
            }
            else {
                model.CategoryList = GetCategoryListItems();
            }
            model.RenderSoftwareList = _parameterService.GetSelectedParameterOfSoftMapByParameterId(model.Id);
            model.UserCategoryList = _parameterService.GetSelectedParameterOfUserCategoryByParameterId(model.Id);
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public IActionResult EditParameter(ParametersVM model)
        {
            if (model != null)
            {
                _parameterService.Update(model);
                _parameterService.AddRemoveSelectedParameterOfSoftMap(model);
                _parameterService.AddRemoveSelectedParameterOfUserCategory(model);
                TempData["Msg"] = "Updated successfully";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteParameter(int id)
        {
            try
            {
                if (id > 0)
                {
                    _parameterService.Delete(id);
                    TempData["Msg"] = "Deleted successfully";
                }
            }
            catch
            {
                TempData["Msg"] = "This paramter is assigned to some Documents";
            }
            return RedirectToAction("Index");
        }

        private AdditionalParameterVM GetAdditionalParaByDocId(int id)
        {
            var doc = _documentService.GetDocById(id);
            var User = _accountService.GetUser(doc.UploadedBy);
            return _parameterService.GetAdditionalParaByDocId(id,User.UserCategory);
        }

        private List<ParametersVM> GetDocumentParameters()
        {
            var parameterList = _parameterService.GetAll();
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

        private IList<SelectListItem> GetCategoryListItems()
        {

            var selectList = new List<SelectListItem>();
            var paramterList = _parameterService.GetAll();
            var masterCategoryList = _masterCategoryService.GetAll();

            var elements = Enum.GetValues(typeof(RendERA.Infrastructure.Enum.Category)).Cast<RendERA.Infrastructure.Enum.Category>();
            foreach (var element in elements)
            {
                // adde all the Category in theselectList except price of Nodes For Project Rederding and Price of Render Time Of Frame per second
                if (((int)element) != (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding && ((int)element) != (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = ((int)element).ToString(),
                        Text = RendERA.Infrastructure.Enum.GetEnumDescription(element)
                    });
                }
            }
           
            //If price of Nodes For Project Rederding is already set so remove from category list
            if (paramterList.Find(a => a.Category == (int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding) == null) {
                selectList.Add(new SelectListItem
                {
                    Value = ((int)RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding).ToString(),
                    Text = RendERA.Infrastructure.Enum.GetEnumDescription(RendERA.Infrastructure.Enum.Category.NodesForProjectRenderding)
                });
            }

            //If Price of Render Time Of Frame per second  is already set so remove from category list
            if (paramterList.Find(a => a.Category == (int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame) == null)
            {
                selectList.Add(new SelectListItem
                {
                    Value = ((int)RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame).ToString(),
                    Text = RendERA.Infrastructure.Enum.GetEnumDescription(RendERA.Infrastructure.Enum.Category.RenderTimeOfFrame)
                });

            }

            foreach (var masterCategory in masterCategoryList){
                selectList.Add(new SelectListItem
                {
                    Value = (masterCategory.Id).ToString(),
                    Text = masterCategory.Name
                });
            }

            return selectList;
        }

        private IList<SelectListItem> GetUserCategoryListItems()
        {
            var selectList = new List<SelectListItem>();
            var elements = Enum.GetValues(typeof(RendERA.Infrastructure.Enum.Category)).Cast<RendERA.Infrastructure.Enum.Category>();
            foreach (var element in elements)
            {
              selectList.Add(new SelectListItem
              {
                    Value = ((int)element).ToString(),
                    Text = RendERA.Infrastructure.Enum.GetEnumDescription(element)
              });
            }
            return selectList;
        }


        public void ProceedPayment(int id)
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

                //Add job log
                SetJobLog(new JobLogVM
                {
                    DocumentId = doc.Id,
                    Status = (int)RendERA.Infrastructure.Enum.JobTrackingStatus.Submitted,
                    LogDetail = "Job successfully Submitted and  assigned to admin"
                });
            }
            catch
            {
                TempData["Msg"] = "Failed to send email";
            }

            if (TempData["Msg"] != null)
                TempData["Msg"] = "Payment Done &" + TempData["Msg"] as string;
            else
                TempData["Msg"] = "Payment Done!";
        }
        private void SetJobLog(JobLogVM model)
        {
            _jobLogService.Insert(model);
        }
    }
}