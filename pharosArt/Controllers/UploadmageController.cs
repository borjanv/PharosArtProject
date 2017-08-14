using System;
using System.Web.Mvc;
using pharosArt.Models;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public class UploadImageController : SurfaceController
    {

        [HttpGet]
        public ActionResult Get(int memberId)
        {
            var member = new Member(Members.GetById(memberId));

            return PartialView("UploadImagePartial", new UploadImageModel
            {
                ProfileFolder = member.MediaRoot.Descendant<ProfileFolder>().Id,
                ProfileImage =
                    member.HasValue("picture") ? new Image(member.Picture).Id : 6927,
                MemberId = memberId
            });
        }

        [HttpPost]
        public ActionResult Post(UploadImageModel model)
        {
            string result = "Error";
            try
            {
                if (model.UploadFile != null && model.UploadFile.ContentLength > 0)
                {
                    var ms = Services.MediaService;
                    var name = model.UploadFile.FileName;

                    if (!model.UploadFile.ContentType.Contains("image"))
                    {
                        return Json("File must be an image");
                    }
                    var member = Services.MemberService.GetById(model.MemberId);

                    var media = ms.CreateMedia(name, model.ProfileFolder, "Image");
                    media.SetValue("umbracoFile", model.UploadFile);
                    ms.Save(media);

                    // delete old picture before sabing new one
                    if (model.ProfileImage != 0 || model.ProfileImage != 6927)
                        ms.Delete(ms.GetById(model.ProfileImage));

                    member.SetValue("picture", media.GetUdi().ToString());                 
                    result = Umbraco.TypedMedia(media.Id).GetCropUrl("Profile");

                    Services.MemberService.Save(member);

                    return Json(new Tuple<string, string>("OK", result));
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                return Json(new Tuple<string, string>("Error", "Internal error."));

            }
            return Json(new Tuple<string, string>("Error", "Please select an image."));

        }

    }
}
