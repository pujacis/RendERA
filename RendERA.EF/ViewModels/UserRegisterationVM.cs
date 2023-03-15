using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace RendERA.DB.ViewModels
{
    public class UserRegisterationVM
    {
        public int? UserCategory { get; set; }

        public short UserType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

    }

    public class LoginViewModel
    {

        [DisplayName("Username")]
        [Required(ErrorMessage = "Required.")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Required.")]
        public string Password { get; set; }

        [DisplayName("Remmember Me")]
        public bool RemmemberMe { get; set; }
    }
    public class GenerateAccountVerificationCodeVM
    {
        public Guid UserId { get; set; }
        public String Code { get; set; }
    }
}

