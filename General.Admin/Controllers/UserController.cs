using General.Admin.Filter;
using General.Admin.Models;
using General.Server;
using General.Server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace General.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userService = new UserRepository();
                var user = userService.GetUser(model.UserName);
                if (user != null)
                {
                    if (TextHelper.Sha256(model.Password) == user.Password)
                    {
                        var formsAuthenticationServic = new FormsAuthenticationService();
                        formsAuthenticationServic.SignIn(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Message", "密码错误");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("Message", "用户名不存在");
                }
            }

            ModelState.AddModelError("Message", "用户名或密码错误");
            return View();
        }

        public ActionResult Logout()
        {
            var formsAuthenticationServic = new FormsAuthenticationService();
            formsAuthenticationServic.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}
