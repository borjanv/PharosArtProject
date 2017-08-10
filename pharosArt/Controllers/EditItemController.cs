using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using pharosArt.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace pharosArt.Controllers
{
    public class EditItemController : SurfaceController
    {
        public ActionResult Get(int id)
        {
            if (Umbraco.TypedMedia(id) == null)
                return Content("The content could not be found.");

            var tags = Umbraco.TypedMedia(id).GetPropertyValue<IEnumerable<string>>("category").ToList();
            var tagString = new StringBuilder();
            if (tags.Any())
            {
                foreach (var tag in tags)
                {
                    tagString.Append(tag + ",");
                }
            }

            var model = new EditItemModel
            {
                Id = id,
                Tags = tagString.ToString(),
                Title = Umbraco.TypedMedia(id).GetPropertyValue<string>("title")
            };

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
            item.SetValue("category", model.Tags);

            ms.Save(item);

            return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
        }
    }
}