using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using pharosArt.Models;
using Umbraco.Web.Mvc;
using Facebook;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class FacebookLoginController : SurfaceController, IRegister
    {
        [HttpPost]
        public ActionResult LogUserIn(string accessToken)
        {
            try
            {
                var client = new FacebookClient(accessToken);
                var parameters = new Dictionary<string, object>();
                parameters["fields"] = "id,name, email";

                dynamic result = client.Get("me", parameters);


                var id = (string)result["id"];
                var name = (string)result["name"];
                var email = (string)result["email"].ToString();

                var umbracoMember = Members.GetByEmail(email);  // check if member with the same email already exists

                if (umbracoMember != null)
                {
                    // member exists so log them in
                    var auth = Membership.ValidateUser(email, Services.MemberService.GetByEmail(email).RawPasswordValue);
                    if (auth)
                    {
                        Members.Login(email, Services.MemberService.GetByEmail(email).RawPasswordValue);
                        return JavaScript("window.location = '/profile'");
                    }
                }

                // member doesnt exist so we create them
                var model = new FacebookMember
                {
                    Email = email,
                    Password = id,
                    FirstName = name,
                    Id = id
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
            Members.RegisterMember(regModel, out status);

            if (status == MembershipCreateStatus.Success)
            {
                Services.MemberService.AssignRole(member.Email, "media");
                Services.MemberService.AssignRole(member.Email, "facebook"); //assign facebook group

                var dbMember = Services.MemberService.GetByUsername(member.Id);
                member.UmbracoId = dbMember.Id;
                dbMember.SetValue("mediaRoot", CreateParentMediaFolderForMember(member).ToString());
            }
            return status;
        }

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