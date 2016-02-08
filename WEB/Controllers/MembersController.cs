using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business.Interface;
using Entity;
using Infrastructure.Constraints.Constant;
using Infrastructure.ServiceResult;
using ViewModel;
using WEB.Attributes;

namespace WEB.Controllers
{
    public class MembersController : BaseController
    {
        private readonly IMemberBusiness _memberBusiness;

        public MembersController(IMemberBusiness memberBusiness)
        {
            _memberBusiness = memberBusiness;
        }

        public ActionResult Index()
        {


            if (Session[SessionKeys.MemberInfo] != null)
                return RedirectToAction(RouteKeys.MemberProfile);

            var returnModel = new RegistrationLoginViewModel()
            {
                LoginViewModel = new MemberLoginViewModel(),
                RegistrationViewModel = new MemberRegisterViewModel()
            };

            ViewBag.PageTitle = "Membership";

            return  View(RouteKeys.Register, returnModel);
        }

        public ActionResult Logout()
        {
            var httpCookie = Request.Cookies[SessionKeys.Cookie_MemberId];

            if (httpCookie != null)
            {
                httpCookie.Expires = DateTime.Now.AddDays(-1);
                //httpCookie.Domain = string.Concat(".", Configuration.FanatikDomain);
                httpCookie.Path = "/";
                Response.Cookies.Add(httpCookie);
                Response.Cookies.Clear();
            }
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return Redirect(string.Format("/{0}/Index", RouteKeys.MemberController));
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return  View(RouteKeys.LoginPartial);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult Login(RegistrationLoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var memberLogin = new MemberLogin
                {
                    NickName = model.LoginViewModel.NickName,
                    Password = model.LoginViewModel.Password
                };
                var member = _memberBusiness.MemberLogin(memberLogin);

                if (member.Success)
                {
                    Response.CacheControl = "no-cache";
                    Response.AddHeader("Pragma", "no-cache");
                    Response.Expires = -1;

                    DoLogin(member.Object, model.LoginViewModel.IsRemember);


                    model.Url = string.Format("/{0}/{1}?login=true",
                        RouteKeys.MemberController, RouteKeys.MemberProfile);
                        return Json(model); 
                    
                }
                else
                {
                    ModelState.AddModelError("error", member.Message);
                }
            }
           
            var returnModel = new RegistrationLoginViewModel()
            {
                LoginViewModel = model.LoginViewModel,
                RegistrationViewModel = new MemberRegisterViewModel(),
                Error = Errors(GetModelErrors())
            };
            return Json(returnModel);
        }

        public ActionResult Register()
        {
            return  View(RouteKeys.RegisterPartial, new MemberRegisterViewModel());
        }

        [AllowAnonymous, HttpPost] 
        public ActionResult Register(MemberRegisterViewModel member, string returnUrl = "")
        {
            var returnModel = new RegistrationLoginViewModel()
            {
                LoginViewModel = new MemberLoginViewModel(),
                RegistrationViewModel = member,
                Error = Errors(GetModelErrors())
            };
            if (ModelState.IsValid)
            {

                ResultSet<Member> resultMember = _memberBusiness.AddMember(member);

                if (!resultMember.Success)
                {
                    ModelState.AddModelError("Error", resultMember.Message);
                }
            }



            returnModel.Error = Errors(GetModelErrors()); 
            return Json(returnModel);
        }

        public ActionResult RegistrationResult()
        {
            return View();
        }

        #region Activation Operations

        #endregion

 
        #region Profile Operations

        [SessionManagement]
        public ActionResult MemberProfile()
        {


            ViewBag.PageTitle = "Profile";
            var result = _memberBusiness.GetMemberDetails(GetSessionUser.Id);
            if (result.Success)
            {
                if (Request["login"] != null)
                    result = _memberBusiness.GetMemberDetails(GetSessionUser.Id);
             


                return View(result.Object);
            }

            return View();
        }


        [HttpPost, SessionManagement]
        public ActionResult MemberProfile(MemberDetailViewModel memberDetailViewModel)
        {
           
            
            memberDetailViewModel.MemberId = GetSessionUser.Id;

            var result = _memberBusiness.UpdateMemberDetail(memberDetailViewModel);

            if (!result.Success)
            {
                 
                ModelState.AddModelError("errorMemberProfile", result.Message);
                memberDetailViewModel.Error = Errors(GetModelErrors());


            }

            return Json(memberDetailViewModel);

        }
        #endregion


    }
}