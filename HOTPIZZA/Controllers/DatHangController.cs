using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HOTPIZZA.Controllers
{
    public class DatHangController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();
        private List<MonAn> monAns = new List<MonAn>();
        // GET: DatHang
        public ActionResult Index()
        {
            return View();
        }
    }
}