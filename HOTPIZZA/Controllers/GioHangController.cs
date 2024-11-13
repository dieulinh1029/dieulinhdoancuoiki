using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HOTPIZZA.Controllers
{
    public class GioHangController : Controller
    {
        HOTPIZZAEntities1 db = new HOTPIZZAEntities1();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult dsa()
            {
            return View();
            }
    }
}