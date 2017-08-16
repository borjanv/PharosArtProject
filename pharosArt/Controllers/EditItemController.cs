using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using pharosArt.Models;
using pharosArt.Models.Shared;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class EditItemController : SurfaceController
    {
        public ActionResult Get(int id)
        {
            if (Umbraco.TypedMedia(id) == null)
                return Content("The content could not be found.");

            if (!Umbraco.MemberIsLoggedOn())
                return Content("You have to be logged in to edit this item");

            var content = Umbraco.TypedMedia(id);
            var tags = content.GetPropertyValue<IEnumerable<string>>("category").ToList();
            var catList = new List<CheckboxItemViewModel>();
            var allTags = AppHelper.GetCategories().ToList().ConvertAll(x => x.ToLower());

            foreach (var c in allTags)
            {
                catList.Add(new CheckboxItemViewModel { Checked = (tags.ConvertAll(x => x.ToLower()).Contains(c)), Name = c });
            }

            var model = new EditItemModel
            {
                Id = id,
                Title = content.GetPropertyValue<string>("title"),
                Categories = catList
            };

            if (content.DocumentTypeAlias == ContentVideo.ModelTypeAlias)
            {
                if (content.HasValue("thumbnail"))
                    model.VideoThumbnailId = content.GetPropertyValue<int>("thumbnail");
            }

            return PartialView("~/Views/Partials/EditItem/EditItemForm.cshtml", model);
        }

        [HttpPost]
        public ActionResult Post(EditItemModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var ms = Services.MediaService;
            var item = ms.GetById(model.Id);

            item.SetValue("title", model.Title);
            if (model.Categories.Any(x => x.Checked))
            {
                var tagsToSave = new StringBuilder();
                foreach (var t in model.Categories.Where(x => x.Checked))
                    tagsToSave.Append(t.Name + ",");

                item.SetValue("category", tagsToSave.ToString().TrimEnd(','));
            }

            if (item.ContentType.Alias == ContentVideo.ModelTypeAlias && model.File != null)
            {
                int thumbnailRoot = (AppHelper.GetHomeNode().HasValue("videoThumbnailsMediaFolder"))
                    ? AppHelper.GetHomeNode().VideoThumbnailsMediaFolder.Id
                    : -1;

                var newMedia = ms.CreateMedia(model.Title + "-thumbnail", thumbnailRoot, "Image");
                newMedia.SetValue("umbracoFile", model.File);
                ms.Save(newMedia);

                // delete old thumbnail before setting new one
                if (model.VideoThumbnailId != 0)
                    ms.Delete(ms.GetById(model.VideoThumbnailId));

                item.SetValue("thumbnail", newMedia.GetUdi().ToString());
            }

            ms.Save(item);

            return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
        }
    }
}