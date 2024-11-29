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
        private HOTPIZZAEntity db = new HOTPIZZAEntity();
        private List<MonAn> ShowMon(int count)
        {
            return db.MonAns.OrderByDescending(a => a.TenMon).Take(count).ToList();
        }
        public ActionResult Index(int page = 1, int pageSize = 9)
        {
            var totalRecords = db.MonAns.Count(); // Tổng số món ăn
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            var monAnList = db.MonAns
                             .OrderByDescending(a => a.TenMon)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return View(monAnList);
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