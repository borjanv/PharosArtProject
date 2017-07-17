using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System;
using System.Web.Security;
using pharosArt.Models;
using System.Web;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using System.Linq;
using Umbraco.Web.Models;
using umbraco.IO;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Core.Models;

namespace pharosArt.Controllers
{
    public class ProfileController : SurfaceController
    {
        public ActionResult getProfile(string userName)
        {
            pharosArt.Models.ProfileMember profile = new pharosArt.Models.ProfileMember();
            List<Tuple<string, string>> mediaMember2 = new List<Tuple<string, string>>();
            var member = (Umbraco.Web.PublishedContentModels.Member)Members.GetByUsername(userName);
            profile.memberId = member.Id;
            profile.biography = member.Biography;  //UmbracoMemberComments
            profile.memberName = member.FirstName + " " + member.LastName;
            profile.profile.Username = userName;
            if (member.Picture != null)
            {
                profile.profile.urlImage = member.Picture.Url;
            }

            var mediaContent = member.MediaRoot;
            foreach (var mediaItem in mediaContent.Descendants<IPublishedContent>().Where(x => x.DocumentTypeAlias == Image.ModelTypeAlias
                || x.DocumentTypeAlias == "File").Where(x=>x.Parent.DocumentTypeAlias != ProfileFolder.ModelTypeAlias))
            {
                profile.mediaMember.Add(new Tuple<string, string>(mediaItem.DocumentTypeAlias, mediaItem.Url.ToString()));
            }
            
            FormsAuthentication.SetAuthCookie(userName, false);
            
            return View("~/Views/MacroPartials/Profile.cshtml", profile);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult editProfile(int id)
        {
            //var service = Services.MemberService;
            //var memberToSave1 = service.GetById(Int32.Parse(id.ToString()));
            var memberToSave = (Umbraco.Web.PublishedContentModels.Member)Members.GetById(Int32.Parse(id.ToString()));

            pharosArt.Models.ProfileMember membertoShow = new pharosArt.Models.ProfileMember();
            membertoShow.memberId = memberToSave.Id;
            membertoShow.memberName = memberToSave.FirstName; //Properties["firstName"].Value.ToString();
            membertoShow.memberLastName = memberToSave.LastName; //Properties["LastName"].Value.ToString();
            membertoShow.biography = memberToSave.Biography; //Properties["biography"].Value.ToString();
            //membertoShow.profile.urlImage = memberToSave.Properties["biography"].Value.ToString();
            return View("~/Views/Partials/EditProfile.cshtml", membertoShow);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult saveProfile(pharosArt.Models.ProfileMember member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var memberToSave = (Umbraco.Web.PublishedContentModels.Member)Members.GetById(member.memberId);
                    var service = Services.MemberService;
                    var memberToSave = service.GetById(Int32.Parse(member.memberId.ToString()));

                    memberToSave.SetValue("firstName", member.memberName);
                    memberToSave.SetValue("LastName", member.memberLastName);
                    memberToSave.SetValue("biography", member.biography);

                    //memberToSave.Properties["memberName"].Value = member.memberName;
                    //memberToSave.Properties["memberLastName"].Value = member.memberLastName;
                    //memberToSave.Properties["description"].Value = member.description;
                    service.Save(memberToSave);
                    TempData["message"] = "Profile was updated!";
                }
                catch (Exception e)
                {
                    e.ToString();
                    TempData["message"] = "Profile was not updated - contact admin";
                }
            }

            /*memberToSave.Picture = member.profile.urlImage; //member.profile.urlImage;
            memberToSave.FirstName = member.memberName;
            memberToSave.LastName = member.memberLastName;
             = member.description;
            member.memberLastName;
            member.memberLastName;*/
            /*var member = service.GetById(Int32.Parse(id));
            memberToSave.Biography = "database analyst";
            member.Properties["picture"].Value = ;
            newMember.SetValue("picture", );*/


            //if (ModelState.IsValid)
            //{
            //    ; ;
            //}
            
            return View("~/Views/MacroPartials/Profile.cshtml", member);
        }

        public int getFolderProfile()
        {
            int idProfileFoler = 0;
            Membership.GetNumberOfUsersOnline();
            var userLogin = Membership.GetUser().UserName;
            var service = Services.MemberService;
            var member = service.GetByUsername(userLogin);
            var mediaService = ApplicationContext.Current.Services.MediaService;
            var rootFolder = member.Properties["mediaRoot"].Value.ToString(); 
            var mediaFolder = Umbraco.Media(Int32.Parse(rootFolder));
            foreach (var mediaItem in mediaFolder.Children())
            {
                if (mediaItem.Name == "Profile")
                {
                    idProfileFoler = mediaItem.Id;
                    var mediaProfiler = Umbraco.Media(idProfileFoler);
                    break;
                }
            }

            return idProfileFoler;
        }

        [HttpPost]
        public async Task<JsonResult> UploadImage(string id)
        {
            try
            {
                var loged = Membership.GetUser().UserName;
                var service = Services.MemberService;
                var member = service.GetByUsername(loged);
                Membership.GetNumberOfUsersOnline();
                var userLogin = Membership.GetUser().UserName;
                var mediaService = ApplicationContext.Current.Services.MediaService;
                var list = mediaService.GetRootMedia();
                int idFolderImage = 0, folder = 0;

                idFolderImage = getFolderProfile();

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var categories_string = "profile";
                        var stream = fileContent.InputStream;
                        var name = fileContent.FileName;
                        if (fileContent.ContentType.Contains("image"))
                        {
                            folder = idFolderImage;
                        }
                        var ms = ApplicationContext.Current.Services.MediaService;
                        var MediaMap = Services.MediaService.CreateMedia(name, folder, "Image");
                        MediaMap.SetValue("umbracoFile", fileContent);
                        MediaMap.SetValue("category", categories_string);
                        Services.MediaService.Save(MediaMap);
                        var urlMedia = Umbraco.Media(MediaMap.Id).Url; //banner.image is the property id.
                        member.SetValue("picture", MediaMap.Id);
                        service.Save(member);
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload Failed");
            }

            return Json("File uploaded successfully");
        }
    }
}