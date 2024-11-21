using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HOTPIZZA.Controllers
{
    public class TuyenDungController : Controller
    {
        private HOTPIZZAEntity db= new HOTPIZZAEntity();
        // GET: TuyenDung
        private List<TuyenDung> Show(int count)
        {
            return db.TuyenDungs.OrderByDescending(a => a.IdBaiTuyenDung).Take(count).ToList();
        }

        public ActionResult Index()
        {
            var gg = Show(4);
            if (gg != null)
            {
                return View(gg);
            }
            else
            {
                return View();
            }
        } 
        public ActionResult Tuyendung()
        {
            var c = from s in db.TuyenDungs
                    
                    select s;
            return PartialView(c);
        } 
        public ActionResult ycTuyenDung(int id)
        {
            var m = from s in db.TuyenDungs
                    where s.IdBaiTuyenDung == id
                    select s;
            return PartialView(m);
        }
    }
}