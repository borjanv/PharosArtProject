using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public static IEnumerable<string> GetCategories()
        {
            var model = new List<string>();
            if (
                UmbHelper()
                    .TypedContentAtRoot()
                    .FirstOrDefault(x => x.DocumentTypeAlias == Categories.ModelTypeAlias) != null && UmbHelper()
                        .TypedContentAtRoot()
                        .FirstOrDefault(x => x.DocumentTypeAlias == Categories.ModelTypeAlias).Children.Any())
            {
                model.AddRange(UmbHelper().TypedContentAtRoot().First(x => x.DocumentTypeAlias == Categories.ModelTypeAlias).Children.Select(category => category.Name));
            }
            return model;
        }
    }
}