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
//using Umbraco.Core.Services;
//using Umbraco.Core.Models;
//using Umbraco.Web.Mvc;
using System.Web.Security;
//using pharosArt.Models;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class ImageUploadController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public JsonResult UploadImage(int targetRootFolder)
        {
            int folder;
            var parentFolder = Umbraco.TypedMedia(targetRootFolder);
            var imageFolder = parentFolder.Descendant<ImagesFolder>();
            var musicFolder = parentFolder.Descendant<MusicFolder>();

            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        folder = fileContent.ContentType.Contains("image") ? imageFolder.Id : musicFolder.Id;

						var categories = new List<string>();
						string categories_string = "";
						
						foreach(string key in Request.Form.AllKeys) {
							if(key.StartsWith("Categories")) 
							{
								categories = (Request.Form[key]).Split(',').ToList();
								categories_string = Request.Form[key];
							}
						}
				
                        var name = fileContent.FileName;

                        var mediaMap = Services.MediaService.CreateMedia(name, folder, "Image");
						mediaMap.SetValue("category", categories_string);
                        mediaMap.SetValue("umbracoFile", fileContent);
                        Services.MediaService.Save(mediaMap);
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