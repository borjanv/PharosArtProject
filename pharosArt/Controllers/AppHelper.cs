using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt.Controllers
{
    public static class AppHelper
    {
        public static UmbracoHelper UmbHelper()
        {
            const string key = "umb-helper";
            if (!HttpContext.Current.Items.Contains(key))
            {
                HttpContext.Current.Items.Add(key, new UmbracoHelper(UmbracoContext.Current));
            }
            return HttpContext.Current.Items[key] as UmbracoHelper;
        }

        public static Home GetHomeNode()
        {
           return (Home)UmbHelper().TypedContentAtRoot().First(x => x.DocumentTypeAlias == Home.ModelTypeAlias);
        }
    }
}