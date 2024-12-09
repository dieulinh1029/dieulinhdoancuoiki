using HOTPIZZA.Models.payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

public class PaymentController : Controller
{
    // Kiểm tra thanh toán trước khi gửi yêu cầu
    public ActionResult CreatePayment(Dictionary<string, string> paymentData)
    {
        decimal amount = Convert.ToDecimal(paymentData["amount"]);
        string transactionDate = paymentData["transactionDate"];

        // Kiểm tra dữ liệu trước khi gửi đến VNPAY
        if (IsValidAmount(amount) && IsValidTransactionDate(transactionDate))
        {
            // Dữ liệu hợp lệ, gọi hàm tạo URL thanh toán
            return RedirectToAction("CreatePaymentUrl", new { amount = amount });
        }
        else
        {
            // Dữ liệu không hợp lệ
            ViewBag.Message = "Dữ liệu thanh toán không hợp lệ!";
            return View();
        }
    }

    // Kiểm tra tổng tiền có phải số dương
    private bool IsValidAmount(decimal amount)
    {
        return amount > 0;
    }

    // Kiểm tra định dạng ngày tháng (VD: yyyyMMddHHmmss)
    private bool IsValidTransactionDate(string transactionDate)
    {
        DateTime temp;
        return DateTime.TryParseExact(transactionDate, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out temp);
    }

    // Action tạo URL thanh toán
    public ActionResult CreatePaymentUrl(decimal amount)
    {
        // Lấy thông tin cấu hình từ web.config
        string vnp_ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["VnpayReturnUrl"];
        string vnp_Url = System.Configuration.ConfigurationManager.AppSettings["VnpayUrl"];
        string vnp_TmnCode = System.Configuration.ConfigurationManager.AppSettings["VnpayTmnCode"];
        string vnp_HashSecret = System.Configuration.ConfigurationManager.AppSettings["VnpayHashSecret"];

        // Khởi tạo đối tượng VnPayLibrary
        VnPayLibrary vnpay = new VnPayLibrary();

        // Thông tin giao dịch
        string orderId = DateTime.Now.Ticks.ToString(); // Tạo ID đơn hàng từ thời gian
        string ipAddr = Request.ServerVariables["REMOTE_ADDR"] ?? "127.0.0.1"; // IP người dùng

        // Thêm dữ liệu giao dịch vào request
        vnpay.AddRequestData("vnp_Version", "2.1.0");
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString()); // Nhân 100 để quy đổi sang đồng VND
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_TxnRef", orderId);
        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng #" + orderId);
        vnpay.AddRequestData("vnp_Locale", "vn"); // Ngôn ngữ
        vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
        vnpay.AddRequestData("vnp_IpAddr", ipAddr);

        // Tạo URL thanh toán
        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

        // Chuyển hướng đến URL thanh toán
        return Redirect(paymentUrl);
    }

    // Hàm mã hóa SHA-512
    private string HmacSHA512(string key, string inputData)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        using (var hmac = new System.Security.Cryptography.HMACSHA512(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    // Action nhận phản hồi từ VNPAY
    public ActionResult Return()
    {
        var vnp_Params = Request.QueryString;
        string vnp_HashSecret = System.Configuration.ConfigurationManager.AppSettings["VnpayHashSecret"];

        // Lấy và kiểm tra chữ ký
        var vnp_SecureHash = vnp_Params["vnp_SecureHash"];
        var inputData = vnp_Params.AllKeys
            .Where(k => k != "vnp_SecureHash")
            .OrderBy(k => k)
            .Select(k => $"{k}={vnp_Params[k]}")
            .ToList();
        string rawData = string.Join("&", inputData);
        string computedHash = HmacSHA512(vnp_HashSecret, rawData);

        // Kiểm tra chữ ký hợp lệ
        if (computedHash == vnp_SecureHash)
        {
            if (vnp_Params["vnp_ResponseCode"] == "00")
            {
                ViewBag.Message = "Thanh toán thành công!";
            }
            else
            {
                ViewBag.Message = $"Thanh toán thất bại! Mã lỗi: {vnp_Params["vnp_ResponseCode"]}";
            }
        }
        else
        {
            ViewBag.Message = "Chữ ký không hợp lệ!";
        }

        return View();
    }

    // Khi nhận phản hồi từ VNPAY (dùng cho webhook hoặc API)
    [HttpPost]
    public ActionResult PaymentResponse(Dictionary<string, string> responseParameters)
    {
        string receivedSignature = responseParameters["vnp_SecureHash"];

        // Kiểm tra chữ ký
        if (CheckSignature(responseParameters, receivedSignature))
        {
            // Chữ ký hợp lệ, xử lý thanh toán
            if (responseParameters["vnp_ResponseCode"] == "00")
            {
                // Thành công, xử lý đơn hàng
                ViewBag.Message = "Thanh toán thành công!";
            }
            else
            {
                // Thanh toán thất bại, xử lý lỗi
                ViewBag.Message = $"Thanh toán thất bại, mã lỗi: {responseParameters["vnp_ResponseCode"]}";
            }
        }
        else
        {
            // Chữ ký không hợp lệ
            ViewBag.Message = "Chữ ký không hợp lệ!";
        }

        return View();
    }

    // Kiểm tra chữ ký
    private bool CheckSignature(Dictionary<string, string> parameters, string receivedSignature)
    {
        string signatureData = CreateSignatureData(parameters);
        string calculatedSignature = CalculateSignature(signatureData);

        return calculatedSignature.Equals(receivedSignature, StringComparison.InvariantCultureIgnoreCase);
    }

    // Tạo chuỗi dữ liệu cho chữ ký
    private string CreateSignatureData(Dictionary<string, string> parameters)
    {
        var sb = new StringBuilder();
        foreach (var param in parameters)
        {
            sb.Append(param.Key).Append("=").Append(param.Value).Append("&");
        }
        sb.Remove(sb.Length - 1, 1); // Xóa dấu "&" thừa ở cuối
        return sb.ToString();
    }

    // Tính chữ ký từ chuỗi dữ liệu
    private string CalculateSignature(string data)
    {
        using (var sha256 = new System.Security.Cryptography.SHA256Managed())
        {
            string dataWithSecret = data + "&vnp_SecureHashSecret=" + "YourVNPAYSecretKey"; // Secret key từ cấu hình
            byte[] hashData = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataWithSecret));
            return BitConverter.ToString(hashData).Replace("-", "").ToLower();
        }
    }
}
