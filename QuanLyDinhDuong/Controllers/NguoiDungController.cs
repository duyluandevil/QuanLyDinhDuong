using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDinhDuong.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        public ActionResult Dangnhap()
        {
            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }
    }
}