using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Web.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Net;
namespace HOTPIZZA.Controllers
{
    public class DatHangController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();
        private List<MonAn> ShowMon(int count)
        {
            return db.MonAns.OrderByDescending(a=>a.TenMon).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var gg = ShowMon(8);
            if (gg != null)
            {
                return View(gg);    
            }
            else
            { 
                return View();
            }
           
        }
        public ActionResult Chitiet(string id)
        {
            var mon = from s in db.MonAns
                      where s.IdMon == id
                      select s;
            return View(mon.Single());
        }

        public ActionResult DanhMuc()
        {
            var dm = from d in db.DanhMucMons
                     select d;
            return PartialView(dm);
        }
        public ActionResult SXDanhMuc(string id)
        {
            var mon = from d in db.MonAns
                     where d.IdDanhMuc == id
                     select d;
            return PartialView(mon);
        }

    }
}