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
    public class TuyenDungsController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/TuyenDungs
        public async Task<ActionResult> Index()
        {
            var tuyenDungs = db.TuyenDungs.Include(t => t.Admin);
            return View(await tuyenDungs.ToListAsync());
        }

        // GET: Admin/TuyenDungs/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Admin/TuyenDungs/Create
        public ActionResult Create()
        {
            ViewBag.MaAdmin = new SelectList(db.Admins, "MaAdmin", "HovaTen");
            return View();
        }

        // POST: Admin/TuyenDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdBaiTuyenDung,DiaChiTuyenDung,ViTriLamViec,EmailLienLac,MoTacCongViec,MaAdmin")] TuyenDung tuyenDung)
        {
            if (ModelState.IsValid)
            {
                db.TuyenDungs.Add(tuyenDung);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaAdmin = new SelectList(db.Admins, "MaAdmin", "HovaTen", tuyenDung.MaAdmin);
            return View(tuyenDung);
        }

        // GET: Admin/TuyenDungs/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.MaAdmin = new SelectList(db.Admins, "MaAdmin", "HovaTen", tuyenDung.MaAdmin);
            return View(tuyenDung);
        }

        // POST: Admin/TuyenDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdBaiTuyenDung,DiaChiTuyenDung,ViTriLamViec,EmailLienLac,MoTacCongViec,MaAdmin")] TuyenDung tuyenDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuyenDung).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaAdmin = new SelectList(db.Admins, "MaAdmin", "HovaTen", tuyenDung.MaAdmin);
            return View(tuyenDung);
        }

        // GET: Admin/TuyenDungs/Delete/5
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
