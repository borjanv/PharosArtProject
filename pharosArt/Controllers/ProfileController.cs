using pharosArt.Models;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;
using Member = Umbraco.Web.PublishedContentModels.Member;

namespace pharosArt.Controllers
{
    public class ProfileController : SurfaceController
    {
        [HttpGet]
        public ActionResult GetProfile(string userName)
        {
            var profile = new EditModel();
            var member = new Member(Members.GetByUsername(userName));
            profile.MemberId = member.Id;
            profile.Biography = member.Biography;
            profile.FirstName = member.FirstName;
            profile.LastName = member.LastName;
            profile.UserName = userName;
            profile.RootMedia = new ProfileFolder(member.MediaRoot);
            profile.ProfileImage = member.HasValue("picture") ? new Image(member.Picture) : new Image(Umbraco.TypedMedia(6927));

            return View("~/Views/Partials/ShowProfilePartial.cshtml", profile);
        }

        public ActionResult EditProfile(int id)
        {
            var member = new Member(Members.GetById(id));

            var model = new EditModel
            {
                MemberId = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Biography = member.Biography,
                RootMedia = new ProfileFolder(member.MediaRoot),
                ProfileImage = member.HasValue("picture") ? new Image(member.Picture) : new Image(Umbraco.TypedMedia(6927))
            };

            return View("~/Views/Partials/EditProfile.cshtml", model);
        }

        [HttpPost]
        public ActionResult SaveProfile(EditModel member)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            try
            {
                var service = Services.MemberService;
                var memberToSave = service.GetById(member.MemberId);

                memberToSave.SetValue("firstName", member.FirstName);
                memberToSave.SetValue("lastName", member.LastName);
                memberToSave.SetValue("biography", member.Biography);

                service.Save(memberToSave);
                TempData["message"] = "Profile was updated!";
            }
            catch (Exception e)
            {
                TempData["message"] = e.Message;
            }

            return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
        }
    }
}