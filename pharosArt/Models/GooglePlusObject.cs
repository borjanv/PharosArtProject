using System.Runtime.Serialization;

namespace pharosArt.Models
{
    public class GooglePlusObject
    {
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "id")]
        public string GoogleId { get; set; }
        [DataMember(Name = "ProfileUrl")]
        public string ProfileUrl { get; set; }
        [DataMember(Name = "Email")]
        public string Email { get; set; }
    }
}