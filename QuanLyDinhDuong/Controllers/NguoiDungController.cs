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
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection, TAIKHOAN tk)
        {
            ViewData["Loi1"] = "";
            ViewData["Loi2"] = "";
            ViewData["Loi3"] = "";
            ViewData["Loi4"] = "";
            ViewData["Loi5"] = "";
            ViewData["Loi6"] = "";
            ViewData["Loi7"] = "";
            ViewData["Loi8"] = "";
            ViewBag.ThongBao1 = "";
            ViewBag.ThongBao2 = "";
            var taikhoan = collection["TaiKhoan"];
            var matkhau = collection["MatKhau"];
            var nhaplaimk = collection["NhapLaiMatKhau"];
            var email = collection["email"];
            var sdt = collection["sdt"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);
            var gioitinh = collection["RadioGioiTinh"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi3"] = "Bạn chưa nhập tên tài khoản";
            }
            else
            {
                TAIKHOAN taik = data.TAIKHOANs.SingleOrDefault(t => t.IDTAIKHOAN == taikhoan);
                if (taik != null)
                {
                    ViewBag.ThongBao2 = "Tên tài khoản đã tồn tại";
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
            if (taikhoan != "" && matkhau != "" && nhaplaimk == matkhau && email != "" && sdt != "" && ngaysinh != "")
            {
                tk.IDTAIKHOAN = taikhoan;
                tk.MATKHAU = matkhau;
                tk.EMAIL = email;
                tk.SDT = sdt;
                tk.GIOITINH = gioitinh;
                tk.NGAYSINH = DateTime.Parse(ngaysinh);
                tk.MACHUCVU = 1;
                data.TAIKHOANs.InsertOnSubmit(tk);
                data.SubmitChanges();
                return this.Dangnhap();
            }
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
                var taik = (from tks in data.TAIKHOANs where tks.IDTAIKHOAN == tkdn && tks.MATKHAU == mkdn select tks).SingleOrDefault();
                if (taik != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else ViewBag.ThongBao1 = "Tên đăng nhập hoặc mật khẩu không đúng";

            }    
            return this.Dangnhap();
        }


        public ActionResult TrangCaNhan()
        {
            return View();
        }
        // 82a71b7698b5168ad94da2e6bb4f364248f19cc3
    }
}