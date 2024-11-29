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
            ViewBag.MaDon = new SelectList(db.DonDatHangs, "MaDon", "MaKH");
            ViewBag.GopYKhachHang = new SelectList(db.GopYKhachHangs, "IdGopY", "IdKH");
            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen");
            return View();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaKhachHang,HovaTen,Email,Phone,NgaySinh,TenDN,MatKhau,IdPhanQuyen")] NguoiDung nd)
        {
            if (ModelState.IsValid)
            {
                db.NguoiDungs.Add(nd);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaDon = new SelectList(db.DonDatHangs, "MaDon", "MaKH");
            ViewBag.GopYKhachHang = new SelectList(db.GopYKhachHangs, "IdGopY", "IdKH");
            ViewBag.IdPhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen");

            return View(nd);
        }
        // GET: Admin/NguoiDungs/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.MaDon = new SelectList(db.DonDatHangs, "MaDon", "MaKH");
            ViewBag.GopYKhachHang = new SelectList(db.GopYKhachHangs, "IdGopY", "IdKH");
            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen");
            return View(nd);
        }

        
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaKhachHang,HovaTen,Email,Phone,NgaySinh,TenDN,MatKhau,IdPhanQuyen")] NguoiDung nd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nd).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaDon = new SelectList(db.DonDatHangs, "MaDon", "MaKH");
            ViewBag.GopYKhachHang = new SelectList(db.GopYKhachHangs, "IdGopY", "IdKH");
            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "IdPhanQuyen", "TenPhanQuyen");
            return View(nd);
        }


        // GET: Admin/NguoiDungs/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           NguoiDung nd = await db.NguoiDungs.FindAsync(id);
            db.NguoiDungs.Remove(nd);
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
