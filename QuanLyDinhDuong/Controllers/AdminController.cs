using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDinhDuong.Models;

namespace QuanLyDinhDuong.Controllers
{
    //[System.Runtime.InteropServices.Guid("32DDE1B2-B387-49D2-9CCD-68A954870922")]
    public class AdminController : Controller
    {
        dbQlDDDataContext data = new dbQlDDDataContext();
        // GET: Admin
        public ActionResult DangNhapAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAdmin(FormCollection collection)
        {
            ViewData["Loi1"] = "";
            ViewData["Loi2"] = "";
            //Đăng nhập
            var tkdn = collection["TenDangNhap"];
            var mkdn = collection["MatKhau"];
            if (String.IsNullOrEmpty(tkdn))
            {
                ViewData["Loi1"] = "Bạn chưa nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(mkdn))
            {
                ViewData["Loi2"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                var taikh = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == tkdn && tks.MATKHAU == mkdn && tks.MACHUCVU == 0 select tks).SingleOrDefault();
                if (taikh != null)
                {
                    var benhnhan = (from bn in data.BENHNHANs where bn.IDTAIKHOAN == tkdn select bn).SingleOrDefault();
                    Session["IDTAIKHOAN"] = taikh;
                    Session["MABENHNHAN"] = benhnhan;
                    return RedirectToAction("Admin", "Admin");
                }
                else ViewData["Loi1"] = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Admin()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
            return View();
        }
        public ActionResult TrangCaNhan()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
            TAIKHOAN taikhoan = (TAIKHOAN)Session["IDTAIKHOAN"]; 
            var tk = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == taikhoan.IDTAIKHOAN select tks).Single();
            return View(tk);
        }

        public ActionResult TaiKhoan()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
            var tk = (from tks in data.TAIKHOANs select tks).ToList();
            
            return View(tk);
        }

        public ActionResult BenhNhan()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
            var bn = (from bns in data.BENHNHANs select bns).ToList();
            
            return View(bn);
        }
        public ActionResult ThucPham()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
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

        //Thêm loại thực phẩm

        public ActionResult ThemLoaiThucPham()
        {
            return View();
        }

        //Thêm thực phẩm 

        public ActionResult ThemThucPham(string MaTP)
        {
            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;
            return View();
        }

        [HttpPost]
        public ActionResult ThemThucPham(FormCollection f, THUCPHAM tp)
        {
            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;

            var mathucpham = f["MATHUCPHAM"];
            var TENTHUCPHAM = f["TENTHUCPHAM"];
            var DAM = f["DAM"];
            var BEO = f["BEO"];
            var XO = f["XO"];
            var CALO = f["CALO"];
            var MALOAITHUCPHAM = f["ChonMaLoai"];

            tp.MATHUCPHAM = mathucpham;
            tp.TENTHUCPHAM = TENTHUCPHAM;
            tp.DAM = float.Parse(DAM);
            tp.BEO = float.Parse(BEO);
            tp.XO = float.Parse(XO);
            tp.CALO = float.Parse(CALO);
            tp.MALOAITHUCPHAM = int.Parse(MALOAITHUCPHAM);

            data.THUCPHAMs.InsertOnSubmit(tp);
            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }
        //Xóa thực phẩm 

        public ActionResult XoaThucPham(string MaTP)
        {
            var tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == MaTP select tps).Single();

            data.THUCPHAMs.DeleteOnSubmit(tp);
            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }

        //Chỉnh sửa thực phẩm
        public static string Matp;

        [HttpGet]
        public ActionResult SuaThucPham(string MaTP)
        {
            Matp = MaTP;
            var tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == MaTP select tps).SingleOrDefault();

            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;

            return View(tp);
        }

        [HttpPost]
        public ActionResult SuaThucPham(FormCollection f, THUCPHAM tp)
        {
            //var mathucpham = f["MATHUCPHAM"];
            var TENTHUCPHAM = f["TENTHUCPHAM"];
            var DAM = f["DAM"];
            var BEO = f["BEO"];
            var XO = f["XO"];
            var CALO = f["CALO"];
            var MALOAITHUCPHAM = f["ChonMaLoai"];

            tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == Matp select tps).Single();

            tp.TENTHUCPHAM = TENTHUCPHAM;
            tp.DAM = float.Parse(DAM);
            tp.BEO = float.Parse(BEO);
            tp.XO = float.Parse(BEO);
            tp.CALO = float.Parse(CALO);
            tp.MALOAITHUCPHAM = int.Parse(MALOAITHUCPHAM.ToString());

            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }

        public ActionResult ThucDon()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
            var td = (from tds in data.THUCDONs select tds).ToList();
            return View(td);
            
        }
    }
}