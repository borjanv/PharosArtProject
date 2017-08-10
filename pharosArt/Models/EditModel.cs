using System.ComponentModel.DataAnnotations;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Models
{
    public class EditModel
    {
        public int MemberId { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Biography")]
        public string Biography { get; set; }

        //public LoginModel profile { get; set; }

        public ProfileFolder RootMedia { get; set; }

        public Image ProfileImage { get; set; }

        public string UserName { get; set; }

        //public ProfileMember()
        //{
        //    profile = new LoginModel();
        //    mediaMember = new List<Tuple<string, string>>();
        //}
    }

}