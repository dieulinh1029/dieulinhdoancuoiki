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
    public class MonAnKemsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/MonAnKems
        public async Task<ActionResult> Index()
        {
            var monAnKems = db.MonAnKems.Include(m => m.DanhMucMon);
            return View(await monAnKems.ToListAsync());
        }

        // GET: Admin/MonAnKems/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAnKem monAnKem = await db.MonAnKems.FindAsync(id);
            if (monAnKem == null)
            {
                return HttpNotFound();
            }
            return View(monAnKem);
        }

        // GET: Admin/MonAnKems/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/MonAnKems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdMonAnKem,TenMonAnKem,IdDanhMuc,HinhMinhHoa,DonGia")] MonAnKem monAnKem)
        {
            if (ModelState.IsValid)
            {
                db.MonAnKems.Add(monAnKem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAnKem.IdDanhMuc);
            return View(monAnKem);
        }

        // GET: Admin/MonAnKems/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAnKem monAnKem = await db.MonAnKems.FindAsync(id);
            if (monAnKem == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAnKem.IdDanhMuc);
            return View(monAnKem);
        }

        // POST: Admin/MonAnKems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdMonAnKem,TenMonAnKem,IdDanhMuc,HinhMinhHoa,DonGia")] MonAnKem monAnKem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monAnKem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAnKem.IdDanhMuc);
            return View(monAnKem);
        }

        // GET: Admin/MonAnKems/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAnKem monAnKem = await db.MonAnKems.FindAsync(id);
            if (monAnKem == null)
            {
                return HttpNotFound();
            }
            return View(monAnKem);
        }

        // POST: Admin/MonAnKems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            MonAnKem monAnKem = await db.MonAnKems.FindAsync(id);
            db.MonAnKems.Remove(monAnKem);
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
