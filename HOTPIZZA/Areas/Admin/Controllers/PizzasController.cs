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
    public class PizzasController : Controller
    {
        private HOTPIZZAEntities1 db = new HOTPIZZAEntities1();

        // GET: Admin/Pizzas
        public async Task<ActionResult> Index()
        {
            var pizzas = db.Pizzas.Include(p => p.DanhMucMon);
            return View(await pizzas.ToListAsync());
        }

        // GET: Admin/Pizzas/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizza pizza = await db.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            return View(pizza);
        }

        // GET: Admin/Pizzas/Create
        public ActionResult Create()
        {
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/Pizzas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdPizza,TenPizza,IdDanhMuc,MoTa,HinhMinhHoa,Size,DonGia")] Pizza pizza)
        {
            if (ModelState.IsValid)
            {
                db.Pizzas.Add(pizza);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", pizza.IdDanhMuc);
            return View(pizza);
        }

        // GET: Admin/Pizzas/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizza pizza = await db.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", pizza.IdDanhMuc);
            return View(pizza);
        }

        // POST: Admin/Pizzas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdPizza,TenPizza,IdDanhMuc,MoTa,HinhMinhHoa,Size,DonGia")] Pizza pizza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pizza).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdDanhMuc = new SelectList(db.DanhMucMons, "IdDanhMuc", "TenDanhMuc", pizza.IdDanhMuc);
            return View(pizza);
        }

        // GET: Admin/Pizzas/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizza pizza = await db.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            return View(pizza);
        }

        // POST: Admin/Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Pizza pizza = await db.Pizzas.FindAsync(id);
            db.Pizzas.Remove(pizza);
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
