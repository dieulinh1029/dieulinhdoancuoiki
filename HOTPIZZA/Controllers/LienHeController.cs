using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HOTPIZZA.Controllers
{
    public class LienHeController : Controller
    {
        HOTPIZZAEntity db = new HOTPIZZAEntity();
        [HttpGet]
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(GopYKhachHang model)
        {
            if (ModelState.IsValid)
            {
                model.NgayGopY = DateTime.Now;
                db.GopYKhachHangs.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Cảm ơn bạn đã gửi góp ý!";
                return RedirectToAction("GuiGopY");
            }
            return View(model);
        }
        public ActionResult GuiGopY()
        {
            return View();
        }
    }
}