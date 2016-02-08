using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class MemberRegisterViewModel
    {
        
        [Required(ErrorMessage = "Nick name is required")]
        [Display(Name = "Nick Name")]
        [RegularExpression(@"^(?=.{3,50}$)([A-Za-z0-9][._\[\]]?)*$", ErrorMessage= "Invalid must be between 3-50 characters")]
        public string NickName { get; set; }
      
        [Required(ErrorMessage = "Password id required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password repeat is required")]
        //[Compare("Password", ErrorMessageResourceType = typeof(LangResource), ErrorMessage = "passwordsAreNotMatch")]
        [Display(Name = "Password repeat")]
        [DataType(DataType.Password)]
        public string PasswordRepeat { get; set; } 


        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; } 
    }
}
