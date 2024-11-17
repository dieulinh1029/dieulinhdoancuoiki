using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HOTPIZZA.Controllers
{
    public class GioHangController : Controller
    {
        HOTPIZZAEntities1 db = new HOTPIZZAEntities1();
        public List<Giohang> LayGioHang()
        {
            List<Giohang> lstGioHang = Session["GIOHANG"] as List<Giohang>;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstGioHang = new List<Giohang>();
                Session["GIOHANG"] = lstGioHang;
            }
            return lstGioHang;
        }
        // GET: GioHang
        public ActionResult ThemGioHang(string maMon, string strURL)
        {
            MonAn mon = db.MonAns.SingleOrDefault(r => r.IdMon == maMon);
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Giohang> lstGioHang = LayGioHang();
            //Kiểm tra sp này đã tồn tại trong session[giohang] chưa
            Giohang sanpham = lstGioHang.Find(n => n.IdMon == maMon);
            if (sanpham == null)
            {
                sanpham = new Giohang(maMon);
                //Add sản phẩm mới thêm vào list
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.SoLuong++;
                return Redirect(strURL);
            }
        }
        public ActionResult CapNhatGioHang(string maMon, FormCollection f)
        {
            //Kiểm tra masp
            MonAn sp = db.MonAns.SingleOrDefault(n => n.IdMon == maMon);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Giohang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.IdMon == maMon);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.SoLuong = (short?)int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(string maMon)
        {
            //Kiểm tra masp
            MonAn sp = db.MonAns.SingleOrDefault(n => n.IdMon == maMon);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Giohang> lstGioHang = LayGioHang();
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.IdMon == maMon);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.IdMon == maMon);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("GioHang", "GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {

            if (Session["GIOHANG"] == null)
            {
                return RedirectToAction("Index", "DatHang");
            }
            List<Giohang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGioHang = Session["GIOHANG"] as List<Giohang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = (int)lstGioHang.Sum(n => n.SoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<Giohang> lstGioHang = Session["GIOHANG"] as List<Giohang>;
            if (lstGioHang != null)
            {
                dTongTien = (double)lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
        //tạo partial giỏ hàng

        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xây dựng 1 view cho người dùng chỉnh sửa giỏ hàng

        public ActionResult SuaGioHang()
        {
            if (Session["GIOHANG"] == null)
            {
                return RedirectToAction("Index", "DatHang");
            }
            List<Giohang> lstGioHang = LayGioHang();
            return View(lstGioHang);

        }
        public ActionResult DatHang()
            {
            return View();
            }
    }
}