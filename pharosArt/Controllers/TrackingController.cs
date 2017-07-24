using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System;
using System.Web.Security;
using pharosArt.Models;
using System.Web;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using System.Linq;
using Umbraco.Web.Models;
using umbraco.IO;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Core.Models;

namespace pharosArt.Controllers
{
    public class TrackingController : SurfaceController
    {
        [HttpGet]
        public bool updateStatistic(string idMedia, string statistic)
        {
            bool result = true;
            try
            {
                var db = new PetaPoco.Database("umbracoDbDSN");
                //long count = db.ExecuteScalar<long>("SELECT Count(*) FROM mauroTest WHERE id=@0", idMedia);
                var increaseStatistic = db.SingleOrDefault<Statistics>("SELECT * FROM Statistics WHERE id=@0", idMedia);

                if (increaseStatistic != null)
                {
                    db.Update("Statistics", "id", new { times = increaseStatistic.times + 1 }, idMedia);
                }
                else
                {
                    var mediaStatistic = new Statistics { id = Int32.Parse(idMedia), times = 1 };

                    db.Insert("Statistics", "id", false, mediaStatistic);
                    //db.Insert(mediaStatistic);
                    // object identity = db.Insert(mediaStatistic);
                }
            }
            catch (Exception e)
            {
                result = false;
                e.ToString();
            }

            //foreach (var a in db.Query<mauroTest>("SELECT * FROM mauroTest"))
            //{
            //    Console.WriteLine("{0} - {1}", a.id, a.name);
            //}

            //var w = db.SingleOrDefault<mauroTest>("SELECT * FROM mauroTest WHERE id=@0", 1);

            return result;
        }

    }
}