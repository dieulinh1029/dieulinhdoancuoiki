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
    public class DonDatHangsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/DonDatHangs
        public async Task<ActionResult> Index()
        {
            var donDatHangs = db.DonDatHangs.Include(d => d.CTDonDatHang).Include(d => d.NguoiDung).Include(d => d.TinhTrangDonHang1).Include(d => d.HinhThucThanhToan1);
            return View(await donDatHangs.ToListAsync());
        }

        // GET: Admin/DonDatHangs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = await db.DonDatHangs.FindAsync(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            return View(donDatHang);
        }

        // GET: Admin/DonDatHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaDon = new SelectList(db.CTDonDatHangs, "MaDon", "MaMon");
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKhachHang", "HovaTen");
            ViewBag.TinhTrangDonHang = new SelectList(db.TinhTrangDonHangs, "IdTTDH", "tenTTDH");
            ViewBag.HinhThucThanhToan = new SelectList(db.HinhThucThanhToans, "idHTTT", "tenHTTT");
            return View();
        }

        // POST: Admin/DonDatHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaDon,MaKH,NgayDat,TriGia,NgayGiao,TenNguoiNhan,DiaChiNguoiNhan,DienThoaiNguoiNhan,TinhTrangDonHang,HinhThucThanhToan")] DonDatHang donDatHang)
        {
            if (ModelState.IsValid)
            {
                db.DonDatHangs.Add(donDatHang);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaDon = new SelectList(db.CTDonDatHangs, "MaDon", "MaMon", donDatHang.MaDon);
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKhachHang", "HovaTen", donDatHang.MaKH);
            ViewBag.TinhTrangDonHang = new SelectList(db.TinhTrangDonHangs, "IdTTDH", "tenTTDH", donDatHang.TinhTrangDonHang);
            ViewBag.HinhThucThanhToan = new SelectList(db.HinhThucThanhToans, "idHTTT", "tenHTTT", donDatHang.HinhThucThanhToan);
            return View(donDatHang);
        }

        // GET: Admin/DonDatHangs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = await db.DonDatHangs.FindAsync(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDon = new SelectList(db.CTDonDatHangs, "MaDon", "MaMon", donDatHang.MaDon);
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKhachHang", "HovaTen", donDatHang.MaKH);
            ViewBag.TinhTrangDonHang = new SelectList(db.TinhTrangDonHangs, "IdTTDH", "tenTTDH", donDatHang.TinhTrangDonHang);
            ViewBag.HinhThucThanhToan = new SelectList(db.HinhThucThanhToans, "idHTTT", "tenHTTT", donDatHang.HinhThucThanhToan);
            return View(donDatHang);
        }

        // POST: Admin/DonDatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDon,MaKH,NgayDat,TriGia,NgayGiao,TenNguoiNhan,DiaChiNguoiNhan,DienThoaiNguoiNhan,TinhTrangDonHang,HinhThucThanhToan")] DonDatHang donDatHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donDatHang).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaDon = new SelectList(db.CTDonDatHangs, "MaDon", "MaMon", donDatHang.MaDon);
            ViewBag.MaKH = new SelectList(db.NguoiDungs, "MaKhachHang", "HovaTen", donDatHang.MaKH);
            ViewBag.TinhTrangDonHang = new SelectList(db.TinhTrangDonHangs, "IdTTDH", "tenTTDH", donDatHang.TinhTrangDonHang);
            ViewBag.HinhThucThanhToan = new SelectList(db.HinhThucThanhToans, "idHTTT", "tenHTTT", donDatHang.HinhThucThanhToan);
            return View(donDatHang);
        }

        // GET: Admin/DonDatHangs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = await db.DonDatHangs.FindAsync(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            return View(donDatHang);
        }

        // POST: Admin/DonDatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DonDatHang donDatHang = await db.DonDatHangs.FindAsync(id);
            db.DonDatHangs.Remove(donDatHang);
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
