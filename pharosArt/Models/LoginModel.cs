using System.ComponentModel.DataAnnotations;

namespace pharosArt.Models
{
    public class LoginModel
    {
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string urlImage { get; set; }

        //[Display(Name = "isArt")]
        //public bool isArt { get; set; }
    }
}