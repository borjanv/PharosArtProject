using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using pharosArt.Models;
using Umbraco.Web.Mvc;
using Facebook;

namespace pharosArt.Controllers
{
    public class FacebookLoginController : SurfaceController
    {
        [HttpPost]
        public ActionResult LogUserIn(string accessToken)
        {
            var service = Services.MemberService;
            //string accessToken = HttpContext.Current.Request["accessToken"];
            try
            {
                var client = new FacebookClient(accessToken);
                var parameters = new Dictionary<string, object>();
                parameters["fields"] = "id,name, email";

                dynamic result = client.Get("me", parameters);
                

                var id = (string)result["id"];
                var name = (string)result["name"];
                var email = (string)result["email"].ToString();
                

                MembershipCreateStatus status;

                var umbracoMember = Members.GetByEmail(email);               

                if (umbracoMember != null)
                {
                    // member exists so log them in
                    var auth = Membership.ValidateUser(email, id);
                    if (auth)
                    {
                        Members.Login(email, id);
                        return JavaScript("window.location = '/profile'");
                    }
                    
                }

                // member doesnt exist so we create them
                var regModel = Members.CreateRegistrationModel("Member");
                regModel.Email = email;
                regModel.Password = id;
                regModel.Username = email;
                regModel.UsernameIsEmail = true;
                regModel.Name = name;
                var newMember = Members.RegisterMember(regModel, out status);
                           
                service.AssignRole(email, "facebook");

                // todo create media folder for member and assign that to the member

                return JavaScript("window.location = '/profile'");
            }
            catch (Exception e)
            {
                var ex = e;
                return Content(e.Message + " " + e.StackTrace);
            }
        }
    }
}