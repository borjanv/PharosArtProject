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

            var mediaFiles = mediaFolder.Descendants().Where(x => x.DocumentTypeAlias == ContentImage.ModelTypeAlias ||
                                                                  x.DocumentTypeAlias == ContentMusic.ModelTypeAlias ||
                                                                  x.DocumentTypeAlias == ContentVideo.ModelTypeAlias)
                .ToList();

            if (mediaFiles.Any())
            {
                foreach (var mediafile in mediaFiles)
                {
                    string category = mediafile.GetProperty("category").DataValue.ToString();

                    List<string> categories = GetCategories(category);

                    if (category.ToLower().Contains(subpageCategory.ToLower()))
                    {
                        models.Add(new LandingPageModel
                        {
                            Media = mediafile,
                            Author = mediafile.Ancestor<ParentFolder>().Member.Name,
                            MediaUrl = mediafile.Url,
                            UploadDate = mediafile.CreateDate,
                            Categories = categories,
                            MemberId = mediafile.Ancestor<ParentFolder>().Member.Id
                        });
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