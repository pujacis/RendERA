using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace RendERA.ServiceManager.IServices
{
    public interface IAccountSrv
    {

        IQueryable<DB.ViewModels.EmailToUser> GetAllUser();
        Guid RegisterUser( DB.ViewModels.UserRegisterationVM  userVM);
        bool UpdateUserProfile(DB.ViewModels.UserRegisterationVM userVM);
        IEnumerable<DB.ViewModels.EmailToUser> GetAllUserByUserType(int userType);
        Guid Login(DB.ViewModels.LoginViewModel LoginVM);
        bool RegistrationVeriy(Guid UserId);
        void ForgotPassword();
        void ResetPassword();
        DB.Models.Users GetUser(Guid UserId);
        DB.Models.Users GetUserByEmail(string email);
        bool ErrorLog(Exception ex);
        bool ChooseDefaultRendingSoftware(Guid UserId, int[] ChooseRenderSoftwaresArray);
        string GetAdminEmail();

    }
}
