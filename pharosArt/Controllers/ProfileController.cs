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

using System.Linq;
using Umbraco.Web;
using umbraco.IO;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;

namespace pharosArt.Controllers
{
    public class ProfileController : SurfaceController
    {
        public ActionResult RenderLogin(string userName)
        {
            ProfileMember profile = new ProfileMember();
            var service = Services.MemberService;
            var member = service.GetByUsername(userName);
            profile.memberId = member.Id;
            profile.description = member.Comments;
            profile.memberName = member.Email;
            profile.profile.Username = userName;
            if(member.Properties["picture"].Value != null)
                profile.profile.urlImage = member.Properties["picture"].Value.ToString();
            //profile.profile.urlImage = member.Properties["picture"].Value.ToString();
            //profile.profile.urlImage = member.ima;
            //profile.profile.Username = member.LastLoginDate;
            //profile.profile. = member.Name;
            profile.memberName = member.Properties["NameMember"].Value.ToString();
            profile.memberLastName = member.Properties["LastName"].Value.ToString();
            FormsAuthentication.SetAuthCookie(userName, false);
            //return View("");
            //return Redirect("/Profile/");
            //return RedirectToAction("RenderLogout", profile);
            //return PartialView("_Logout", profile);

            //return Redirect("/profile/");
            //return PartialView("_Logout");
            return View("~/Views/MacroPartials/_Logout.cshtml", profile);
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult editProfile(string id, string userName)
        {
            //ProfileMember profile = new ProfileMember();
            var service = Services.MemberService;
            var member = service.GetById(Int32.Parse(id));
            member.Comments = "database analyst";
            //member.Properties["picture"].Value = ;
            //newMember.SetValue("picture", );
            service.Save(member);

            if (ModelState.IsValid)
            {
                ; ;
            }
            return CurrentUmbracoPage();
        }

        public int getFolderProfile()
        {
            /** upload media to the profile **/
            int idImageFoler = 0;
            Membership.GetNumberOfUsersOnline();
            var userLogin = Membership.GetUser().UserName;
            var service = Services.MemberService;
            var member = service.GetByUsername(userLogin);
            var mediaService = ApplicationContext.Current.Services.MediaService;
            //int idFolderImage = 0, idFolderMusic = 0, folder;
            var rootFolder = member.Properties["mediaRoot"].Value.ToString(); //replace this foreach
            var mediaFolder = Umbraco.Media(Int32.Parse(rootFolder));
            foreach (var mediaItem in mediaFolder.Children())
            {
                if (mediaItem.Name == "Profile")
                {
                    idImageFoler =  mediaItem.Id;
                    var mediaProfiler = Umbraco.Media(idImageFoler);
                    break;
                }
            }

            return idImageFoler;
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
                int /*idFolderMedia = 0,*/ idFolderImage = 0, folder = 0;

                idFolderImage = getFolderProfile();
                /*foreach (var ww in list)
                {
                    if (ww.ContentType.Alias == "Folder")
                    {
                        if (ww.Name == userLogin)
                        {
                            idFolderMedia = ww.Id;
                            var listChild = mediaService.GetChildren(idFolderMedia);
                            var listChild0 = mediaService.GetDescendants(idFolderMedia);
                            foreach (var child in listChild)
                            {
                                if (child.ContentType.Alias == "Folder")
                                {
                                    if (child.Name == "Images")
                                    {
                                        idFolderImage = child.Id;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                /**************/

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var categories_string = "profile";
                        var stream = fileContent.InputStream;
                        var name = fileContent.FileName;
                        /** media profile **/
                        if (fileContent.ContentType.Contains("image"))
                        {
                            folder = idFolderImage;
                        }
                        /*******/
                        var ms = ApplicationContext.Current.Services.MediaService;
                        var MediaMap = Services.MediaService.CreateMedia(name, folder, "Image");
                        MediaMap.SetValue("umbracoFile", fileContent);
                        //MediaMap.SetValue("shortDescription", category);
                        MediaMap.SetValue("category", categories_string);
                        Services.MediaService.Save(MediaMap);
                        var urlMedia = Umbraco.Media(MediaMap.Id).Url; //banner.image is the property id.
                        member.SetValue("picture", MediaMap.Id);
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