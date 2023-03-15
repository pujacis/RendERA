using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RendERA.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// dependency
        /// </summary>
        private ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session
        
        public AccountController(ServiceManager.IServices.IAccountSrv accountService, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _contextAccessor = contextAccessor;
        }
        
        public IActionResult Register()
        {            
            
            DB.ViewModels.UserRegisterationVM user = new DB.ViewModels.UserRegisterationVM() { UserType = (int)Infrastructure.Enum.UserType.User };
            return View(user);
        }

        public IActionResult RegisterPartner()
        {
            DB.ViewModels.UserRegisterationVM user = new DB.ViewModels.UserRegisterationVM() { UserType = (int)Infrastructure.Enum.UserType.Partner };
            return View("Register", user);
        }

        public IActionResult RegisterAdmin()
        {
            DB.ViewModels.UserRegisterationVM user = new DB.ViewModels.UserRegisterationVM() { UserType = (int)Infrastructure.Enum.UserType.Admin };
            return View("Register", user);
        }
        [HttpPost]
        public IActionResult Register(DB.ViewModels.UserRegisterationVM user)
        {
            try
            {
                Guid UserId = _accountService.RegisterUser(user);
                if (UserId!=null)
                {
                    //set user in session
                    _contextAccessor.HttpContext.Session.SetString(
                        Infrastructure.Enum.SessionKey.LoggedInUserId.ToString(),
                        UserId.ToString());
                    
                    return RedirectToAction("GenerateAccountVerificationCode");
                }
                else
                {
                    if(user.UserCategory == (int)RendERA.Infrastructure.Enum.UserType.User)
                        return RedirectToAction("Login");
                    else
                        return RedirectToAction("PartnerLogin");
                }
            }
            catch (Exception ex)
            {

                TempData["Errmsg"] = ex.Message.ToString();
                // ViewBag.Errmsg = ex.Message.ToString();
                if (user.UserCategory == (int)RendERA.Infrastructure.Enum.UserType.User)
                    return RedirectToAction("Login");
                else
                    return RedirectToAction("PartnerLogin");
            }
        }

        public IActionResult Login()
        {
            if (TempData["Errmsg"] != null)
                ViewBag.Errmsg = TempData["Errmsg"] as string;
            return View();
        }

        public IActionResult PartnerLogin()
        {
            if (TempData["Errmsg"] != null)
                ViewBag.Errmsg = TempData["Errmsg"] as string;
            return View();
        }

        [HttpPost]
        public IActionResult PartnerLogin(DB.ViewModels.LoginViewModel LoginVm)
        {
            Guid UserId = _accountService.Login(LoginVm);
            if (UserId == Guid.Empty)
            {
                ViewBag.Msg = "Invalid Login Id or Password";
                return View();
            }

            //set user in session
            _contextAccessor.HttpContext.Session.SetString(
                Infrastructure.Enum.SessionKey.LoggedInUserId.ToString(),
                UserId.ToString());
            DB.Models.Users User = _accountService.GetUser(UserId);
            if (User.IsVerified == false) return RedirectToAction("GenerateAccountVerificationCode");
            if (User.UserType == (short)Infrastructure.Enum.UserType.User)
            {
                ViewBag.Msg = "User Not registered as a Partner";
                return View();
                //if (User.ReadWelcomePopup == true) return RedirectToAction("UserDashboard", "Home");
                //else { return RedirectToAction("UserWelcome", "Account"); }
            }
            else if (User.UserType == (short)Infrastructure.Enum.UserType.Partner)
            {

                var value = _contextAccessor.HttpContext.Session.GetString(
                     Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());

                return RedirectToAction("PartnerDashBoard", "Home");
            }

            else if (User.UserType == (short)Infrastructure.Enum.UserType.Admin)
            {

                var value = _contextAccessor.HttpContext.Session.GetString(
                     Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());

                return RedirectToAction("AdminDashboard", "Home");
            }
            else
            {
               // ViewBag.Msg = "Unknown Usertype";
                return View();

            }
        }

        public IActionResult UserWelcome()
        {
            return View();
        }

        public JsonResult ForgetPassword(string email,int userType) {
            // ViewBag.Errmsg = ex.Message.ToString();
            var user = _accountService.GetUserByEmail(email);
            if (user != null)
            {
                if (userType == user.UserType)
                {
                    Helpers.Utility.SendEmail(user.Email, "RendERA: Recover Password", "<br/> You account Password is : <b>" + user.Password + "</b>");
                    return Json(new { status = "Success", message = "Sent" });
                }
                else
                {
                    return Json(new { status = "Success", message = "Not registered as a "+ Enum.GetName(typeof(RendERA.Infrastructure.Enum.UserType), userType) });
                }
            }
            else {
                return Json(new { status = "Failed", message = "Invaild Email" });
            }
        }

        [HttpPost]
        public IActionResult UserWelcome(int[] RenderSoftware)
        {
            string UserId = _contextAccessor.HttpContext.Session.GetString(Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (string.IsNullOrEmpty(UserId)) return RedirectToAction("Login");
             
            if (_accountService.ChooseDefaultRendingSoftware(new Guid(UserId), RenderSoftware)) return RedirectToAction("UserDashboard","Home");
            else return RedirectToAction("Login");

        }

        [HttpPost]
        public IActionResult Login(DB.ViewModels.LoginViewModel LoginVm)
        {
            Guid UserId = _accountService.Login(LoginVm);
            if (UserId == Guid.Empty)
            {
                ViewBag.Msg = "Invalid Login Id or Password";
                return View();
            }

            //set user in session
            _contextAccessor.HttpContext.Session.SetString(
                Infrastructure.Enum.SessionKey.LoggedInUserId.ToString(),
                UserId.ToString());

            DB.Models.Users User = _accountService.GetUser(UserId);
            if (User.IsVerified == false) return RedirectToAction("GenerateAccountVerificationCode");
            if (User.UserType == (short)Infrastructure.Enum.UserType.User)
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (User.ReadWelcomePopup == true) return RedirectToAction("UserDashboard", "Home");
                else { return RedirectToAction("UserWelcome", "Account"); }
            }
            else if (User.UserType == (short)Infrastructure.Enum.UserType.Partner)
            {
                ViewBag.Msg = "Not registered as a User";
                return View();
            }
            else if (User.UserType == (short)Infrastructure.Enum.UserType.Admin)
            {
                ViewBag.Msg = "Not registered as a User";
                return View();
            }
            else
            {
                ViewBag.Msg = "Unknown Usertype";
                return View();

            }
        }

        [HttpGet]
        public IActionResult GenerateAccountVerificationCode()
        {
            //problem userid shwoing in querystring if write redirecttoaction("GenerateAccountVerificationCode",Userid) with parameter to a get action method
            //solution of this is we generate no parameter method and this will redirect to a same name post action method
            return GenerateAccountVerificationCode( _contextAccessor.HttpContext.Session.GetString(Infrastructure.Enum.SessionKey.LoggedInUserId.ToString()));
        }

        [HttpPost]
        public IActionResult GenerateAccountVerificationCode(string UserId)
        {
            if ( string.IsNullOrEmpty( UserId ) ) return RedirectToAction("Login");
            DB.ViewModels.GenerateAccountVerificationCodeVM o = new DB.ViewModels.GenerateAccountVerificationCodeVM();
            o.UserId =new Guid( UserId);
            o.Code = Helpers.Utility.GenerateRandomCode();
            DB.Models.Users u = _accountService.GetUser(o.UserId );

            try {
                //send code by email to useremail
                Helpers.Utility.SendEmail(u.Email, "RendERA: Account Verification Code","Welcome to RendERA. <br/> You account verifycation Code is : <b>" + o.Code+ "</b>" );
            }
            catch (Exception ex) {
                //return RedirectToAction("Login");
            }
            _contextAccessor.HttpContext.Session.SetString(Infrastructure.Enum.SessionKey.RegisterationVerificationCode.ToString(), o.Code);
            return View(o);
        }

        [HttpPost]

        public IActionResult AccountCodeVerification(DB.ViewModels.GenerateAccountVerificationCodeVM o)
        {
            if (o == null || o.UserId == null || string.IsNullOrEmpty(o.Code)) return RedirectToAction("Register");

            if (o.Code.ToLower() == HttpContext.Session.GetString(Infrastructure.Enum.SessionKey.RegisterationVerificationCode.ToString()).ToLower()
                 && _accountService.RegistrationVeriy(o.UserId))
            {
                HttpContext.Session.Remove(Infrastructure.Enum.SessionKey.RegisterationVerificationCode.ToString());
                return View();
            }
            else
            {
                TempData["Errmsg"] = "You have entered invalid code, Please Try again";
                return RedirectToAction("GenerateAccountVerificationCode");
            }
        }
        public IActionResult Logout()
        {
            _contextAccessor.HttpContext.Session.Remove(RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            return RedirectToAction("Login");
        }
        public IActionResult Pricing()
        {
            return View();
        }

       
    }
}