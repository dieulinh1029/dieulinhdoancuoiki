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
using System.IO;
using Ganss.XSS;
using System.Data.Entity.Validation;
using PaypalServerSdk.Standard.Models;

namespace HOTPIZZA.Areas.Admin.Controllers
{
    public class MonAnsController : Controller
    {
        HOTPIZZAEntity db = new HOTPIZZAEntity();
        // GET: Admin/MonAns
        public async Task<ActionResult> Index()
        {
            var man = db.MonAns.Include(t => t.DanhMucMon);
            return View(await man.ToListAsync());
        }

        // GET: Admin/MonAns/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAn man = await db.MonAns.Include(m => m.DanhMucMon)
                                 .FirstOrDefaultAsync(m => m.IdMon == id);
            if (man == null)
            {
                return HttpNotFound();
            }
            return View(man);
        }


        // GET: Admin/MonAns/Create

        public ActionResult Create()
        {
            // Load danh sách danh mục món ăn
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: MonAns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TenMon, DonGia, MoTa, IdDanhMuc")] MonAn monAn, HttpPostedFileBase file)
        {
            // Kiểm tra danh mục
            if (string.IsNullOrEmpty(monAn.IdDanhMuc))
            {
                ModelState.AddModelError("IdDanhMuc", "Vui lòng chọn danh mục.");
            }

            // Xử lý file upload
            if (file != null && file.ContentLength > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("HinhMinhHoa", "Chỉ chấp nhận các định dạng ảnh: .jpg, .jpeg, .png, .gif.");
                }
                else
                {
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/images/Menu"), fileName);

                    if (!Directory.Exists(Server.MapPath("~/images/Menu")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/images/Menu"));
                    }

                    file.SaveAs(path);
                    monAn.HinhMinhHoa = fileName;
                }
            }
            else
            {
                ModelState.AddModelError("HinhMinhHoa", "Vui lòng tải lên hình ảnh.");
            }

            // Kiểm tra nếu có lỗi thì return lại view với dữ liệu cũ
            if (!ModelState.IsValid)
            {
                ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAn.IdDanhMuc);
                return View(monAn);
            }

            // Lưu vào CSDL nếu hợp lệ
            db.MonAns.Add(monAn);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // GET: Admin/MonAns/Edit/5
        // GET: MonAns/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var monAn = await db.MonAns.FindAsync(id);
            if (monAn == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAn.IdDanhMuc);
            return View(monAn);
        }

        // POST: MonAns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, TenMon, DonGia, MoTa, IdDanhMuc")] MonAn monAn, HttpPostedFileBase file)
        {
            // Kiểm tra danh mục
            if (string.IsNullOrEmpty(monAn.IdDanhMuc))
            {
                ModelState.AddModelError("IdDanhMuc", "Vui lòng chọn danh mục.");
            }

            // Xử lý file upload
            if (file != null && file.ContentLength > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("HinhMinhHoa", "Chỉ chấp nhận các định dạng ảnh: .jpg, .jpeg, .png, .gif.");
                }
                else
                {
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/images/Menu"), fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(Server.MapPath("~/images/Menu")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/images/Menu"));
                    }

                    file.SaveAs(path);
                    monAn.HinhMinhHoa = fileName;
                }
            }
            else
            {
                // Giữ nguyên hình ảnh cũ nếu không upload ảnh mới
                var existingMonAn = db.MonAns.AsNoTracking().FirstOrDefault(m => m.IdMon == monAn.IdMon);
                monAn.HinhMinhHoa = existingMonAn?.HinhMinhHoa;
            }

            // Kiểm tra và lưu dữ liệu nếu hợp lệ
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(monAn).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var error in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            // Nếu có lỗi, hiển thị lại view với dữ liệu đã nhập
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAn.IdDanhMuc);
            return View(monAn);
        }


        // GET: Admin/MonAns/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAn monan = await db.MonAns.FindAsync(id);
            if (monan == null)
            {
                return HttpNotFound();
            }
            return View(monan);
        }

        // POST: Admin/TuyenDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            MonAn monan = await db.MonAns.FindAsync(id);
            db.MonAns.Remove(monan);
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
