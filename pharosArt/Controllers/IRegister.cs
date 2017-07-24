using System.Web.Security;
using Umbraco.Core;

namespace pharosArt.Controllers
{
    public interface IRegister
    {
        MembershipCreateStatus RegisterMember(IUmbracoMember member);
        GuidUdi CreateParentMediaFolderForMember(IUmbracoMember member);
    }

    public interface IUmbracoMember
    {
        string Id { get; set; }
        int UmbracoId { get; set; }
        string Genre { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        int MediaFolder { get; set; }
        string Email { get; set; }
    }
}