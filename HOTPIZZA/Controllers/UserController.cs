using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HOTPIZZA.Models;
namespace HOTPIZZA.Controllers
{
    public class UserController : Controller
    {
        NguoiDung _nd;
        HOTPIZZAEntities1 db = new HOTPIZZAEntities1();
        public ActionResult Index()
        {
            return View();
        }
        // dang ky
        public ActionResult DangKy()
        {
            _nd = (NguoiDung)Session["user"];
            if (_nd != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NguoiDung khh)
        {
            _nd = (NguoiDung)Session["user"];
            if (_nd != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(khh);
            }
            _nd = new NguoiDung();
            _nd.HovaTen = khh.HovaTen;
            _nd.NgaySinh = khh.NgaySinh;
            _nd.Phone = khh.Phone;
            _nd.Email = khh.Email;
            _nd.TenDN = khh.TenDN;
            _nd.MatKhau = khh.MatKhau;

            db.NguoiDungs.Add(_nd);
            db.SaveChanges();
            Session.Add("user", _nd);
            return RedirectToAction("Index", "Home");
        }

        //dang nhap
        public ActionResult DangNhap()
        {
            _nd = (NguoiDung)Session["user"];
            if (_nd != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(Login lg)
        {
            _nd = (NguoiDung)Session["user"];
            
            if (_nd != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                _nd = db.NguoiDungs.SingleOrDefault(n => n.TenDN == lg.TenDN && n.MatKhau == lg.MatKhau);
               
                if (_nd != null)
                {
                    Session.Add("user", _nd);
                    if (_nd.IdPhanQuyen == 2)
                    {
                        return RedirectToAction("Index", "DonDatHangs", new { area = "Admin" });
                    }
                    else 
                    { 
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.Fail = "Đăng nhập thất bại";
                return View("DangNhap");
            }
            return View(lg);
        }
        public ActionResult DangXuat()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");

        }
    }
}