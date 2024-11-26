using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HOTPIZZA.Areas.Admin.Controllers
{
    public class NguoiDungsController : Controller
    {
        private HOTPIZZAEntity db = new HOTPIZZAEntity();
        // GET: Admin/NguoiDungs
        public async Task<ActionResult> Index()
        {
            var das = db.NguoiDungs.Include(d => d.DonDatHangs).Include(d => d.GopYKhachHangs).Include(d => d.PhanQuyen);
            return View(await das.ToListAsync());
        }

        // GET: Admin/NguoiDungs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nd = await db.NguoiDungs.FindAsync(id);
            if (nd == null)
            {
                return HttpNotFound();
            }
            return View(nd);
        }

        // GET: Admin/NguoiDungs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NguoiDungs/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/NguoiDungs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/NguoiDungs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/NguoiDungs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/NguoiDungs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
