using System.Web.Mvc;
using System.Web.Security;
using pharosArt.Models;

namespace pharosArt.Controllers
{
    public class LoginController : SurfaceController
    {
        public ActionResult RenderLogin()
        {
            return PartialView("~/Views/Partials/_Login.cshtml", new LoginModel());
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult SubmitLogin(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            if (Membership.ValidateUser(model.Username, model.Password))
            {
                Members.Login(model.Username, model.Password);
                FormsAuthentication.SetAuthCookie(model.Username, false);

                return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
            }

            ModelState.AddModelError("", "The username or password provided is incorrect.");
            return CurrentUmbracoPage();
        }

        public ActionResult RenderLogout()
        {
            return PartialView("~/Views/MacroPartials/Profile.cshtml");
        }

        public ActionResult SubmitLogout()
        {
            TempData.Clear();
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}