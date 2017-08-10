using pharosArt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace pharosArt.Controllers
{
    public class LandingPageController : SurfaceController
    {
        // Values for number of items per page
        private const int _recordsPerPage = 6;

        public LandingPageController()
        {
            ViewBag.RecordsPerPage = _recordsPerPage;
        }

        public ActionResult Index()
        {
            return RedirectToAction("GetItems");
        }

        public ActionResult GetItems(int? pageNum)
        {
            pageNum = pageNum ?? 0;
            ViewBag.IsEndOfRecords = false;
            if (Request.IsAjaxRequest())
            {
                var items = GetRecordsForPage(pageNum.Value);
                ViewBag.IsEndOfRecords = (items.Any()) && ((pageNum.Value * _recordsPerPage) >= items.Last().Key);
                return PartialView("~/Views/Partials/Home/LandingPageHome.cshtml", items);
            }
            else
            {
                LoadAllItemsToSession();
                var items = GetRecordsForPage(pageNum.Value);
                ViewBag.Items = items;
                return PartialView("~/Views/Partials/Home/LandingPageHome.cshtml", items);
            }
        }

        public void LoadAllItemsToSession()
        {
            var itemsRepo = new InfiniteItemRepositoryController();
            var items = itemsRepo.ListGridItems();
            Session["Items"] = items.ToDictionary(x => x.Key, x => x.Value);
            ViewBag.TotalNumberItems = items.Count();
        }

        public Dictionary<int, LandingPageModel> GetRecordsForPage(int pageNum)
        {
            Dictionary<int, LandingPageModel> items = (Session["Items"] as Dictionary<int, LandingPageModel>);

            int from = (pageNum * _recordsPerPage);
            int to = from + _recordsPerPage;

            return items
                .Where(x => x.Key > from && x.Key <= to)
                .OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }   
    }
}