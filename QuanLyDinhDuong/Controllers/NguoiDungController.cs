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
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection, TAIKHOAN tk, BENHNHAN bn)
        {
            ViewData["Loi1"] = "";
            ViewData["Loi2"] = "";
            ViewData["Loi3"] = "";
            ViewData["Loi4"] = "";
            ViewData["Loi5"] = "";
            ViewData["Loi6"] = "";
            ViewData["Loi7"] = "";
            ViewData["Loi8"] = "";
            var tab = collection["tab"];
            if (tab == "DangNhap")
            {
                //Đăng nhập
                var tkdn = collection["TenDangNhap"];
                var mkdn = collection["MatKhauDangNhap"];
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
                    var taikh = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == tkdn && tks.MATKHAU == mkdn select tks).SingleOrDefault();
                    if (taikh != null)
                    {
                        var benhnhan = (from b in data.BENHNHANs where b.IDTAIKHOAN == tkdn select b).SingleOrDefault();
                        Session["IDTAIKHOAN"] = taikh;
                        Session["MABENHNHAN"] = benhnhan;
                        return RedirectToAction("Index2", "Home");
                    }
                    else ViewData["Loi1"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            else
            {
                //Đăng ký
                var taikhoan = collection["TaiKhoan"];
                var matkhau = collection["MatKhau"];
                var nhaplaimk = collection["NhapLaiMatKhau"];
                var email = collection["email"];
                var sdt = collection["sdt"];
                var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);
                var gioitinh = collection["RadioGioiTinh"];

                TAIKHOAN taik = data.TAIKHOANs.SingleOrDefault(t => t.IDTAIKHOAN == taikhoan);
                if (taik != null && taikhoan != "" && matkhau != "" && nhaplaimk == matkhau && email != "" && sdt != "" && ngaysinh != "")
                {
                    tk.IDTAIKHOAN = taikhoan;
                    tk.MATKHAU = matkhau;
                    tk.EMAIL = email;
                    tk.SDT = sdt;
                    tk.GIOITINH = gioitinh;
                    tk.NGAYSINH = DateTime.Parse(ngaysinh);
                    tk.MACHUCVU = 1; // Khi đăng ký tài khoản mặc định sẽ là bệnh nhân
                    data.TAIKHOANs.InsertOnSubmit(tk);
                    data.SubmitChanges();
                    //tạo mã bệnh nhân có tự động tăng???
                    //Thêm thông tin bệnh nhân đồng thời khi tạo tài khoản
                    
                    bn.GIOITINH = gioitinh;
                    bn.NGAYSINH = DateTime.Parse(ngaysinh);
                    bn.IDTAIKHOAN = taikhoan;
                    data.BENHNHANs.InsertOnSubmit(bn);
                    data.SubmitChanges();

                    ViewData["Loi1"] = "Bạn đã đăng ký thành công";

                    return this.Dangnhap();
                }
                else
                {
                    if (String.IsNullOrEmpty(taikhoan))
                    {
                        ViewData["Loi3"] = "Bạn chưa nhập tên tài khoản";
                    }
                    else
                    {
                        if (taik != null)
                        {
                            ViewData["Loi3"] = "Tên tài khoản đã tồn tại";
                        }
                    }
                    if (String.IsNullOrEmpty(matkhau))
                    {
                        ViewData["Loi4"] = "Bạn chưa nhập mật khẩu";
                    }
                    if (String.IsNullOrEmpty(nhaplaimk))
                    {
                        ViewData["Loi5"] = "Bạn chưa nhập lại mật khẩu";
                    }
                    else if (matkhau != nhaplaimk)
                    {
                        ViewBag.ThongBao = "Mật khẩu nhập lại không đúng";
                    }
                    if (String.IsNullOrEmpty(email))
                    {
                        ViewData["Loi6"] = "Bạn chưa nhập Email";
                    }
                    if (String.IsNullOrEmpty(sdt))
                    {
                        ViewData["Loi7"] = "Bạn chưa nhập số điện thoại";
                    }
                    if (String.IsNullOrEmpty(ngaysinh))
                    {
                        ViewData["Loi8"] = "Bạn chưa chọn ngày sinh";
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult TrangCaNhan()
        {
            if (Session["MABENHNHAN"] == null)
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["IDTAIKHOAN"] = null;
            Session["MABENHNHAN"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ChinhSuaThongTin()
        {
            if (Session["MABENHNHAN"] == null)
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChinhSuaThongTin(FormCollection collection)
        {
            var ten = collection["hoten"];
            var email = collection["email"];
            var sdt = collection["sdt"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);
            var gioitinh = collection["RadioGioiTinh"];
            var chieucao = collection["chieucao"];
            var cannang = collection["cannang"];

            TAIKHOAN tk = (TAIKHOAN)Session["IDTAIKHOAN"];
            var taikh = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == tk.IDTAIKHOAN select tks).SingleOrDefault();
            var benhnhan = (from b in data.BENHNHANs where b.IDTAIKHOAN == tk.IDTAIKHOAN select b).SingleOrDefault();
            taikh.HOTEN = ten;
            taikh.EMAIL = email;
            taikh.SDT = sdt;
            taikh.NGAYSINH = DateTime.Parse(ngaysinh);
            taikh.GIOITINH = gioitinh;
            data.SubmitChanges();

            benhnhan.HOTEN = ten;
            benhnhan.NGAYSINH = DateTime.Parse(ngaysinh);
            benhnhan.GIOITINH = gioitinh;
            benhnhan.CHIEUCAO = float.Parse(chieucao);
            benhnhan.CANNANG = float.Parse(cannang);
            data.SubmitChanges();
            Session["IDTAIKHOAN"] = taikh;
            Session["MABENHNHAN"] = benhnhan;
            return RedirectToAction("TrangCaNhan", "NguoiDung");
        }
        // 82a71b7698b5168ad94da2e6bb4f364248f19cc3
    }
}