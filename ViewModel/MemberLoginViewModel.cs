using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class MemberLoginViewModel : IValidatableObject
    {

        public int MemberId { get; set; }

        [Display(Name = "Nick name")]
        [Required(ErrorMessage = "Nick name is required")]
        [StringLength(50, ErrorMessage = "Nick name maximum character length is 50")]
        public string NickName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string NameSurname { get; set; }

        [Display(Name = "Remember me")]
        public bool IsRemember { get; set; }


        public string ReturnUrl { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
