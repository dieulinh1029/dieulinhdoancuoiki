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
                // Pass available categories (DanhMucMon) to the view for the dropdown
                ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
                return View();
            }

        // POST: MonAns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TenMon, HinhMinhHoa, DonGia, MoTa, IdDanhMuc")] MonAn monAn, HttpPostedFileBase file)
        {
            // Validate the IdDanhMuc field
            if (string.IsNullOrEmpty(monAn.IdDanhMuc))
            {
                ModelState.AddModelError("IdDanhMuc", "Danh Mục is required");
            }

            // File upload validation
            if (file != null && file.ContentLength > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("HinhMinhHoa", "Invalid file type. Please upload an image.");
                }
                else
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/images/Menu"), fileName);
                    file.SaveAs(filePath);
                    monAn.HinhMinhHoa = fileName;
                }
            }
            else
            {
                ModelState.AddModelError("HinhMinhHoa", "Please upload an image.");
            }

            // Check for model state errors
            if (ModelState.IsValid)
            {
                db.MonAns.Add(monAn);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                // If validation fails, log errors
                var validationErrors = db.GetValidationErrors();
                foreach (var validationError in validationErrors)
                {
                    foreach (var error in validationError.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
                    }
                }
            }

            // Re-populate dropdown in case of errors
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", monAn.IdDanhMuc);
            return View(monAn);
        }



        // GET: Admin/MonAns/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/MonAns/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/MonAns/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuyenDung tuyenDung = await db.TuyenDungs.FindAsync(id);
            if (tuyenDung == null)
            {
                return HttpNotFound();
            }
            return View(tuyenDung);
        }

        // POST: Admin/TuyenDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TuyenDung tuyenDung = await db.TuyenDungs.FindAsync(id);
            db.TuyenDungs.Remove(tuyenDung);
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
