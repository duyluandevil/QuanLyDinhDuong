using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDinhDuong.Models;

namespace QuanLyDinhDuong.Controllers
{
    [System.Runtime.InteropServices.Guid("32DDE1B2-B387-49D2-9CCD-68A954870922")]
    public class AdminController : Controller
    {
        dbQlDDDataContext data = new dbQlDDDataContext();
        // GET: Admin
        public ActionResult Admin()
        {

            return View();
        }
        public ActionResult TrangCaNhan()
        {
            var tk = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == "duyluan0104" select tks).Single();
            return View(tk);
        }

        public ActionResult TaiKhoan()
        {
            var tk = (from tks in data.TAIKHOANs select tks).ToList();
            
            return View(tk);
        }

        public ActionResult BenhNhan()
        {
            var bn = (from bns in data.BENHNHANs select bns).ToList();
            
            return View(bn);
        }

        public ActionResult ThucPham()
        {
            return View();
        }

        public ActionResult ThucDon()
        {
            var td = (from tds in data.TAIKHOANs where tds.IDTAIKHOAN == "duyluan0104" select tds).ToList();
            return View(td);
            
        }
    }
}