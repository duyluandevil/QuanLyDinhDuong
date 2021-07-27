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



            if (Session["IDTAIKHOAN"] == null)
                ViewBag.ThongBao = null;
            else
                ViewBag.ThongBao = "notnull";

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
            TAIKHOAN tk = (TAIKHOAN)Session["IDTAIKHOAN"];
            BENHNHAN bn1 = (BENHNHAN)Session["MABENHNHAN"];

            MaThucPham = id;
            var tp = (from t in data.THUCPHAMs where t.MATHUCPHAM == id select t).Single();

            var bn = bn1;
            var TDList = (from t in data.THUCDONs where t.MABENHNHAN == bn1.MABENHNHAN select t).ToList();
            ViewBag.THUCDONs = TDList;

            return View(tp);
        }

        [HttpPost]
        public ActionResult ChiTietThucPham(FormCollection f, CTTD cttd)
        {
            cttd = new CTTD();
            var MaThucDon = f["ThucDon"];
            var SoLuong = f["SOLUONG"];
            var mtp = MaThucPham;

            cttd.MATHUCDON = int.Parse(MaThucDon.ToString());
            cttd.MATHUCPHAM = mtp;

            var cttdDB = (from cttds in data.CTTDs where cttds.MATHUCPHAM == MaThucPham && cttds.MATHUCDON == int.Parse(MaThucDon.ToString()) select cttds).SingleOrDefault();
            
            if (cttdDB != null)
            {
                if (String.IsNullOrEmpty(SoLuong))
                {
                    cttdDB.SOLUONG = cttdDB.SOLUONG + 1;
                    data.SubmitChanges();
                }
                else
                {
                    cttdDB.SOLUONG = cttdDB.SOLUONG + int.Parse(SoLuong.ToString());
                    data.SubmitChanges();
                }

                return RedirectToAction("ThucPham", "ThucPham");
            }
            else
            {
                if(String.IsNullOrEmpty(SoLuong))
                {
                    var tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == mtp select tps).Single();

                    cttd.TENTHUCPHAM = tp.TENTHUCPHAM;
                    cttd.ANHBIA = tp.ANHBIA;
                    cttd.SOLUONG = 1;
                    cttd.DAM = tp.DAM;
                    cttd.BEO = tp.BEO;
                    cttd.XO = tp.XO;
                    cttd.CALO = tp.CALO;
                    cttd.TONGCALO = 1 * tp.CALO;

                    data.CTTDs.InsertOnSubmit(cttd);
                    data.SubmitChanges();

                    return RedirectToAction("ThucPham", "ThucPham");
                }
                else
                {
                    var tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == mtp select tps).Single();

                    cttd.TENTHUCPHAM = tp.TENTHUCPHAM;
                    cttd.ANHBIA = tp.ANHBIA;
                    cttd.SOLUONG = int.Parse(SoLuong);
                    cttd.DAM = tp.DAM;
                    cttd.BEO = tp.BEO;
                    cttd.XO = tp.XO;
                    cttd.CALO = tp.CALO;
                    cttd.TONGCALO = float.Parse(SoLuong.ToString()) * tp.CALO;

                    data.CTTDs.InsertOnSubmit(cttd);
                    data.SubmitChanges();

                    return RedirectToAction("ThucPham", "ThucPham");
                }
            }

            //var tp = (from tps in data.THUCPHAMs where tps.MATHUCPHAM == mtp select tps).Single();

            //cttd.TENTHUCPHAM = tp.TENTHUCPHAM;
            //cttd.ANHBIA = tp.ANHBIA;
            //cttd.SOLUONG = float.Parse(SoLuong.ToString());
            //cttd.DAM = tp.DAM;
            //cttd.BEO = tp.BEO;
            //cttd.XO = tp.XO;
            //cttd.CALO = tp.CALO;
            //cttd.TONGCALO = float.Parse(SoLuong.ToString()) * tp.CALO;
            
            //data.CTTDs.InsertOnSubmit(cttd);
            //data.SubmitChanges();

            //return RedirectToAction("ThucPham", "ThucPham");
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

        
    }
}