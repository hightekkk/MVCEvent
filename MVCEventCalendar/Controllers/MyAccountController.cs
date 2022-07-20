using MVCEventCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCEventCalendar.Controllers
{
    public class MyAccountController : Controller
    {
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnUrl = "")
        {
            #region Existing Code
            //using (MyDatabaseEntities dc = new MyDatabaseEntities())
            //{
            //    var user = dc.Users.Where(a => a.Username.Equals(l.Username) && a.Password.Equals(l.Password)).FirstOrDefault();
            //    if (user != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(user.Username, l.RememberMe);
            //        if (Url.IsLocalUrl(ReturnUrl))
            //        {
            //            return Redirect(ReturnUrl);
            //        }
            //        else
            //        {
            //            return RedirectToAction("MyProfile", "Auth");
            //        }
            //    }
            //}
            #endregion
            if (ModelState.IsValid)
            {
                var isValidUser = Membership.ValidateUser(l.Username, l.Password);
                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("IndexA", "Auth");
                    }
                }
            }

            ModelState.Remove("Password");
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("IndexA", "Auth");
        }
    }
}