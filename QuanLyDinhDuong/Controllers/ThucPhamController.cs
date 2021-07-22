using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDinhDuong.Models;
using PagedList;
using PagedList.Mvc;

namespace QuanLyDinhDuong.Controllers
{
    public class ThucPhamController : Controller
    {
        // GET: ThucPham
        dbQlDDDataContext data = new dbQlDDDataContext();


        private List<THUCPHAM> layThucPham(int count)
        {
            return data.THUCPHAMs.OrderByDescending(a => a.MATHUCPHAM).Take(count).ToList();
        }
        public ActionResult ThucPham(int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);

            //var thucphammoi = layThucPham(10);
            var lstthucpham = (from ltp in data.THUCPHAMs select ltp).ToList();
            return View(lstthucpham.ToPagedList(pageNum, pageSize));
        }

        public ActionResult LoaiThucPham()
        {
            var loaithucpham = from ltp in data.LOAITHUCPHAMs select ltp;
            return PartialView(loaithucpham);
        }


        public ActionResult TPTheoLoai(int id, int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);

            //var thucphammoi = layThucPham(10);

            var thucpham = from t in data.THUCPHAMs where t.MALOAITHUCPHAM == id select t;

            return View(thucpham.ToPagedList(pageNum, pageSize));
        }

        public static string MaThucPham;
        public ActionResult ChiTietThucPham(string id)
        {
            MaThucPham = id;
            var tp = (from t in data.THUCPHAMs where t.MATHUCPHAM == id select t).Single();

            var bn = getBenhNhan("duyluan0104");
            var TDList = (from t in data.THUCDONs where t.MABENHNHAN == bn.MABENHNHAN select t).ToList();
            ViewBag.THUCDONs = TDList;

            return View(tp);
        }

        [HttpPost]
        public ActionResult ChiTietThucPham(FormCollection f, CTTD cttd)
        {

            var MaThucDon = f["ThucDon"];
            var td = (from tds in data.THUCDONs where tds.MATHUCDON.ToString() == MaThucDon select tds).Single();

            //cttd = (from tds in data.CTTDs where tds.MATHUCDON.ToString() == MaThucDon select tds).Single();

            //var SoLuong = f["quantity"];
            

            cttd.MATHUCPHAM = "TP008";
            cttd.MATHUCDON = td.MATHUCDON;
            cttd.ANHBIA = "null";
            cttd.CALO = 0;
            cttd.DAM = 0;
            cttd.BEO = 0;
            cttd.XO = 0;
            cttd.SOLUONG = 0;
            cttd.TONGCALO = 0;
            cttd.TENTHUCPHAM = "Trứng";


            
            data.CTTDs.InsertOnSubmit(cttd);
            data.SubmitChanges();

            return RedirectToAction("DanhSachThucDon", "ThucDon");
        }


        private BENHNHAN getBenhNhan(string idTaiKhoan)
        {
            var benhnhan = (from bn in data.BENHNHANs where idTaiKhoan == bn.IDTAIKHOAN select bn).Single();
            return benhnhan;
        }
        public ActionResult DanhSachThucDon()
        {
            var bn = getBenhNhan("duyluan0104");
            var buoithucdon = (from t in data.THUCDONs where t.MABENHNHAN == bn.MABENHNHAN  select t).ToList();
            return PartialView(buoithucdon);
        }

        public ActionResult ThemThucPhamVaoThucDon(CTTD cttd)
        {
            //var MaThucDon = f["ThucDon"];
            //var td = (from tds in data.THUCDONs where tds.MATHUCDON.ToString() == MaThucDon select tds.BUOI).Single();

            //var SoLuong = f["quantity"];

            cttd.MATHUCPHAM = "TP009";
            cttd.TENTHUCPHAM = "Trứng thối";
            cttd.MATHUCDON = 1;


            data.CTTDs.InsertOnSubmit(cttd);
            data.SubmitChanges();

            return RedirectToAction("DanhSachThucDon", "ThucDon");
        }

    }
}