using pharosArt.Models;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System;
namespace pharosArt.Controllers
{
    public class TrackingController : SurfaceController
    {
        [HttpGet]
        public bool UpdateStatistic(int idMedia)
        {
            bool result = true;
            if (idMedia != null && idMedia != 0)
            {
                try
                {
                    var db = new PetaPoco.Database("umbracoDbDSN");
                    var increaseStatistic = db.SingleOrDefault<Statistics>("SELECT * FROM [Statistics] WHERE mediaId=@0", idMedia);

                    if (increaseStatistic != null)
                    {
                        db.Update("Statistics", "mediaId", new { times = increaseStatistic.Times + 1 }, idMedia);
                    }
                    else
                    {
                        var mediaStatistic = new Statistics { MediaId = idMedia, Times = 1 };

                        db.Insert("Statistics", "mediaId", false, mediaStatistic);
                    }
                }
                catch (Exception e)
                {
                    var s = e;
                    result = false;
                }
            }
            return result;
        }

    }
}