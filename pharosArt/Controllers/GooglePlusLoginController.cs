using pharosArt.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class GooglePlusLoginController:SurfaceController,IRegister
    {
        [HttpPost]
        public ActionResult LogUserIn(GooglePlusObject response)
        {
            try
            {
                var umbracoMember = Members.GetByEmail(response.Email);  // check if member with the same email already exists
                if (umbracoMember != null)
                {
                    // member exists so log them in
                    var auth = Membership.ValidateUser(Services.MemberService.GetByEmail(response.Email).Username, Services.MemberService.GetByEmail(response.Email).RawPasswordValue);
                    if (auth)
                    {
                        Members.Login(Services.MemberService.GetByEmail(response.Email).Username, Services.MemberService.GetByEmail(response.Email).RawPasswordValue);
                        return JavaScript("window.location = '/profile'");
                    }
                }
                // member doesnt exist so we create them
                var model = new GooglePlusMember
                {
                    Email = response.Email,
                    Password = response.GoogleId,
                    FirstName = response.FirstName,
                    Id = response.GoogleId,
                    LastName = response.LastName
                };

                MembershipCreateStatus status = RegisterMember(model);
                if (status != MembershipCreateStatus.Success)
                    return CurrentUmbracoPage();

                return JavaScript("window.location = '/profile'");                
            }
            catch (Exception e)
            {
                var ex = e;
                return Content(e.Message + " " + e.StackTrace);
            }
        }

        public MembershipCreateStatus RegisterMember(IUmbracoMember member)
        {
            MembershipCreateStatus status;
            var regModel = Members.CreateRegistrationModel("Member");
            regModel.Email = member.Email;
            regModel.Password = member.Password;
            regModel.Username = member.Id; // set username to facebookId NOT as email
            regModel.UsernameIsEmail = false;
            regModel.Name = member.FirstName + " " + member.LastName;
            regModel.MemberProperties.Add(new UmbracoProperty { Alias = "firstName", Name = "First name", Value = member.FirstName });
            regModel.MemberProperties.Add(new UmbracoProperty { Alias = "lastName", Name = "Last name", Value = member.LastName });
            regModel.MemberProperties.Add(new UmbracoProperty { Alias = "googlePlusId", Name = "Google Plus Id", Value = member.Id });
            Members.RegisterMember(regModel, out status);

            if (status == MembershipCreateStatus.Success)
            {
                Services.MemberService.AssignRole(member.Id, "googlePlus"); //assign facebook group

                var dbMember = Services.MemberService.GetByUsername(member.Id);
                member.UmbracoId = dbMember.Id;
                dbMember.SetValue("mediaRoot", CreateParentMediaFolderForMember(member).ToString());
                Services.MemberService.Save(dbMember);
            }
            return status;
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
            Services.MediaService.CreateMediaWithIdentity("Videos", newMediaFolder,
              VideosFolder.ModelTypeAlias);

            return newMediaFolder.GetUdi();
        }
    }
}