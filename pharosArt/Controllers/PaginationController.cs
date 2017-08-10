using pharosArt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace pharosArt.Controllers
{
    public class PaginationController : SurfaceController
    {
        // GET: Pagination
        public ActionResult ShowPages(int _totalPages, int _currentPage)
        {
            var model = new PaginationModel();
            model.totalPages = _totalPages;
            model.currentPage = _currentPage;
            return PartialView("~/Views/Partials/PaginationPartial.cshtml", model);
        }
    }
}