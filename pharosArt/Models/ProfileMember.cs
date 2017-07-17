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

        [Display(Name = "Biography")]
        public string biography { get; set; }

        public LoginModel profile { get; set; }

        public List<Tuple<string, string>> mediaMember { get; set; }
        //public List<TupleList<string, string>> mediaMember2 { get; set; }
        //public Tuple<string, string> scores { get; set; }

        public ProfileMember()
        {
            this.profile = new LoginModel();
            mediaMember = new List<Tuple<string, string>>();
            //mediaMember2 = new List<TupleList<string, string>>();
        }
    }

    //public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    //{
    //    public void Add(T1 item, T2 item2)
    //    {
    //        Add(new Tuple<T1, T2>(item, item2));
    //    }
    //}
}