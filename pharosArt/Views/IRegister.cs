using System.Web.Security;
using Umbraco.Web.Models;

namespace pharosArt.Controllers
{
    public interface IRegister
    {
        MembershipCreateStatus RegisterMember(RegisterModel member);
    }
}