using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using umbraco.IO;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;
using Umbraco.Core;
using System.Web.Security;

using Umbraco.Web.Mvc;
using pharosArt.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Core.Models;


namespace pharosArt.Controllers
{
    public class ImageUploadController : Umbraco.Web.Mvc.SurfaceController
    {
        public int idFolderImage { get; set; }
        public int idFolderMusic { get; set; }

        public void getFoldersMedia()
        {
            /** upload media to the profile **/
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
                if (mediaItem.Name == "Images")
                {
                    this.idFolderImage = mediaItem.Id;
                }
                else
                {
                    this.idFolderMusic = mediaItem.Id;
                }
            }
        }

        // GET: ImageUpload
        [HttpPost]
        public async Task<JsonResult> UploadImage(string id)
        {
            int folder;
            var member = (Umbraco.Web.PublishedContentModels.Member)Members.GetById(Int32.Parse(id));
            var username = Membership.GetUser().UserName;
            
            try
            {
                //getFoldersMedia();
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
						List<string> categories = new List<string>();
						string categories_string = "";
						
						foreach(string key in Request.Form.AllKeys) {
							if(key.StartsWith("Categories")) 
							{
								categories = (Request.Form[key]).Split(',').ToList();
								categories_string = Request.Form[key];
							}
						}
					
                        var stream = fileContent.InputStream;
                        var name = fileContent.FileName;
                        /** media profile **/
                        if (fileContent.ContentType.Contains("image"))
                        {
                            folder = member.MediaRoot.Descendants<IPublishedContent>().Where(x => x.DocumentTypeAlias == Image.ModelTypeAlias
                             || x.DocumentTypeAlias == "File").Where(x => x.Parent.DocumentTypeAlias == ImagesFolder.ModelTypeAlias).FirstOrDefault().Parent.Id;
                        }
                        else
                        {
                            folder = member.MediaRoot.Descendants<IPublishedContent>().Where(x => x.DocumentTypeAlias == Image.ModelTypeAlias
                             || x.DocumentTypeAlias == "File").Where(x => x.Parent.DocumentTypeAlias == ImagesFolder.ModelTypeAlias).FirstOrDefault().Parent.Id;
                                //member.MediaRoot.Descendants<IPublishedContent>().Where(x => x.Name == username).FirstOrDefault().Id;
                                //member.MediaRoot.Descendants<IPublishedContent>().Where(x => x.DocumentTypeAlias == MusicFolder.ModelTypeAlias).FirstOrDefault().Id; //idFolderMusic;
                        }
                        /*******/
                        var ms = ApplicationContext.Current.Services.MediaService;
                        var MediaMap = Services.MediaService.CreateMedia(name, folder, "Image");
						MediaMap.SetValue("category", categories_string);
                        MediaMap.SetValue("umbracoFile", fileContent);
                        Services.MediaService.Save(MediaMap);
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
        
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var parent = Services.MediaService.GetRootMedia().Single();
                var media = Services.MediaService.CreateMedia(file.FileName, parent, "Image");
                media.SetValue("umbracoFile", file);

                Services.MediaService.Save(media);
            }
            return RedirectToAction("Upload");
        }
    }
}