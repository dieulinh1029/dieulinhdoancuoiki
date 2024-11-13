using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HOTPIZZA.Models;
namespace HOTPIZZA.Controllers
{
    public class UserController : Controller
    {
        HOTPIZZAEntities1 db = new HOTPIZZAEntities1();
        // dang ky
        public ActionResult DangKy()
        {
            return View();
        }

        
        //dang nhap
    }
}