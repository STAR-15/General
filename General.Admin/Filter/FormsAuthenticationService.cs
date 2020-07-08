using General.Server.DataModel;
using General.Server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace General.Admin.Filter
{
    public class FormsAuthenticationService
    {
        public TimeSpan ExpirationTimeSpan { get; set; }

        public FormsAuthenticationService()
        {
            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        public UserTO GetAuthenticatedUser()
        {
            var httpContext = System.Web.HttpContext.Current;

            if (!(httpContext.User.Identity is FormsIdentity))
                return null;

            var formsIdentity = (FormsIdentity)httpContext.User.Identity;
            var userData = formsIdentity.Ticket.UserData ?? "";



            int userId;
            if (!int.TryParse(userData, out userId))
            {
                return null;
            }

            var userService = new UserRepository();

            return userService.GetUserById(userId);
        }

        public void SignIn(UserTO user, bool createPersistentCookie)
        {
            var now = DateTime.Now;

            var userData = user.UserId.ToString();

            var ticket = new FormsAuthenticationTicket(
                1,
                user.UserId.ToString(),
                now,
                now.Add(ExpirationTimeSpan),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath
            };

            var httpContext = System.Web.HttpContext.Current;

            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie)
            {
                cookie.Expires = ticket.Expiration;
            }

            httpContext.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {

            var httpContext = System.Web.HttpContext.Current;

            if (!(httpContext.User.Identity is FormsIdentity))
                return;

            FormsAuthentication.SignOut();

            var removeFormCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1)
            };

            httpContext.Response.Cookies.Add(removeFormCookie);
        }
    }
}