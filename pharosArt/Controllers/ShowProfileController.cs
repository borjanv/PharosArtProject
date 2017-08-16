using pharosArt.Models;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;
using Member = Umbraco.Web.PublishedContentModels.Member;

namespace pharosArt.Controllers
{
    public class ShowProfileController : SurfaceController
    {
        // GET: ShowProfile
        public ActionResult ShowProfile(int memberId)
        {

            var profile = new EditModel();
            var member = new Member(Members.GetById(memberId));
            if (member.MediaRoot == null)
                return Content("Media folder not found. Please contact administrator.");

            profile.MemberId = member.Id;
            profile.Biography = member.Biography;
            profile.FirstName = member.FirstName;
            profile.LastName = member.LastName;
            profile.RootMedia = new ProfileFolder(member.MediaRoot);
            profile.ProfileImage = member.HasValue("picture") ? new Image(member.Picture) : new Image(Umbraco.TypedMedia(6927));

            return View("~/Views/Partials/ShowProfilePartial.cshtml", profile);
        }
    }
}