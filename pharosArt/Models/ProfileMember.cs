using System.ComponentModel.DataAnnotations;

using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System;
using System.Web.Security;
using pharosArt.Models;
using System.Web;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Models;

namespace pharosArt.Models
{
    public class ProfileMember
    {
        public int memberId { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string memberName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string memberLastName { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        public LoginModel profile { get; set; }

        public ProfileMember()
        {
            this.profile = new LoginModel();
        }
    }
}