using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace RendERA.ServiceManager.Services
{
    public class AccountSrv : IServices.IAccountSrv
    {
        private readonly RendERA.Infrastructure.IRepositories.IUnitOfWork _unitOfWork;
        public AccountSrv(Infrastructure.IRepositories.IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public IQueryable<DB.ViewModels.EmailToUser> GetAllUser()
        {
            var query = from u in _unitOfWork.IUsersRepo.Table.Where(a=> a.UserId != null)
                        select new DB.ViewModels.EmailToUser
                        {
                            user = u.UserName,
                            UserId = u.UserId
                        };
            return query;
        }

        public IEnumerable<DB.ViewModels.EmailToUser> GetAllUserByUserType(int userType)
        {
            var query = from u in _unitOfWork.IUsersRepo.Table.Where(a => a.UserType == userType)
                        select new DB.ViewModels.EmailToUser
                        {
                            user = u.UserName,
                            UserId = u.UserId
                        };
            return query.ToList();
        }
        public bool ErrorLog(Exception e)
        {
            try
            {
                ////_unitOfWork.IUsersRepo.Get()
                RendERA.DB.Models.ErrorLogs error = new DB.Models.ErrorLogs();
                //tblErrorLog error = new tblErrorLog();
                error.InnerException = Convert.ToString(e.InnerException);
                error.Message = Convert.ToString(e.Message);
                error.HelpLink = Convert.ToString(e.HelpLink);
                error.Source = Convert.ToString(e.Source);
                error.StackTrace = Convert.ToString(e.StackTrace);
                error.CreatedOn = DateTime.UtcNow;
                _unitOfWork.IErrorLogsRepo.Add(error);
                _unitOfWork.IErrorLogsRepo.Save();
                
                return true;
            }
            catch (Exception exx)
            { throw exx; }
            throw new NotImplementedException();
        }
        public Guid RegisterUser(DB.ViewModels.UserRegisterationVM UserVm)
        {
            int DuplicateUserCount = _unitOfWork.IUsersRepo.GetAll().Where(o => o.UserName.Trim() == UserVm.UserName.Trim() || o.Email.Trim() == UserVm.Email.Trim()).Count();
            if (DuplicateUserCount > 0) { throw new Exception("Username or Email already exists"); }

            DB.Models.Users User = new DB.Models.Users()
            {
                Email = UserVm.Email.Trim(),
                FirstName = UserVm.FirstName,
                LastName = UserVm.LastName,
                IsVerified = false,
                Password = UserVm.Password,
                Phone = UserVm.Phone,
                UserId = Guid.NewGuid(),
                UserName = UserVm.UserName.Trim(),
                UserType = UserVm.UserType,
                UserCategory = UserVm.UserCategory
            };

            _unitOfWork.IUsersRepo.Add(User);
            _unitOfWork.IUsersRepo.Save();
            return User.UserId;

        }
        public bool UpdateUserProfile(DB.ViewModels.UserRegisterationVM userVM) { return false; }

        public Guid Login(DB.ViewModels.LoginViewModel LoginVM)
        {
            Guid UserId = (from a in _unitOfWork.IUsersRepo.GetAll()
                           where (a.Email == LoginVM.UserName || a.UserName == LoginVM.UserName) && a.Password == LoginVM.Password
                           select a.UserId).SingleOrDefault();

            return UserId;
        }
        public DB.Models.Users GetUser(Guid UserId)
        {
            DB.Models.Users user = (from a in _unitOfWork.IUsersRepo.GetAll()
                                    where a.UserId == UserId
                                    select new DB.Models.Users()
                                    {
                                        Email = a.Email,
                                        FirstName = a.FirstName,
                                        IsVerified = a.IsVerified,
                                        LastName = a.LastName,
                                        Phone = a.Phone,
                                        UserId = a.UserId,
                                        UserName = a.UserName,
                                        UserType = a.UserType,
                                        ReadWelcomePopup=a.ReadWelcomePopup,
                                        UserCategory = a.UserCategory
                                    }).SingleOrDefault();
            return user;
        }
        public DB.Models.Users GetUserByEmail(string email)
        {
            DB.Models.Users user = (from a in _unitOfWork.IUsersRepo.GetAll()
                                    where a.Email == email
                                    select new DB.Models.Users()
                                    {
                                        Email = a.Email,
                                        FirstName = a.FirstName,
                                        IsVerified = a.IsVerified,
                                        LastName = a.LastName,
                                        Phone = a.Phone,
                                        UserId = a.UserId,
                                        UserName = a.UserName,
                                        UserType = a.UserType,
                                        ReadWelcomePopup=a.ReadWelcomePopup,
                                        UserCategory = a.UserCategory,
                                        Password =a.Password
                                    }).SingleOrDefault();
            return user;
        }

        public void ForgotPassword() { throw new NotImplementedException(); }
        public void ResetPassword() { throw new NotImplementedException(); }

        public bool RegistrationVeriy(Guid Userid)
        {
            try
            {
                DB.Models.Users user = _unitOfWork.IUsersRepo.GetAll().Where(o => o.UserId == Userid).Single();

                user.IsVerified = true;
                _unitOfWork.IUsersRepo.Update(user);
                _unitOfWork.IUsersRepo.Save();
                return true;
            }
            catch { return false; }
        }

        public bool ChooseDefaultRendingSoftware( Guid UserId,  int[] ChooseRenderSoftwaresArray)
        {
            try
            {
                DB.Models.Users user = _unitOfWork.IUsersRepo.GetAll().Where(o => o.UserId == UserId).Single();

                //remvoe old default software for this user
                List<DB.Models.DefaultRenderSoftwares> OldSelectedSoftware = _unitOfWork.IDefaultRenderSoftwaresRepo.GetAll().Where(o => o.UserId == UserId).DefaultIfEmpty().ToList();
                if(OldSelectedSoftware == null)
                _unitOfWork.IDefaultRenderSoftwaresRepo.RemoveRange(OldSelectedSoftware.ToArray());
                
                //add new entry
                List<DB.Models.DefaultRenderSoftwares> NewSelectedSoftwares = new List<DB.Models.DefaultRenderSoftwares>();
                foreach (var item in ChooseRenderSoftwaresArray)
                {
                    DB.Models.DefaultRenderSoftwares s = new DB.Models.DefaultRenderSoftwares();
                    s.Id = Guid.NewGuid();
                    s.SoftwareEnumId = item;
                    s.UserId =  UserId;
                    NewSelectedSoftwares.Add(s);
                }

                _unitOfWork.IDefaultRenderSoftwaresRepo.AddRange(NewSelectedSoftwares.ToArray()); 
                user.ReadWelcomePopup = true;
                //_unitOfWork.IUsersRepo.Update(user);
                //_unitOfWork.IUsersRepo.Save();
                _unitOfWork.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public string GetAdminEmail()
        {
            return _unitOfWork.IUsersRepo.Table.Where(a => a.UserType == (int)RendERA.Infrastructure.Enum.UserType.Admin).FirstOrDefault().Email;
        }
    }
}
