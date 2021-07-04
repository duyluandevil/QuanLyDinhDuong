using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDinhDuong.Models;

namespace QuanLyDinhDuong.Controllers
{
    public class ThucPhamController : Controller
    {
        // GET: ThucPham
        dbQlDDDataContext data = new dbQlDDDataContext();

        public ActionResult ThucPham()
        {
            return View();
        }

        public ActionResult LoaiThucPham()
        {
            var loaithucpham = from ltp in data.LOAITHUCPHAMs select ltp;
            return PartialView(loaithucpham);
        }

        public ActionResult TPTheoLoai(int id)
        {
            var thucpham = from t in data.THUCPHAMs where t.MALOAITHUCPHAM == id select t;
            return View(thucpham);
        }
    }
}