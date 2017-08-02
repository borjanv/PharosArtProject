using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class GetContentController:SurfaceController
    {
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var model = Umbraco.TypedMedia(id);
            if (model != null)
                return PartialView("~/Views/Partials/_AudioAjaxPartial.cshtml", new File(model));

            return Content("The requested resource could not be found.");
        }
    }
}