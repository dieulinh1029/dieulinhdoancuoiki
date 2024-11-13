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
    public class KhaiVivaSaladsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/KhaiVivaSalads
        public async Task<ActionResult> Index()
        {
            var khaiVivaSalads = db.KhaiVivaSalads.Include(k => k.DanhMucMon);
            return View(await khaiVivaSalads.ToListAsync());
        }

        // GET: Admin/KhaiVivaSalads/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhaiVivaSalad khaiVivaSalad = await db.KhaiVivaSalads.FindAsync(id);
            if (khaiVivaSalad == null)
            {
                return HttpNotFound();
            }
            return View(khaiVivaSalad);
        }

        // GET: Admin/KhaiVivaSalads/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/KhaiVivaSalads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdKhaiVivaSalad,TenKhaiVivaSalad,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] KhaiVivaSalad khaiVivaSalad)
        {
            if (ModelState.IsValid)
            {
                db.KhaiVivaSalads.Add(khaiVivaSalad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", khaiVivaSalad.IdDanhMuc);
            return View(khaiVivaSalad);
        }

        // GET: Admin/KhaiVivaSalads/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhaiVivaSalad khaiVivaSalad = await db.KhaiVivaSalads.FindAsync(id);
            if (khaiVivaSalad == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", khaiVivaSalad.IdDanhMuc);
            return View(khaiVivaSalad);
        }

        // POST: Admin/KhaiVivaSalads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdKhaiVivaSalad,TenKhaiVivaSalad,IdDanhMuc,HinhMinhHoa,MoTa,DonGia")] KhaiVivaSalad khaiVivaSalad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khaiVivaSalad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", khaiVivaSalad.IdDanhMuc);
            return View(khaiVivaSalad);
        }

        // GET: Admin/KhaiVivaSalads/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhaiVivaSalad khaiVivaSalad = await db.KhaiVivaSalads.FindAsync(id);
            if (khaiVivaSalad == null)
            {
                return HttpNotFound();
            }
            return View(khaiVivaSalad);
        }

        // POST: Admin/KhaiVivaSalads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            KhaiVivaSalad khaiVivaSalad = await db.KhaiVivaSalads.FindAsync(id);
            db.KhaiVivaSalads.Remove(khaiVivaSalad);
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
