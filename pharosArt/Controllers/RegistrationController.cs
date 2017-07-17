using System.Web.Mvc;
using System.Web.Security;
using pharosArt.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class RegistrationController : SurfaceController, IRegister
    {
        [ChildActionOnly]
        public ActionResult GetRegistration()
        {
            // return an empty registration form
            return View("RegistrationPartial", new RegistrationModel());
        }

        [HttpPost]
        public ActionResult PostRegistration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            // 
            if (Members.GetByEmail(model.Member.Email) == null)
            {
                ModelState.AddModelError("Email", "A member with this email address already exists");
                return CurrentUmbracoPage();
            }

            var registerMember = RegisterMember(model.Member);
            if (registerMember != MembershipCreateStatus.Success)
            {
                ViewBag.RegistrationMessage = registerMember.ToString();
                return CurrentUmbracoPage();
            }

            // todo maybe replace this with a global login function or redirect to login action
            Members.Login(model.Member.Email, model.Member.Password);

            var redirectPage = Umbraco.TypedContent(AppHelper.GetHomeNode().ProfilePage);

            return RedirectToUmbracoPage(redirectPage.Id);
        }


        public MembershipCreateStatus RegisterMember(IUmbracoMember member)
        {
            MembershipCreateStatus status;
            var regModel = Members.CreateRegistrationModel("Member");
            regModel.Email = member.Email;
            regModel.Password = member.Password;
            regModel.Username = member.Email;
            regModel.UsernameIsEmail = true;
            regModel.Name = member.FirstName + " " + member.LastName;
            regModel.MemberProperties.Add(new UmbracoProperty { Alias = "genre", Name = "Genre", Value = member.Genre });
            regModel.MemberProperties.Add(new UmbracoProperty { Alias = "firstName", Name = "First name", Value = member.FirstName });
            regModel.MemberProperties.Add(new UmbracoProperty { Alias = "lastName", Name = "Last name", Value = member.LastName });
            Members.RegisterMember(regModel, out status);

            if (status == MembershipCreateStatus.Success)
            {
                Services.MemberService.AssignRole(member.Email, "media");
                Services.MemberService.AssignRole(member.Email, "regular");

                var dbMember = Services.MemberService.GetByUsername(member.Email);
                member.UmbracoId = dbMember.Id;
                dbMember.SetValue("mediaRoot", CreateParentMediaFolderForMember(member).ToString());
            }
            return status;
        }

        /// <summary>
        /// Creates the folder structure in the media section for the member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Id of main member folder in media.</returns>
        public int CreateParentMediaFolderForMember(IUmbracoMember member)
        {
            var newMediaFolder = Services.MediaService.CreateMediaWithIdentity(member.FirstName + " " + member.LastName, 5830, ParentFolder.ModelTypeAlias);
            newMediaFolder.SetValue("member", member.UmbracoId);
            Services.MediaService.Save(newMediaFolder);

            Services.MediaService.CreateMediaWithIdentity("Music", newMediaFolder,
                MusicFolder.ModelTypeAlias);
            Services.MediaService.CreateMediaWithIdentity("Profile", newMediaFolder,
                ProfileFolder.ModelTypeAlias);
            Services.MediaService.CreateMediaWithIdentity("Images", newMediaFolder,
                ImagesFolder.ModelTypeAlias);

            return newMediaFolder.Id;
        }
    }

}