using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HOTPIZZA.Models;
using System.Data.Entity;
using System.Net;
namespace HOTPIZZA.Controllers
{
    public class XuatDonController : Controller
    {
        private HOTPIZZAEntity db = new HOTPIZZAEntity();
        public ActionResult Index()
        {
            //Kiểm tra đang đăng nhập
            if (Session["user"] == null || Session["user"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            NguoiDung kh = (NguoiDung)Session["user"];
            int maND = kh.MaKhachHang;
            var donhangs = db.DonDatHangs.Include(d => d.NguoiDung).Where(d => d.MaKH == maND);
            return View(donhangs.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donhang = db.DonDatHangs.Find(id);
            var chitiet = db.CTDonDatHangs.Include(d => d.MonAn).Where(d => d.MaDon == id).ToList();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(chitiet);
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
