using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class MemberDetailViewModel
    {


        [Editable(false)]
        public int MemberId { get; set; }



        [Display(Name = "Nick name")]
        public string NickName { get; set; }
        #region Password Operations
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Display(Name = "New password")]
        public string NewPassword { get; set; }


        [Display(Name = "Password repeat")]
        public string NewPasswordMatch { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        #endregion


        #region Personal Informations
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Sur name")]
        public string Surname { get; set; }

       
        #endregion
         
        [Display(Name = "Forgot Password Email")]
        public string Email { get; set; }


        public string Error { get; set; }
 
    }
}
