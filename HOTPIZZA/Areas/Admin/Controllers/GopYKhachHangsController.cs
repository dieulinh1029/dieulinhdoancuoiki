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
    public class GopYKhachHangsController : Controller
    {
        private HOTPIZZAEntity db = new HOTPIZZAEntity();
        // GET: Admin/GopYKhachHangs
        public async Task<ActionResult> Index()
        {
            var gopy = await db.GopYKhachHangs.ToListAsync(); 
            return View(gopy);
        }

        // GET: Admin/GopYKhachHangs/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var gopY = await db.GopYKhachHangs.FindAsync(id);
            if (gopY == null)
            {
                return HttpNotFound();
            }
            return View(gopY);
        }


        
        // GET: Admin/GopYKhachHangs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/GopYKhachHangs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
