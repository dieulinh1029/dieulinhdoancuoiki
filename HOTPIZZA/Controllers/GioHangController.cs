using HOTPIZZA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
        public ActionResult ThemGioHang(string IdMon, string strURL)
        {
            MonAn mon = db.MonAns.SingleOrDefault(r => r.IdMon == IdMon);
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Giohang> lstGioHang = LayGioHang();
            //Kiểm tra sp này đã tồn tại trong session[giohang] chưa
            Giohang sanpham = lstGioHang.Find(n => n.IdMon == IdMon);
            if (sanpham == null)
            {
                sanpham = new Giohang(IdMon);
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
        public ActionResult CapNhatGioHang(string IdMon, FormCollection f)
        {
            //Kiểm tra masp
            MonAn sp = db.MonAns.SingleOrDefault(n => n.IdMon == IdMon);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Giohang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.IdMon == IdMon);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.SoLuong = (short?)int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(string IdMon)
        {
            //Kiểm tra masp
            MonAn sp = db.MonAns.SingleOrDefault(n => n.IdMon == IdMon);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Giohang> lstGioHang = LayGioHang();
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.IdMon == IdMon);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.IdMon == IdMon);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("GioHang", "GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {

            //if (Session["GIOHANG"] == null)
            //{
            //    return RedirectToAction("Index", "DatHang");
            //}
            List<Giohang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult DonHang()   
        {
            NguoiDung nd = (NguoiDung)Session["user"];
            if ((NguoiDung)Session["user"] == null)
            {
                return RedirectToAction("DangNhap", "User");
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


        [HttpPost]
        public ActionResult DatHang(string tennguoinhan,string diachi, string phone,string payment, DateTime ngaygiao)
        {
            // Get current user's information from session
            NguoiDung nd = (NguoiDung)Session["user"];
            if (nd == null)
            {
                // If the user is not logged in, redirect to the login page
                return RedirectToAction("DangNhap", "User");
            }
            List<Giohang> lstGioHang = Session["GIOHANG"] as List<Giohang>;
            if (lstGioHang == null || !lstGioHang.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi đặt hàng.");
                return RedirectToAction("GioHang");
            }
            // Create an order object
            DonDatHang order = new DonDatHang
            {
                NgayDat = DateTime.Now,
                MaKH = nd.MaKhachHang,  // Use MaKhachHang as the foreign key
                TenNguoiNhan = tennguoinhan,
                DiaChiNguoiNhan = diachi,
                DienThoaiNguoiNhan = phone,
                NgayGiao = ngaygiao,
                TinhTrangDonHang = 1,
                HinhThucThanhToan = payment == "cod" ? 1 : 2, // Assuming 1 is COD, 2 is card
            };
            decimal totalValue = lstGioHang.Sum(item => item.DonGia * item.SoLuong).GetValueOrDefault();

            // Add order to the database
            db.DonDatHangs.Add(order);
            db.SaveChanges(); // Save to generate the MaDon (order ID)
            ViewBag.TotalValue = totalValue;
            // Create order details (cart items)

            foreach (var item in lstGioHang)
            {
                // Ensure valid item exists in the database
                MonAn monAn = db.MonAns.SingleOrDefault(m => m.IdMon == item.IdMon);
                if (monAn == null)
                {
                    ModelState.AddModelError("", $"IdMon {item.IdMon} khong tồn tại..");
                    return View(lstGioHang); // Return with error message if product is invalid
                }

                // Check if the product has a valid category
                if (monAn.IdDanhMuc == null)
                {
                    ModelState.AddModelError("", $"IDmon {item.IdMon} k khả zụng.");
                    return View(lstGioHang); // Return with error message if category is invalid
                }

                // Ensure that the category exists in the DanhMucMon table
                var category = db.DanhMucMons.SingleOrDefault(c => c.IdDanhMuc == monAn.IdDanhMuc);
                if (category == null)
                {
                    ModelState.AddModelError("", $"Danh mục IdMon  {item.IdMon} khong tồn tại.");
                    return View(lstGioHang); // Return with error message if category does not exist
                }

                // Create and add order details
                CTDonDatHang orderDetail = new CTDonDatHang
                {
                    MaDon = order.MaDon,  // Link order with MaDon
                    MaMon = item.IdMon,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia
                };

                db.CTDonDatHangs.Add(orderDetail);
            }

            // Save order details
            db.SaveChanges();

            // Clear the cart (session)
            Session["GIOHANG"] = null;

            // Redirect to the order confirmation page
            return RedirectToAction("XacNhanDatHang", "Giohang");
        }




        // Xac Nhan Dat Hang (Order Confirmation)
        public ActionResult XacNhanDatHang()
        {
            ViewBag.Message = "Đặt hàng thành công!";
            return View();
        }



    }
}