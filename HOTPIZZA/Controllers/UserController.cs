using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HOTPIZZA.Models;
namespace HOTPIZZA.Controllers
{
    public class UserController : Controller
    {
        NguoiDung _nd;
        HOTPIZZAEntity db = new HOTPIZZAEntity();
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
        public ActionResult DangKy(NguoiDung khh, DateTime ngaysinh)
        {
            if (Session["user"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(khh);
            }

            NguoiDung newUser = new NguoiDung
            {
                HovaTen = khh.HovaTen,
                NgaySinh = ngaysinh,
                Phone = khh.Phone,
                Email = khh.Email,
                TenDN = khh.TenDN,
                MatKhau = khh.MatKhau, // Mã hóa mật khẩu
                IdPhanQuyen = 1
            };

            try
            {
                db.NguoiDungs.Add(newUser);
                db.SaveChanges();
                Session["user"] = newUser; // Đăng nhập tự động sau khi đăng ký thành công
                return RedirectToAction("Index", "Home");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return View(khh);
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