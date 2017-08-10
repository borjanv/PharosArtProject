using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class GetContentController : SurfaceController
    {
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var model = Umbraco.TypedMedia(id);
            if (model != null)
            {
                if (model.DocumentTypeAlias == ContentMusic.ModelTypeAlias)
                    return PartialView("~/Views/Partials/_AudioAjaxPartial.cshtml", new File(model));
                if (model.DocumentTypeAlias == ContentVideo.ModelTypeAlias)
                    return PartialView("~/Views/Partials/_VideoAjaxPartial.cshtml", new ContentVideo(model));
            }
                

            return Content("The requested resource could not be found.");
        }

        [HttpGet]
        public ActionResult ConfirmContentDelete(int id)
        {
            var model = Umbraco.TypedMedia(id);
            if (model != null)
                return PartialView("~/Views/Partials/DeleteContentConfirmation.cshtml", model);

            return Content("Content item not found in our database.");
        }

        [HttpPost]
        public ActionResult DeleteContentById(int id)
        {
            if (id == 0)
                throw new NotImplementedException();
            var mediaToDelete = Services.MediaService.GetById(id);

            try
            {
                Services.MediaService.Delete(mediaToDelete, 0);
                return RedirectToUmbracoPage(AppHelper.GetHomeNode().ProfilePage.Id);
            }
            catch (Exception e)
            {
                return Content("There was an error, please try again.");
            }

        }
    }
}