namespace ViewModel
{
    public class RegistrationLoginViewModel
    {
        public MemberRegisterViewModel RegistrationViewModel { get; set; }
        public MemberLoginViewModel LoginViewModel { get; set; }

        public string Error { get; set; }

        public string Url { get; set; }
    }
}
