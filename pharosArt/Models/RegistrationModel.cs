using System;
using System.Collections.Generic;
using System.Linq;

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