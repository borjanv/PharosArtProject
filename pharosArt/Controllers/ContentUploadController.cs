using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using System.Net;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class ContentUploadController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        public JsonResult UploadContent(int targetRootFolder, string categories)
        {
            int folder;
            string alias = "File";
            var parentFolder = Umbraco.TypedMedia(targetRootFolder);
                                 
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        folder = 0;
                        if (fileContent.ContentType.Contains("image"))
                        {
                            var imageFolder = parentFolder.Descendant<ImagesFolder>();
                            folder = imageFolder.Id;
                            alias = ContentImage.ModelTypeAlias;
                        }
                        if (fileContent.ContentType.Contains("video"))
                        {
                            var videosFolder = parentFolder.Descendant<VideosFolder>();
                            folder = videosFolder.Id;
                            alias = ContentVideo.ModelTypeAlias;
                        }
                        if (fileContent.ContentType.Contains("mp3"))
                        {
                            var musicFolder = parentFolder.Descendant<MusicFolder>();
                            folder = musicFolder.Id;
                            alias = ContentMusic.ModelTypeAlias;
                            categories = categories + "music,";
                        }

                        var cat = categories.Trim(',');

                        var mediaMap = Services.MediaService.CreateMedia(fileContent.FileName, folder, alias);
                        mediaMap.SetValue("category", cat);
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
    }
}