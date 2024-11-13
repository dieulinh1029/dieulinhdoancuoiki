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
    public class TrangMiengsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/TrangMiengs
        public async Task<ActionResult> Index()
        {
            var trangMiengs = db.TrangMiengs.Include(t => t.DanhMucMon);
            return View(await trangMiengs.ToListAsync());
        }

        // GET: Admin/TrangMiengs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrangMieng trangMieng = await db.TrangMiengs.FindAsync(id);
            if (trangMieng == null)
            {
                return HttpNotFound();
            }
            return View(trangMieng);
        }

        // GET: Admin/TrangMiengs/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/TrangMiengs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdTrangMieng,TenTrangMieng,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] TrangMieng trangMieng)
        {
            if (ModelState.IsValid)
            {
                db.TrangMiengs.Add(trangMieng);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", trangMieng.IdDanhMuc);
            return View(trangMieng);
        }

        // GET: Admin/TrangMiengs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrangMieng trangMieng = await db.TrangMiengs.FindAsync(id);
            if (trangMieng == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", trangMieng.IdDanhMuc);
            return View(trangMieng);
        }

        // POST: Admin/TrangMiengs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdTrangMieng,TenTrangMieng,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] TrangMieng trangMieng)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trangMieng).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", trangMieng.IdDanhMuc);
            return View(trangMieng);
        }

        // GET: Admin/TrangMiengs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrangMieng trangMieng = await db.TrangMiengs.FindAsync(id);
            if (trangMieng == null)
            {
                return HttpNotFound();
            }
            return View(trangMieng);
        }

        // POST: Admin/TrangMiengs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            TrangMieng trangMieng = await db.TrangMiengs.FindAsync(id);
            db.TrangMiengs.Remove(trangMieng);
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
