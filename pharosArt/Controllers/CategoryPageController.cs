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
    public class CategoryPageController : SurfaceController
    {
        // GET: CategoryPage
        public ActionResult ShowUploadedImages()
        {
            var subpageCategory = CurrentPage.Name;
            var models = new List<LandingPageModel>();
            var mediaFolder = Umbraco.TypedMedia(5830);
            int mediaID;
            string mediaUrl = null;
            string category;
            List<string> categories;
            DateTime date;
            var allowedCategories = Umbraco.TypedContentAtRoot().First().Descendants("Menu").ToList();

            var mediaFiles = mediaFolder.Descendants().Where(x => x.DocumentTypeAlias == Image.ModelTypeAlias || x.DocumentTypeAlias == "File").Where(x => x.Parent.DocumentTypeAlias != ProfileFolder.ModelTypeAlias);
            
            if (mediaFiles.Any())
            {
                foreach (var mediafile in mediaFiles)
                {
                    mediaUrl = mediafile.Url;
                    date = mediafile.CreateDate;
                    mediaID = mediafile.Id;
                    if (mediafile.DocumentTypeAlias == Image.ModelTypeAlias)
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
                    if(categories.Contains(subpageCategory))
                    {
                        models.Add(new LandingPageModel { Media = mediafile, Author = mediafile.Ancestor<ParentFolder>().Member.Name, MediaUrl = mediaUrl, UploadDate = date, Categories = categories, MemberId = mediafile.Ancestor<ParentFolder>().Member.Id });
                    }
                }
            }
            
            List<LandingPageModel> sortedModels = models.OrderByDescending(o => o.UploadDate).ToList(); //sorting images by date
            return PartialView("~/Views/Partials/Home/LandingPage.cshtml", sortedModels);
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
    }
}