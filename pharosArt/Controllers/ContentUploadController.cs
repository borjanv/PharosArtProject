using System;
using System.Collections.Generic;
using System.Linq;
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
        public JsonResult UploadContent(int targetRootFolder)
        {
            int folder;
            string alias = "File";
            var parentFolder = Umbraco.TypedMedia(targetRootFolder);
            var imageFolder = parentFolder.Descendant<ImagesFolder>();
            var musicFolder = parentFolder.Descendant<MusicFolder>();
            var videosFolder = parentFolder.Descendant<VideosFolder>();

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
                            folder = imageFolder.Id;
                            alias = ContentImage.ModelTypeAlias;
                        }
                        if (fileContent.ContentType.Contains("video"))
                        {// needs testing
                            folder = videosFolder.Id;
                            alias = ContentVideo.ModelTypeAlias;
                        }
                        if (fileContent.ContentType.Contains("mp3"))
                        {
                            folder = musicFolder.Id;
                            alias = ContentMusic.ModelTypeAlias;
                        }

                        string categories_string = "";

                        foreach (string key in Request.Form.AllKeys)
                        {
                            if (key.StartsWith("Categories"))
                            {
                                categories_string = Request.Form[key];
                            }
                        }

                        //var mediaMap = Services.MediaService.CreateMedia(fileContent.FileName, folder, alias);
                        //mediaMap.SetValue("category", categories_string);
                        //mediaMap.SetValue("umbracoFile", fileContent);
                        //Services.MediaService.Save(mediaMap);
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