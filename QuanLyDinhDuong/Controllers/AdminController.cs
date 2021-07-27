﻿using System;
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

        [HttpGet]
        public ActionResult ThemTaiKhoan(string IDTK)
        {
            var TpList = (from t in data.CHUCVUs select t).ToList();
            ViewBag.CHUCVUs = TpList;
            return View();
        }

        [HttpPost]
        public ActionResult ThemTaiKhoan(FormCollection f, TAIKHOAN tp)
        {
            var TpList = (from t in data.CHUCVUs select t).ToList();
            ViewBag.CHUCVUs = TpList;

            var idtaikhoan = f["IDTAIKHOAN"];
            var hoten = f["HOTEN"];
            var gioitinh = f["GIOITINH"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", f["NGAYSINH"]);
            var matkhau = f["MATKHAU"];
            var sdt = f["SDT"];
            var email = f["EMAIL"];
            var MACHUCVU = f["ChonChucVu"];

            tp.IDTAIKHOAN = idtaikhoan;
            tp.HOTEN = hoten;
            tp.GIOITINH = gioitinh;
            tp.NGAYSINH = DateTime.Parse(ngaysinh);
            tp.MATKHAU = matkhau;
            tp.SDT = sdt;
            tp.EMAIL = email;
            tp.MACHUCVU = int.Parse(MACHUCVU);

            data.TAIKHOANs.InsertOnSubmit(tp);
            data.SubmitChanges();

            return RedirectToAction("TaiKhoan", "Admin");
        }
        //Chỉnh sửa tai khoan
        public static string Matk;

        [HttpGet]
        public ActionResult SuaTaiKhoan(string MaTK)
        {
            Matk = MaTK;
            var tk = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == MaTK select tks).SingleOrDefault();

            var TpList = (from t in data.CHUCVUs select t).ToList();
            ViewBag.CHUCVUs = TpList;
            return View(tk);
        }
        [HttpPost]
        public ActionResult SuaTaiKhoan(FormCollection f, TAIKHOAN tk)
        {
            var hoten = f["HOTEN"];
            var gioitinh = f["GIOITINH"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", f["NGAYSINH"]);
            var matkhau = f["MATKHAU"];
            var sdt = f["SDT"];
            var email = f["EMAIL"];
            var MACHUCVU = f["ChonChucVu"];

            tk = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == Matk select tks).Single();

            tk.HOTEN = hoten;
            tk.GIOITINH = gioitinh;
            tk.NGAYSINH = DateTime.Parse(ngaysinh);
            tk.MATKHAU = matkhau;
            tk.SDT = sdt;
            tk.EMAIL = email;
            tk.MACHUCVU = int.Parse(MACHUCVU);

            data.SubmitChanges();

            return RedirectToAction("TaiKhoan", "Admin");
        }
        //Xóa tài khoản
        public ActionResult XoaTaiKhoan(string MaTK)
        {
            var tk = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == MaTK select tks).Single();

            data.TAIKHOANs.DeleteOnSubmit(tk);
            data.SubmitChanges();

            return RedirectToAction("TaiKhoan", "Admin");
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

        public static int Mabn;
        //Thêm bệnh nhân
        public ActionResult ThemBenhNhan()
        {
            var TkList = (from t in data.TAIKHOANs select t).ToList();
            ViewBag.TAIKHOAN = TkList;

            var BnLast = (from tps in data.BENHNHANs select tps).ToList();
            ViewData["MaBenhNhan"] = int.Parse(BnLast.LastOrDefault().MABENHNHAN.ToString()) + 1;
            Mabn = int.Parse(BnLast.LastOrDefault().MABENHNHAN.ToString()) + 1;

            return View();
        }

        [HttpPost]
        public ActionResult ThemBenhNhan(FormCollection f, BENHNHAN bn)
        {
            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;



            var MABENHNHAN = Mabn;
            var TENTHUCPHAM = f["TENBENHNHAN"];
            var GIOITINH = f["ChonGioiTinh"];
            var NGAYSINH = String.Format("{0:MM/dd/yyyy}", f["NGAYSINH"]);
            var CHIEUCAO = f["CHIEUCAO"];
            var CANNANG = f["CANNANG"];
            var CALO = f["CALO"];
            
            var BMI = f["BMI"];
            var IDTAIKHOAN = f["ChonIDTaiKhoan"];

            bn.MABENHNHAN = MABENHNHAN;
            bn.HOTEN = TENTHUCPHAM;
            bn.GIOITINH = GIOITINH;
            bn.NGAYSINH = DateTime.Parse(NGAYSINH);
            bn.CHIEUCAO = float.Parse(CHIEUCAO);
            bn.CALO = float.Parse(CALO);
            bn.NUOC = 0;
            bn.BMI = float.Parse(BMI);
            bn.IDTAIKHOAN = IDTAIKHOAN;

            data.BENHNHANs.InsertOnSubmit(bn);
            data.SubmitChanges();

            return RedirectToAction("BenhNhan", "Admin");
        }

        public static int mabn;
        //Sửa bệnh nhân
        public ActionResult SuaBenhNhan(int MaBN)
        {
            mabn = MaBN;
            var TkList = (from t in data.BENHNHANs  select t).ToList();
            ViewBag.TAIKHOAN = TkList;

            var bn = (from bns in data.BENHNHANs where bns.MABENHNHAN == MaBN select bns).Single();
            

            return View(bn);
        }

        [HttpPost]
        public ActionResult SuaBenhNhan(FormCollection f, BENHNHAN bn)
        {
            //var MABENHNHAN = mabn;
            var TENTHUCPHAM = f["TENBENHNHAN"];
            var GIOITINH = f["ChonGioiTinh"];
            var NGAYSINH = String.Format("{0:MM/dd/yyyy}", f["NGAYSINH"]);
            var CHIEUCAO = f["CHIEUCAO"];
            var CANNANG = f["CANNANG"];
            var CALO = f["CALO"];

            var BMI = f["BMI"];
            var IDTAIKHOAN = f["ChonIDTaiKhoan"];

            bn = (from bns in data.BENHNHANs where bns.MABENHNHAN == mabn select bns).Single();

            
            bn.HOTEN = TENTHUCPHAM;
            bn.GIOITINH = GIOITINH;
            bn.NGAYSINH = DateTime.Parse(NGAYSINH);
            bn.CHIEUCAO = float.Parse(CHIEUCAO);
            bn.CALO = float.Parse(CALO);
            bn.NUOC = 0;
            bn.BMI = float.Parse(BMI);
            bn.IDTAIKHOAN = IDTAIKHOAN;

            
            data.SubmitChanges();

            return RedirectToAction("BenhNhan", "Admin");
        }

        //Xóa bệnh nhân
        public ActionResult XoaBenhNhan(int MaBN)
        {
            var bn = (from bns in data.BENHNHANs where bns.MABENHNHAN == MaBN select bns).Single();

            data.BENHNHANs.DeleteOnSubmit(bn);
            data.SubmitChanges();

            return RedirectToAction("BenhNhan", "Admin");
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
        public static int MLTP;
        public ActionResult ThemLoaiThucPham()
        {
            var tpLast = (from tps in data.LOAITHUCPHAMs select tps).ToList();
            ViewData["MaThucDon"] = int.Parse(tpLast.LastOrDefault().MALOAITHUCPHAM.ToString()) + 1;
            MLTP = int.Parse(tpLast.LastOrDefault().MALOAITHUCPHAM.ToString()) + 1;
            return View();
        }

        [HttpPost]
        public ActionResult ThemLoaiThucPham(FormCollection f, LOAITHUCPHAM ltp)
        {
            var MaLoaiThucPham = MLTP;
            var TenLoaiThucPham = f["TENLOAITHUCPHAM"];

            ltp.MALOAITHUCPHAM = MaLoaiThucPham;
            ltp.TENLOAITHUCPHAM = TenLoaiThucPham;

            data.LOAITHUCPHAMs.InsertOnSubmit(ltp);
            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }

        //Xóa loại thực phẩm
        public ActionResult XoaLoaiThucPham(int MaLTP)
        {
            var tp = (from tps in data.LOAITHUCPHAMs where tps.MALOAITHUCPHAM == MaLTP select tps).Single();

            data.LOAITHUCPHAMs.DeleteOnSubmit(tp);
            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }

        //Sửa loại thực phẩm
        public static int Maltp;

        [HttpGet]
        public ActionResult SuaLoaiThucPham(int MaLTP)
        {
            Maltp = MaLTP;
            var tp = (from tps in data.LOAITHUCPHAMs where tps.MALOAITHUCPHAM == MaLTP select tps).SingleOrDefault();

            return View(tp);
        }

        [HttpPost]
        public ActionResult SuaLoaiThucPham(FormCollection f, LOAITHUCPHAM ltp)
        {
            //var mathucpham = f["MATHUCPHAM"];
            var TENLOAITHUCPHAM = f["TENLOAITHUCPHAM"];


            ltp = (from tps in data.LOAITHUCPHAMs where tps.MALOAITHUCPHAM == Maltp select tps).Single();

            ltp.TENLOAITHUCPHAM = TENLOAITHUCPHAM;

            data.SubmitChanges();

            return RedirectToAction("ThucPham", "Admin");
        }

        //Thêm thực phẩm 

        public ActionResult ThemThucPham(string MaTP)
        {
            var TpList = (from t in data.LOAITHUCPHAMs select t).ToList();
            ViewBag.LOAITHUCPHAMs = TpList;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemThucPham(FormCollection f, THUCPHAM tp, HttpPostedFileBase fileupload)
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

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn lại đường dẫn";
                return View();
            }
            //Thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                    //Kiểm tra hình ảnh tồn tại chưa?
                    
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Lưu hình ảnh vào đường dẫn 
                        fileupload.SaveAs(path);
                    }
                    tp.ANHBIA = "/Content/img/" + fileName;
                    tp.MATHUCPHAM = mathucpham;
                    tp.TENTHUCPHAM = TENTHUCPHAM;
                    tp.DAM = float.Parse(DAM);
                    tp.BEO = float.Parse(BEO);
                    tp.XO = float.Parse(XO);
                    tp.CALO = float.Parse(CALO);
                    tp.MALOAITHUCPHAM = int.Parse(MALOAITHUCPHAM);
                    //Lưu vào CSDL
                    data.THUCPHAMs.InsertOnSubmit(tp);
                    data.SubmitChanges();
                }
                return RedirectToAction("ThucPham","Admin");
            }
            
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
        [ValidateInput(false)]
        public ActionResult SuaThucPham(FormCollection f, THUCPHAM tp, HttpPostedFileBase fileupload)
        {
            //var mathucpham = f["MATHUCPHAM"];
            var TENTHUCPHAM = f["TENTHUCPHAM"];
            var DAM = f["DAM"];
            var BEO = f["BEO"];
            var XO = f["XO"];
            var CALO = f["CALO"];
            var MALOAITHUCPHAM = f["ChonMaLoai"];

            tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == Matp select tps).Single();

            if (fileupload == null)
            {
                tp.TENTHUCPHAM = TENTHUCPHAM;
                tp.DAM = float.Parse(DAM);
                tp.BEO = float.Parse(BEO);
                tp.XO = float.Parse(BEO);
                tp.CALO = float.Parse(CALO);
                tp.MALOAITHUCPHAM = int.Parse(MALOAITHUCPHAM.ToString());
                data.SubmitChanges();
                return RedirectToAction("ThucPham", "Admin");
                ViewBag.Thongbao = "Vui lòng chọn lại đường dẫn";
                return View();
            }
            //Thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                    //Kiểm tra hình ảnh tồn tại chưa?

                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Lưu hình ảnh vào đường dẫn 
                        fileupload.SaveAs(path);
                    }
                    tp.ANHBIA = "/Content/img/" + fileName;

                    tp.TENTHUCPHAM = TENTHUCPHAM;
                    tp.DAM = float.Parse(DAM);
                    tp.BEO = float.Parse(BEO);
                    tp.XO = float.Parse(XO);
                    tp.CALO = float.Parse(CALO);
                    tp.MALOAITHUCPHAM = int.Parse(MALOAITHUCPHAM);
                    //Lưu vào CSDL
                    data.SubmitChanges();
                }
                return RedirectToAction("ThucPham", "Admin");
            }            
        }

        [HttpGet]
        public ActionResult ThucDon()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("DangNhapAdmin", "Admin");
            }
            var td = (from tds in data.THUCDONs select tds).ToList();

            return View(td);
            
        }
        

        public static int Matd;
        //Thêm thực đơn
        public ActionResult ThemThucDon(string MaTD)
        {

            var TdLast = (from tps in data.THUCDONs select tps).ToList();
            ViewData["MaThucDon"] = int.Parse(TdLast.LastOrDefault().MATHUCDON.ToString()) + 1;
            Matd = int.Parse(TdLast.LastOrDefault().MATHUCDON.ToString()) + 1;

            var BnList = (from t in data.BENHNHANs select t).ToList();
            ViewBag.BENHNHANs = BnList;

            return View();
        }

        [HttpPost]
        public ActionResult ThemThucDon(FormCollection f, THUCDON td)
        {
            var BnList = (from t in data.BENHNHANs select t).ToList();
            ViewBag.BENHNHANs = BnList;

            //var MATHUCDON = f["MATHUCDON"];
            var BUOI = f["BUOI"];
            var NGAYLAP = String.Format("{0:MM/dd/yyyy}", f["NGAYLAP"]);
            var MABENHNHAN = f["ChonMaBenhNhan"];

            //td.MATHUCDON = int.Parse(MATHUCDON);

            td.BUOI = BUOI;
            td.NGAYLAP = DateTime.Parse(NGAYLAP);
            td.MABENHNHAN = int.Parse(MABENHNHAN.ToString());


            data.THUCDONs.InsertOnSubmit(td);
            data.SubmitChanges();

            return RedirectToAction("ThucDon", "Admin");
        }

        //Sửa thực đơn
        //Chỉnh sửa thực phẩm
        public static int Matds;

        [HttpGet]
        public ActionResult SuaThucDon(int MaTD)
        {
            Matds = MaTD;
            var td = (from tps in data.THUCDONs where tps.MATHUCDON == MaTD select tps).SingleOrDefault();

            var BnList = (from t in data.BENHNHANs select t).ToList();
            ViewBag.BENHNHANs = BnList;

            return View(td);
        }

        [HttpPost]
        public ActionResult SuaThucDon(FormCollection f, THUCDON td)
        {
            var BUOI = f["BUOI"];
            var NGAYLAP = String.Format("{0:MM/dd/yyyy}", f["NGAYLAP"]);
            var MABENHNHAN = f["ChonMaBenhNhan"];

            td = (from tds in data.THUCDONs where tds.MATHUCDON == Matds select tds).Single();

            td.BUOI = BUOI;
            td.NGAYLAP = DateTime.Parse(NGAYLAP);
            td.MABENHNHAN = int.Parse(MABENHNHAN.ToString());

            data.SubmitChanges();

            return RedirectToAction("ThucDon", "Admin");
        }

        //Xóa thực đơn
        public ActionResult XoaThucDon(int MaTD)
        {
            var td = (from tds in data.THUCDONs where tds.MATHUCDON == MaTD select tds).Single();

            data.THUCDONs.DeleteOnSubmit(td);
            data.SubmitChanges();

            return RedirectToAction("ThucDon", "Admin");
        }

        public ActionResult ChiTietThucDon(int MaTD)
        {
            var td = (from tds in data.THUCDONs select tds).ToList();

            var CttdList = (from t in data.CTTDs where t.MATHUCDON == MaTD select t).ToList();
            ViewBag.CTTDs = CttdList;

            return View(td);
        }

        public ActionResult ThemChiTietThucDon()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ThemChiTietThucDon(FormCollection f, CTTD cttd)
        {
            var MATHUCDON = f["MATHUCDON"];
            var MATHUCPHAM = f["MATHUCPHAM"];

            var TP = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == MATHUCPHAM select tps).Single();

            var TENTHUCPHAM = TP.TENTHUCPHAM;

            var ANHBIA = TP.ANHBIA;
            var DAM = TP.DAM;
            var BEO = TP.BEO;
            var XO = TP.XO;
            var CALO = TP.CALO;
            var SOLUONG = f["SOLUONG"];
            var TONGCALO = int.Parse(SOLUONG.ToString()) * CALO;

            cttd.MATHUCDON = int.Parse(MATHUCDON.ToString());
            cttd.MATHUCPHAM = MATHUCPHAM;
            cttd.TENTHUCPHAM = TENTHUCPHAM;
            cttd.ANHBIA = ANHBIA;
            cttd.DAM = float.Parse(DAM.ToString());
            cttd.BEO = float.Parse(BEO.ToString());
            cttd.XO = float.Parse(XO.ToString());
            cttd.CALO = float.Parse(CALO.ToString());
            cttd.SOLUONG = float.Parse(SOLUONG.ToString());
            cttd.TONGCALO = float.Parse(TONGCALO.ToString());

            data.CTTDs.InsertOnSubmit(cttd);
            data.SubmitChanges();

            return RedirectToAction("ThucDon", "Admin");
        }
    }
}