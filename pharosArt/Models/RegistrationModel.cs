using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using pharosArt.Controllers;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Models
{
    public class RegistrationModel
    {
        public UmbracoMember Member { get; set; }
        public IEnumerable<string> Genres { get; set; }

        public RegistrationModel()
        {
            Genres = Enum.GetNames(typeof(Genre)).ToList();
        }
    }

    public enum Genre
    {
        Genre,
        Art,
        Music,
        Photography,
        Film,
        Fashion
    }
}