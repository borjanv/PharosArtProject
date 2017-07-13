using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using pharosArt.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace pharosArt.Controllers
{
    public class RegistrationController : SurfaceController,IRegister
    {
        [HttpGet]
        public ActionResult GetRegistration()
        {
            // return an empty registration form
            return PartialView("RegistrationPartial", new RegistrationModel());
        }

        [HttpPost]
        public ActionResult PostRegistration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var registerMember = RegisterMember(model.Member);

            if (registerMember != MembershipCreateStatus.Success)
            {
                ViewBag["regStatus"] = registerMember.ToString();
                return CurrentUmbracoPage();
            }

            //todo redirect to congrads page
            //return RedirectToUmbracoPage(1234);
            throw new NotImplementedException();
        }


        public MembershipCreateStatus RegisterMember(RegisterModel member)
        {
            MembershipCreateStatus status;
            var regModel = Members.CreateRegistrationModel("Member");
            regModel.Email = member.Email;
            regModel.Password = member.Password;
            regModel.Username = member.Username;
            regModel.UsernameIsEmail = true;
            regModel.Name = member.Name;
            var newMember = Members.RegisterMember(regModel, out status);

            if (status == MembershipCreateStatus.Success)
                Services.MemberService.AssignRole(member.Email, "facebook");

            return status;
        }
    }

}