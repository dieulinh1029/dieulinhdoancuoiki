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
    public class NguoiDungsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/NguoiDungs
        public async Task<ActionResult> Index()
        {
            var nguoiDungs = db.NguoiDungs.Include(n => n.Login).Include(n => n.PhanQuyen);
            return View(await nguoiDungs.ToListAsync());
        }

        // GET: Admin/NguoiDungs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // GET: Admin/NguoiDungs/Create
        public ActionResult Create()
        {
            ViewBag.TenDN = new SelectList(db.Logins, "TenDN", "MatKhau");
            ViewBag.IdPhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen");
            return View();
        }

        // POST: Admin/NguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaKhachHang,HovaTen,Email,Phone,NgaySinh,TenDN,MatKhau,IdPhanQuyen")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.NguoiDungs.Add(nguoiDung);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TenDN = new SelectList(db.Logins, "TenDN", "MatKhau", nguoiDung.TenDN);
            ViewBag.IdPhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen", nguoiDung.IdPhanQuyen);
            return View(nguoiDung);
        }

        // GET: Admin/NguoiDungs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenDN = new SelectList(db.Logins, "TenDN", "MatKhau", nguoiDung.TenDN);
            ViewBag.IdPhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen", nguoiDung.IdPhanQuyen);
            return View(nguoiDung);
        }

        // POST: Admin/NguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaKhachHang,HovaTen,Email,Phone,NgaySinh,TenDN,MatKhau,IdPhanQuyen")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiDung).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TenDN = new SelectList(db.Logins, "TenDN", "MatKhau", nguoiDung.TenDN);
            ViewBag.IdPhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen", nguoiDung.IdPhanQuyen);
            return View(nguoiDung);
        }

        // GET: Admin/NguoiDungs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: Admin/NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            db.NguoiDungs.Remove(nguoiDung);
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
