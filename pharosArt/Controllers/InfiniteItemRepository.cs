using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Models
{
    public class InfiniteItemRepositoryController:SurfaceController
    {
        public Dictionary<int, LandingPageModel> ListGridItems()
        {
            int i = 1;
            var itemsDictionary = new Dictionary<int, LandingPageModel>();

            var mediaFolder = Umbraco.TypedMedia(5830);
            List<string> categories;
            string category;

            var mediaFiles = mediaFolder.Descendants().Where(x => x.DocumentTypeAlias == ContentImage.ModelTypeAlias ||
                x.DocumentTypeAlias == ContentMusic.ModelTypeAlias || x.DocumentTypeAlias == ContentVideo.ModelTypeAlias)
                .OrderByDescending(x => x.CreateDate).ToList();

            if (mediaFiles.Any())
            {
                foreach (var mediafile in mediaFiles)
                {
                    category = mediafile.GetPropertyValue<string>("category");
                    categories = GetCategories(category);
                    itemsDictionary.Add(i, new LandingPageModel { Media = mediafile, Author = mediafile.Ancestor<ParentFolder>().Member.Name, MediaUrl = mediafile.Url, UploadDate = mediafile.CreateDate, Categories = categories, MemberId = mediafile.Ancestor<ParentFolder>().Member.Id });

                    i++;
                }
            }
            return itemsDictionary;
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