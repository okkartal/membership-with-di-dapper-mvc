using System.Web;
using Business.Interface;
using Infrastructure.Constraints.Constant;
using Infrastructure.Security;
using ViewModel;
using SWM = System.Web.Mvc;
namespace WEB.Attributes
{
    public class SessionManagementAttribute : SWM.ActionFilterAttribute
    {
        public override void OnActionExecuting(SWM.ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session[SessionKeys.MemberInfo] == null)
            {
                IMemberBusiness memberBusiness = (IMemberBusiness)SWM.DependencyResolver.Current.GetService(typeof(IMemberBusiness));

                var httpCookie = HttpContext.Current.Request.Cookies[SessionKeys.CookiePrefix];

                if (httpCookie != null && httpCookie.Values[SessionKeys.Cookie_MemberId] != null)
                {
                    string value = httpCookie.Values[SessionKeys.Cookie_MemberId];
                    CrpytorEngine crp = new CrpytorEngine() { SecurityKey = SessionKeys.Cookie_MemberId };

                    var memberId = int.Parse(crp.Decrypt(value, true));

                    var resultSet = memberBusiness.GetMemberByMemberId(memberId);
                    if (resultSet.Success)
                    {
                        HttpContext.Current.Session[SessionKeys.MemberInfo] = new SessionUser()
                        {
                            Id = resultSet.Object.Id,
                            NickName = resultSet.Object.NickName,
                            Name = resultSet.Object.Name,
                            SurName = resultSet.Object.Surname
                        };
                    }
                }
            }
            if (HttpContext.Current.Session[SessionKeys.MemberInfo] == null)
                filterContext.Result = new SWM.RedirectResult(string.Format("/{0}/{1}",
                     RouteKeys.MemberController, "Index"), false);
            base.OnActionExecuting(filterContext);
        }

    }
}