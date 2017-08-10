using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.Media.ThumbnailProviders;

namespace pharosArt.Models
{
    public class EditItemModel
    {
        public string Title { get; set; }
        [Display(Name = "Categories (comma separated)")]
        public string Tags { get; set; }
        public int Id { get; set; }
    }
}