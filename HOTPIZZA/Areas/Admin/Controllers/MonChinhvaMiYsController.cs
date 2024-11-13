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
    public class MonChinhvaMiYsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/MonChinhvaMiYs
        public async Task<ActionResult> Index()
        {
            var monChinhvaMiYs = db.MonChinhvaMiYs.Include(m => m.DanhMucMon);
            return View(await monChinhvaMiYs.ToListAsync());
        }

        // GET: Admin/MonChinhvaMiYs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonChinhvaMiY monChinhvaMiY = await db.MonChinhvaMiYs.FindAsync(id);
            if (monChinhvaMiY == null)
            {
                return HttpNotFound();
            }
            return View(monChinhvaMiY);
        }

        // GET: Admin/MonChinhvaMiYs/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/MonChinhvaMiYs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdMonChinhvaMiY,TenMonChinhvaMiY,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] MonChinhvaMiY monChinhvaMiY)
        {
            if (ModelState.IsValid)
            {
                db.MonChinhvaMiYs.Add(monChinhvaMiY);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monChinhvaMiY.IdDanhMuc);
            return View(monChinhvaMiY);
        }

        // GET: Admin/MonChinhvaMiYs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonChinhvaMiY monChinhvaMiY = await db.MonChinhvaMiYs.FindAsync(id);
            if (monChinhvaMiY == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monChinhvaMiY.IdDanhMuc);
            return View(monChinhvaMiY);
        }

        // POST: Admin/MonChinhvaMiYs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdMonChinhvaMiY,TenMonChinhvaMiY,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] MonChinhvaMiY monChinhvaMiY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monChinhvaMiY).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monChinhvaMiY.IdDanhMuc);
            return View(monChinhvaMiY);
        }

        // GET: Admin/MonChinhvaMiYs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonChinhvaMiY monChinhvaMiY = await db.MonChinhvaMiYs.FindAsync(id);
            if (monChinhvaMiY == null)
            {
                return HttpNotFound();
            }
            return View(monChinhvaMiY);
        }

        // POST: Admin/MonChinhvaMiYs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            MonChinhvaMiY monChinhvaMiY = await db.MonChinhvaMiYs.FindAsync(id);
            db.MonChinhvaMiYs.Remove(monChinhvaMiY);
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
