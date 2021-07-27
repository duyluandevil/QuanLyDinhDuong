using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDinhDuong.Models;

namespace QuanLyDinhDuong.Controllers
{
    public class ThucDonController : Controller
    {

        dbQlDDDataContext data = new dbQlDDDataContext();
        
        // GET: ThucDon
        public ActionResult ThucDon()
        {
            if (Session["IDTAIKHOAN"] == null)
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }

            BENHNHAN bn = (BENHNHAN)Session["MABENHNHAN"];

            var listThucDon = (from td in data.THUCDONs where td.MABENHNHAN == bn.MABENHNHAN select td).ToList();
            return View(listThucDon);
        }

        public static int maTD;

        public ActionResult ChiTietThucDon(int MaThucDon)
        {
            var thucdon = (from td in data.CTTDs where td.MATHUCDON == MaThucDon select td).ToList();
            maTD = MaThucDon;
            //ViewBag.AnhBia = (from tp in data.THUCPHAMs where tp.MATHUCPHAM == "TP001" select tp.ANHBIA).FirstOrDefault();

            return View(thucdon);
        }


        public ActionResult XoaThucpham(string MaThucPham)
        {
            //var thucdon = (from td in data.CTTDs where td.MATHUCDON == maTD select td).ToList();

            var thucdon = (from tdn in data.CTTDs where tdn.MATHUCDON == maTD && tdn.MATHUCPHAM == MaThucPham select tdn).Single();

            data.CTTDs.DeleteOnSubmit(thucdon);
            data.SubmitChanges();

            //string url = this.Url.Action("ChiTietThuocDon", "ThucDon", new { id = maTD });

            return RedirectToAction("ChiTietThucDon", "ThucDon");
        }

        public ActionResult XoaThucDon(int MaThucDon)
        {
            //var thucdon = (from td in data.CTTDs where td.MATHUCDON == maTD select td).ToList();

            var thucdon = (from tdn in data.THUCDONs where tdn.MATHUCDON == MaThucDon select tdn).Single();

            data.THUCDONs.DeleteOnSubmit(thucdon);
            data.SubmitChanges();

            //string url = this.Url.Action("ChiTietThuocDon", "ThucDon", new { id = maTD });

            return RedirectToAction("ThucDon", "ThucDon");
        }

        [HttpGet]
        public ActionResult ThemThucDon()
        {
            //var thucdon = (from td in data.CTTDs where td.MATHUCDON == maTD select td).ToList();

            return View();
        }


        [HttpPost]
        public ActionResult ThemThucDon(FormCollection f, THUCDON td)
        {
            BENHNHAN bn = (BENHNHAN)Session["MABENHNHAN"];
            var buoi = f["buoi"];
            var ngaylap = String.Format("{0:MM/dd/yyyy}", f["ngaylap"]);


            if (String.IsNullOrEmpty(buoi))
            {
                ViewData["Loi1"] = "Bạn chưa nhập buổi của thực đơn";
            }


            else
            {
                //td = (from tdv in data.THUCDONs where td.MABENHNHAN == "BN001" select tdv).Single();
                td.BUOI = buoi;
                td.NGAYLAP = DateTime.Parse(ngaylap);
                td.MABENHNHAN = bn.MABENHNHAN;
                data.THUCDONs.InsertOnSubmit(td);
                data.SubmitChanges();
                return RedirectToAction("ThucDon","ThucDon");
            }
            return this.ThemThucDon();
        }

    }
}