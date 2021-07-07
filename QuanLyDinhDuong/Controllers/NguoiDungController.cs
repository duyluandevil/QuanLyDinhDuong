using QuanLyDinhDuong.Models;
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
        dbQlDDDataContext data = new dbQlDDDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        //public ActionResult Dangnhap(FormCollection collection,TAIKHOAN tk)
        //{

        //    return this.Dangnhap();
        //}
    }
}