using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDinhDuong.Models;

namespace QuanLyDinhDuong.Controllers
{
    public class HomeController : Controller
    {

        //Tạo Objects DtB
        dbQlDDDataContext data = new dbQlDDDataContext();

        public ActionResult Index()
        {
            return View();
        }
 

        private List<TAIKHOAN> laytaikhoan()
        {
            var lstTaiKhoan = (from taikhoan in data.TAIKHOANs select taikhoan).ToList();
            
            return lstTaiKhoan;
        }
        List<TAIKHOAN> taikhoan;

        [HttpGet]
        public ActionResult TinhToanChiSo()
        {
            //taikhoan = laytaikhoan();

            BENHNHAN bn = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();

            //String ho = bn.HOTEN;
            //String Year = bn.NGAYSINH.Value.Year.ToString();


            return View(bn);
        }

        [HttpPost]
        public ActionResult TinhToanChiSo(FormCollection collection,BENHNHAN bn)
        {

            bn = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();

            var ChieuCao = collection["chieucao"];
            var CanNang = collection["cannang"];
            var mucdovandong = collection["mucvandong"];

            float cc = float.Parse(ChieuCao) /100;
            float cn = float.Parse(CanNang);

            double R = 0;

            if (mucdovandong =="vandongit")
                R = 1.2;
            if (mucdovandong == "vandongnhe")
                R = 1.375;
            if (mucdovandong == "vandongvua")
                R = 1.55;
            if (mucdovandong ==  "vandongnang")
                R = 1.725;
            if (mucdovandong == "vandongratnang")
                R = 1.9;

            var BMI = cn / (cc * cc);

            bn.BMI = BMI;

            data.SubmitChanges();

            return View();
        }

        public ActionResult KetQuaTinhToanChiSo()
        {
            BENHNHAN bn = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();

            return View(bn);
        }

    }
}