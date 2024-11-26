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
        private HOTPIZZAEntity db = new HOTPIZZAEntity();

        // GET: Admin/danhmuc
        public async Task<ActionResult> Index()
        {
            var dmm = db.DanhMucMons.Include(d => d.MonAns);
            return View(await dmm.ToListAsync());
        }

        // GET: Admin/DanhMucMons/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/DanhMucMons/Create
        public ActionResult Create()
        {
            
            ViewBag.MaKH = new SelectList(db.MonAns, "IdMon", "TenMon");
            
            return View();
        }

        // POST: Admin/DonDatHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdDanhMuc,TenDanhMuc")] DanhMucMon dmm)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucMons.Add(dmm);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.MonAns, "IdMon", "TenMon",dmm.IdDanhMuc);
            
            return View(dmm);
        }

        // GET: Admin/DanhMucMons/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucMon dmm = await db.DanhMucMons.FindAsync(id);
            if (dmm == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.MonAns, "IdMon", "TenMon", dmm.IdDanhMuc);

            return View(dmm);
        }

        
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdDanhMuc,TenDanhMuc")] DanhMucMon dmm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dmm).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.MonAns, "IdMon", "TenMon", dmm.IdDanhMuc);

            return View(dmm);
        }


        // GET: Admin/DanhMucMons/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucMon dmm = await db.DanhMucMons.FindAsync(id);
            if (dmm == null)
            {
                return HttpNotFound();
            }
            return View(dmm);
        }

        // POST: Admin/DonDatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DanhMucMon dmm = await db.DanhMucMons.FindAsync(id);
            db.DanhMucMons.Remove(dmm);
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
