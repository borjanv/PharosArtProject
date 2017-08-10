using pharosArt.Controllers;
using System.ComponentModel.DataAnnotations;

namespace pharosArt.Models
{
    public class UmbracoMember : IUmbracoMember
    {
        public string Id { get; set; }
        public int UmbracoId { get; set; }
        [Display(Name = "Genre")]
        public string Genre { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public int MediaFolder { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address is not valid")]
        [Required]
        public string Email { get; set; }
    }
}