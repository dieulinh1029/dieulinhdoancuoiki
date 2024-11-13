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
    public class ThucUongsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/ThucUongs
        public async Task<ActionResult> Index()
        {
            var thucUongs = db.ThucUongs.Include(t => t.DanhMucMon);
            return View(await thucUongs.ToListAsync());
        }

        // GET: Admin/ThucUongs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThucUong thucUong = await db.ThucUongs.FindAsync(id);
            if (thucUong == null)
            {
                return HttpNotFound();
            }
            return View(thucUong);
        }

        // GET: Admin/ThucUongs/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/ThucUongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdThucUong,TenThuocUong,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] ThucUong thucUong)
        {
            if (ModelState.IsValid)
            {
                db.ThucUongs.Add(thucUong);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", thucUong.IdDanhMuc);
            return View(thucUong);
        }

        // GET: Admin/ThucUongs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThucUong thucUong = await db.ThucUongs.FindAsync(id);
            if (thucUong == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", thucUong.IdDanhMuc);
            return View(thucUong);
        }

        // POST: Admin/ThucUongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdThucUong,TenThuocUong,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] ThucUong thucUong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thucUong).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", thucUong.IdDanhMuc);
            return View(thucUong);
        }

        // GET: Admin/ThucUongs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThucUong thucUong = await db.ThucUongs.FindAsync(id);
            if (thucUong == null)
            {
                return HttpNotFound();
            }
            return View(thucUong);
        }

        // POST: Admin/ThucUongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ThucUong thucUong = await db.ThucUongs.FindAsync(id);
            db.ThucUongs.Remove(thucUong);
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
