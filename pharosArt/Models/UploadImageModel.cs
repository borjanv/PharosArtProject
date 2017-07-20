﻿using System.Web;
using Umbraco.Web.PublishedContentModels;
using Image = Umbraco.Web.PublishedContentModels.Image;

namespace pharosArt.Models
{
    public class UploadImageModel
    {
        public HttpPostedFileBase UploadFile { get; set; }
        public Image ProfileImage { get; set; }
        public int ProfileFolder { get; set; }
        public int MemberId { get; set; }
    }
}