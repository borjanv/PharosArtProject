using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System;
using System.Web.Security;
using pharosArt.Models;
using System.Web;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace pharosArt.Controllers
{
    public class LoginController : SurfaceController
    {
        public ActionResult RenderLogin()
        {
            return PartialView("~/Views/Partials/_Login.cshtml", new LoginModel());
        }
        public bool creteMember(string username)
        {
            bool result = true;
            var service = Services.MemberService;
            try
            {
                var newMember = service.CreateMember(username, "test@gmail.com", "test mauro", "Member");  //get from the form
                newMember.SetValue("NameMember", "Mauricio 2"); //get from the form
                newMember.SetValue("LastName", "Roldan");       //get from the form
                service.Save(newMember);
                var member = service.GetByUsername(username);
                Services.MemberService.AssignRole(member.Id, "media");

                var mediaService = ApplicationContext.Current.Services.MediaService;
                var profileFoler = mediaService.CreateMedia(username, Constants.System.Root, "Folder");
                mediaService.Save(profileFoler);
                var idProfileFolder = profileFoler.Id;

                var profileImages = mediaService.CreateMedia("Images", idProfileFolder, "Folder");
                mediaService.Save(profileImages);

                var profileMusic = mediaService.CreateMedia("Music", idProfileFolder, "Folder");
                mediaService.Save(profileMusic);

                var profileProfile = mediaService.CreateMedia("Profile", idProfileFolder, "Folder");
                mediaService.Save(profileProfile);

                member.SetValue("mediaRoot", profileFoler.Id.ToString());
                service.Save(member);
            }
            catch(Exception e)
            {
                result = false;
                e.ToString();
            }            

            return result;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var service = Services.MemberService;
                if (Membership.ValidateUser(model.Username, model.Password))
                {
                    /** CREATE MEMBERS - GET USR BY NAME **/                    
                    service.FindMembersInRole("media", model.Username);
                    service.GetByUsername(model.Username);
                    if (!service.Exists(model.Username))
                    {
                        creteMember(model.Username);
                    }
                    else
                    {
                        return RedirectToAction("getProfile","Profile",  new { userName = model.Username});
                    }
                    /************************/

                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);
                    
                    if (myHelper.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("/home/");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The username or password provided is incorrect.");
                }
            }
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