using System;
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

        [HttpPost]
        public ActionResult DeleteContentById(int id)
        {
            if (id == 0)
                throw new NotImplementedException();
            var mediaToDelete = Services.MediaService.GetById(id);

            try
            {
                Services.MediaService.Delete(mediaToDelete, 0);
            }catch(Exception e)
            {
                throw new NotImplementedException();
            }
            
        }
    }
}