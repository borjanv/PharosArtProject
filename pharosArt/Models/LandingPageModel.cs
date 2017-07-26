using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace pharosArt.Models
{
    public class LandingPageModel
    {
        public IPublishedContent Media { get; set; }
        public string Author { get; set; }
        public string MediaUrl { get; set; }
        public DateTime UploadDate { get; set; } 
        public List<string> Categories { get; set; }
        public int MemberId { get; set; }
    }
}