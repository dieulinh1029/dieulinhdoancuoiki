using HOTPIZZA.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;

namespace HOTPIZZA.Controllers
{
    public class GioHangController : Controller
    {
        HOTPIZZAEntity db = new HOTPIZZAEntity();
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


       // Import namespace


        [HttpPost]
    public ActionResult DatHang(string tennguoinhan, string diachi, string phone, string payment, DateTime ngaygiao, Giohang dl)
    {
        NguoiDung nd = (NguoiDung)Session["user"];
        if (nd == null)
        {
            return RedirectToAction("DangNhap", "User");
        }

        List<Giohang> lstGioHang = Session["GIOHANG"] as List<Giohang>;
        if (lstGioHang == null || !lstGioHang.Any())
        {
            ModelState.AddModelError("", "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi đặt hàng.");
            return RedirectToAction("GioHang");
        }

        decimal totalValue = lstGioHang.Sum(item => item.DonGia * item.SoLuong).GetValueOrDefault();
        DonDatHang order = new DonDatHang
        {
            NgayDat = DateTime.Now,
            MaKH = nd.MaKhachHang,
            TenNguoiNhan = tennguoinhan,
            DiaChiNguoiNhan = diachi,
            DienThoaiNguoiNhan = phone,
            NgayGiao = ngaygiao,
            TinhTrangDonHang = 2,
            TriGia = totalValue,
            HinhThucThanhToan = payment == "cod" ? 1 : 2,
        };

        db.DonDatHangs.Add(order);
        db.SaveChanges();

        foreach (var item in lstGioHang)
        {
            MonAn monAn = db.MonAns.SingleOrDefault(m => m.IdMon == item.IdMon);
            if (monAn == null || monAn.IdDanhMuc == null || db.DanhMucMons.SingleOrDefault(c => c.IdDanhMuc == monAn.IdDanhMuc) == null)
            {
                ModelState.AddModelError("", $"Sản phẩm {item.IdMon} không hợp lệ.");
                return View(lstGioHang);
            }

            CTDonDatHang orderDetail = new CTDonDatHang
            {
                MaDon = order.MaDon,
                MaMon = item.IdMon,
                SoLuong = item.SoLuong,
                DonGia = item.DonGia
            };

            db.CTDonDatHangs.Add(orderDetail);
        }

        db.SaveChanges();
        Session["GIOHANG"] = null;

            

            return RedirectToAction("XacNhanDatHang", "Giohang");
    }
        // Xac Nhan Dat Hang (Order Confirmation)
        public ActionResult XacNhanDatHang()
        {
            ViewBag.Message = "Đặt hàng thành công!";
            return View();
        }

        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            // Tổng giá trị đơn hàng
            decimal subtotal = 0;
            List<Giohang> lstGioHang = Session["GIOHANG"] as List<Giohang>;
            if (lstGioHang == null || !lstGioHang.Any())
            {
                throw new Exception("Giỏ hàng trống!");
            }

            // Tính tổng giá trị đơn hàng (chuyển đổi sang USD nếu cần)
            subtotal = Math.Round((decimal)(lstGioHang.Sum(item => item.DonGia * item.SoLuong) / 25399), 2);

            // Tạo đối tượng Payer
            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            // Cấu hình URL chuyển hướng sau khi thanh toán
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Tính toán tổng tiền (chỉ cần sử dụng `subtotal` nếu không có tax/shipping)
            decimal tax = 0; // Nếu có thuế, thêm vào đây
            decimal shipping = 0; // Nếu có phí ship, thêm vào đây
            var amount = new Amount()
            {
                currency = "USD",
                total = Math.Round(subtotal + tax + shipping, 2).ToString(), // Tổng giá trị thanh toán
            };

            // Tạo transaction
            var transactionList = new List<Transaction>();
            var paypalOrderId = DateTime.Now.Ticks; // Tạo ID hóa đơn duy nhất
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), // Số hóa đơn
                amount = amount,
                item_list = null // Không cần liệt kê sản phẩm, chỉ hiển thị tổng giá trị đơn hàng
            });

            // Tạo đối tượng Payment
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Tạo thanh toán bằng APIContext
            return this.payment.Create(apiContext);
        }

    }
}