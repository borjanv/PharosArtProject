using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using pharosArt.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
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
            if (Members.GetByEmail(model.Member.Email) != null)
            {
                var s = Members.GetByEmail(model.Member.Email).ToString();
                //member exists so check if they logged in with facebook in the past
                if (!Roles.IsUserInRole(Services.MemberService.GetByEmail(model.Member.Email).Username,"facebook"))
                {
                    // they havent logged in with facebook in the past so they already exist as regular member
                    ModelState.AddModelError("Email", "A member with this email address already exists");
                    return CurrentUmbracoPage();
                }
                //have logged in with facebook so just update member with their information
                UpdateMember(model.Member, Members.GetByEmail(model.Member.Email).Id);

                // log member in
                Members.Login(model.Member.Email, model.Member.Password);

                //redirect member to profile page
                return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
            }

            // member doesnt exist so proceed with registration
            var registerMember = RegisterMember(model.Member);
            if (registerMember != MembershipCreateStatus.Success)
            {
                ViewBag.RegistrationMessage = registerMember.ToString();
                return CurrentUmbracoPage();
            }

            // todo maybe replace this with a global login function or redirect to login action
            Members.Login(model.Member.Email, model.Member.Password);

            return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
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
                Roles.AddUserToRole(member.Email, "regular");

                var dbMember = Services.MemberService.GetByUsername(member.Email);
                member.UmbracoId = dbMember.Id;
                dbMember.SetValue("mediaRoot", CreateParentMediaFolderForMember(member).ToString());
                Services.MemberService.Save(dbMember);
            }
            return status;
        }

        public void UpdateMember(IUmbracoMember model, int memberId)
        {
            var member = Services.MemberService.GetById(memberId);

            Roles.AddUserToRole(member.Username, "regular");
            Roles.RemoveUserFromRole(member.Username, "facebook");

            if (!string.IsNullOrEmpty(model.FirstName))
                member.SetValue("firstName", model.FirstName);
            if (!string.IsNullOrEmpty(model.LastName))
                member.SetValue("lastName", model.LastName);
            if (!string.IsNullOrEmpty(model.Email))
                member.Username = model.Email;

            // add user to regular group and not just facebook            
            Services.MemberService.Save(member);

            Services.MemberService.SavePassword(member, model.Password);
        }

        public GuidUdi CreateParentMediaFolderForMember(IUmbracoMember member)
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

            return newMediaFolder.GetUdi();
        }
    }

}