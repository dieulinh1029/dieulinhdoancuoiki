using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HOTPIZZA.Models;

namespace HOTPIZZA.Areas.Admin.Controllers
{
    public class DanhMucMonsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/DanhMucMons
        public async Task<ActionResult> Index()
        {
            var danhMucMons = db.DanhMucMons.Include(d => d.Giohang);
            return View(await danhMucMons.ToListAsync());
        }

        // GET: Admin/DanhMucMons/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucMon danhMucMon = await db.DanhMucMons.FindAsync(id);
            if (danhMucMon == null)
            {
                return HttpNotFound();
            }
            return View(danhMucMon);
        }

        // GET: Admin/DanhMucMons/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.Giohangs, "IdMon", "TenMon");
            return View();
        }

        // POST: Admin/DanhMucMons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdDanhMuc,TenDanhMuc")] DanhMucMon danhMucMon)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucMons.Add(danhMucMon);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.Giohangs, "IdMon", "TenMon", danhMucMon.IdDanhMuc);
            return View(danhMucMon);
        }

        // GET: Admin/DanhMucMons/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucMon danhMucMon = await db.DanhMucMons.FindAsync(id);
            if (danhMucMon == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.Giohangs, "IdMon", "TenMon", danhMucMon.IdDanhMuc);
            return View(danhMucMon);
        }

        // POST: Admin/DanhMucMons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdDanhMuc,TenDanhMuc")] DanhMucMon danhMucMon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMucMon).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.Giohangs, "IdMon", "TenMon", danhMucMon.IdDanhMuc);
            return View(danhMucMon);
        }

        // GET: Admin/DanhMucMons/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucMon danhMucMon = await db.DanhMucMons.FindAsync(id);
            if (danhMucMon == null)
            {
                return HttpNotFound();
            }
            return View(danhMucMon);
        }

        // POST: Admin/DanhMucMons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DanhMucMon danhMucMon = await db.DanhMucMons.FindAsync(id);
            db.DanhMucMons.Remove(danhMucMon);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
