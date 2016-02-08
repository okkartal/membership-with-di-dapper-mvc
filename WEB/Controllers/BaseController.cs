using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Constraints.Constant;
using Infrastructure.Security;
using ViewModel;
using ENT = Entity;
namespace WEB.Controllers
{
    public abstract class BaseController : Controller
    {

        protected ModelErrorCollection GetModelErrors()
        {
            ModelErrorCollection modelError = new ModelErrorCollection();
            foreach (ModelError error in ViewData.ModelState.Values.SelectMany(entry => entry.Errors))
            {
                modelError.Add(error);
            }
            return modelError;
        }

        protected string Errors(ModelErrorCollection modelError)
        {
            if (modelError == null) return (string.Empty);
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            foreach (var tag  in modelError)
            {
                sb.Append(tag.ErrorMessage+",");
            }
            return (sb.ToString().TrimEnd((',')));
        }

      

        protected void DoLogin(ENT.Member member, bool chkIsRemember)
        {

            Session[SessionKeys.MemberInfo] = new SessionUser()
            {
                Id = member.Id,
                NickName = member.NickName,
                Name = member.Name,
                SurName = member.Surname,
                Email = member.Email
            };

            CrpytorEngine crp = new CrpytorEngine() { SecurityKey = SessionKeys.Cookie_MemberId };

            var httpCookie = new HttpCookie(SessionKeys.CookiePrefix);
            httpCookie.Values.Add(new NameValueCollection
                        {
                            {SessionKeys.Cookie_MemberId,crp.Encrypt(member.Id.ToString(),true).ToString(CultureInfo.InvariantCulture)}

                        });
            httpCookie.Path = "/";
            httpCookie.Secure = false;

            httpCookie.Expires = chkIsRemember ? DateTime.Now.AddDays(7) : DateTime.MinValue;
            Response.SetCookie(httpCookie);
        }

        protected SessionUser GetSessionUser
        {
            get
            {
                return ((SessionUser)Session[SessionKeys.MemberInfo]);
            }
        }
    }
}