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
            var tP = (from tPs in data.THUCPHAMs select tPs).ToList();

            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;
            return View(tP);
        }
        [HttpPost]
        public ActionResult ThucPham(FormCollection collection)
        {
            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;

            List<THUCPHAM> tP = (from tPs in data.THUCPHAMs select tPs).ToList();
            int l = Int32.Parse(collection["loaithucpham"]);
            if (l != -1)
            {
                tP = (from tPs in data.THUCPHAMs where tPs.MALOAITHUCPHAM == l select tPs).ToList();
            }
            
            return View(tP);
        }

        //Xóa thực phẩm 

        public ActionResult XoaThucPham(string MaTP)
        {
            var tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == MaTP select tps).Single();

            data.THUCPHAMs.DeleteOnSubmit(tp);
            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }    
    

        public ActionResult ThucDon()
        {
            var td = (from tds in data.THUCDONs select tds).ToList();
            return View(td);
            
        }
    }
}