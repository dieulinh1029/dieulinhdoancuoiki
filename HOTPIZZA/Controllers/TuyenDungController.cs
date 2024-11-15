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
        private HOTPIZZAEntities1 db= new HOTPIZZAEntities1();
        // GET: TuyenDung
        public ActionResult Index()
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