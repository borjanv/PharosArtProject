using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using pharosArt.Models.Shared;

namespace pharosArt.Models
{
    public class EditItemModel
    {
        public string Title { get; set; }
        [Display(Name = "Categories (comma separated)")]
        public int Id { get; set; }
        public int VideoThumbnailId { get; set; }
        public HttpPostedFileBase File { get; set;}
        public IEnumerable<CheckboxItemViewModel> Categories { get; set; }
    }
}