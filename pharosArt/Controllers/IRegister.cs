using System.Web.Security;
using pharosArt.Models;
using Umbraco.Core.Models;

namespace pharosArt.Controllers
{
    public interface IRegister
    {
        MembershipCreateStatus RegisterMember(IUmbracoMember member);
        int CreateParentMediaFolderForMember(IUmbracoMember member);
    }

    public interface IUmbracoMember
    {
        int Id { get; set; }
        string Genre { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        int MediaFolder { get; set; }
        string Email { get; set; }
    }
}