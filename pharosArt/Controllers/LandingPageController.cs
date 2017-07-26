using pharosArt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class LandingPageController : SurfaceController
    {

        // GET: LandingPage
        public ActionResult ShowUploadedImages()
        {
            var models = new List<LandingPageModel>();
            /*models.Add(new LandingPageModel { Author = "Amy Eight" });
            models.Add(new LandingPageModel { Author = "Amy Nine" });

            var mediaID = 5876;
            var media = Umbraco.Media(mediaID);
            models.Add(new LandingPageModel { Author = "Amy Ten", ImageUrl = media.Url, UploadDate = media.UpdateDate });*/

            var mediaFolder = Umbraco.TypedMedia(5830);
            int mediaID;
            string mediaUrl = null;
            DateTime date;
            List<string> categories;
            string category;

            //var images = mediaFolder.Descendants<Image>().Where(x => x.DocumentTypeAlias == Image.ModelTypeAlias || x.DocumentTypeAlias == MusicFolder.ModelTypeAlias).Where(x => x.Parent.DocumentTypeAlias == ImagesFolder.ModelTypeAlias);
            var mediaFiles = mediaFolder.Descendants().Where(x => x.DocumentTypeAlias == Image.ModelTypeAlias || x.DocumentTypeAlias == "File").Where(x => x.Parent.DocumentTypeAlias != ProfileFolder.ModelTypeAlias);

            if (mediaFiles.Any())
            {
                foreach (var mediafile in mediaFiles)
                {
                    mediaUrl = mediafile.Url;
                    date = mediafile.CreateDate;
                    mediaID = mediafile.Id;
                        
                    if(mediafile.DocumentTypeAlias == Image.ModelTypeAlias)
                    {
                        category = mediafile.GetProperty("category").DataValue.ToString(); 
                    }
                    else
                    {
                        if (mediafile.GetPropertyValue<string>("umbracoExtension") == "mp3")
                        {
                            category = "music";
                        }
                        else
                        {
                            category = "";
                        }
                    }
                    categories = GetCategories(category);
                    models.Add(new LandingPageModel { MediaID = mediaID, Author = mediafile.Ancestor<ParentFolder>().Member.Name, MediaUrl = mediaUrl, UploadDate = date, Categories = categories, MemberId = mediafile.Ancestor<ParentFolder>().Member.Id });
                }
            }

            List<LandingPageModel> sortedModels = models.OrderByDescending(o => o.UploadDate).ToList(); //sorting images by date
            return PartialView("LandingPage", sortedModels);
        }

        private List<string> GetCategories(string category)
        {
            List<string> categories = new List<string>();
            category = category.Replace(" ", string.Empty);
            category = category.ToUpper();
            if (category != null)
            {
                if (category.Contains(','))
                {
                    categories = category.Split(',').ToList();
                    return categories;
                }
            }
            categories.Add(category);
            return categories;
        }

        /*public ActionResult DisplayImages(LandingPageModel model)
        {
            return CurrentUmbracoPage();
        }*/
    }
}