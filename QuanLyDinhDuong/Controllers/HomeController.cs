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

        public static int TuoiBenhNhan;
        public ActionResult TinhToanChiSo()
        {
            //taikhoan = laytaikhoan();
            BENHNHAN bn = (BENHNHAN)Session["MABENHNHAN"];
            //BENHNHAN bn = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();

            var NamSinhBenhNhan = bn.NGAYSINH.Value.Year;
            var NamSinhHienTai = DateTime.Now.Year;
            TuoiBenhNhan = NamSinhHienTai - NamSinhBenhNhan;

            ViewData["Tuoi"] = TuoiBenhNhan;
            //String ho = bn.HOTEN;
            //String Year = bn.NGAYSINH.Value.Year.ToString();


            return View();
        }

        public static float BMI;
        [HttpPost]
        public ActionResult TinhToanChiSo(FormCollection collection,BENHNHAN bn)
        {

            bn = (BENHNHAN) Session["MABENHNHAN"];
            //bn = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();

            var ChieuCao = collection["chieucao"];
            var CanNang = collection["cannang"];
            var mucdovandong = collection["mucvandong"];
            var gioitinh = collection["gioitinh"];

           

            float cc = float.Parse(ChieuCao) /100;
            float cn = float.Parse(CanNang);

            double R = 0;

            if (mucdovandong == "vandongit")
                R = 1.2;
            if (mucdovandong == "vandongnhe")
                R = 1.375;
            if (mucdovandong == "vandongvua")
                R = 1.55;
            if (mucdovandong == "vandongnang")
                R = 1.725;
            if (mucdovandong == "vandongratnang")
                R = 1.9;

            BMI = cn / (cc * cc);
            var BMR = 0.0;
            //Nam giới: BMR = 66 + [13, 7 x trọng lượng] + [5 x chiều cao] – [6.76 x số tuổi]
            //Nữ giới: BMR = 655 + [9, 6 x trọng lượng] + [1, 8 x chiều cao] – [ 4,7 x số tuổi]
            if (gioitinh == "Nam")
            {
                BMR = ((13.397 * cn) + (4.799 * (cc * 100)) - (5.677 * TuoiBenhNhan) + 88.362);
            }
            else
                BMR = (9.247 * cn) + (3.098 * (cc * 100)) - (4.330 * TuoiBenhNhan) + 447.593;

            var Calo = BMR * R;

            //if (BMI < 16)
            //    @ViewData["ThongTinCoThe"] = "Rất Óm";

            //if (BMI >= 16 && BMI < 17)
            //    @ViewData["ThongTinCoThe"] = "Óm";

            //if (BMI >= 17 && BMI < 18.5)
            //    @ViewData["ThongTinCoThe"] = "Óm Vừa";

            //if (BMI >= 18.5 && BMI < 25)
            //    @ViewData["ThongTinCoThe"] = "Cân Đối";

            //if (BMI >= 25 && BMI < 30)
            //    @ViewData["ThongTinCoThe"] = "Thừa Cân";

            //if (BMI >= 30 && BMI < 35)
            //    @ViewData["ThongTinCoThe"] = "Thừa Cân Loại 1";

            //if (BMI >= 35 && BMI < 40)
            //    @ViewData["ThongTinCoThe"] = "Thừa Cân Loại 2";

            //if (BMI >= 40)
            //    @ViewData["ThongTinCoThe"] = "Thừa Cân Loại 3";

            bn.CHIEUCAO = cc;
            bn.CANNANG = cn;

            bn.BMI = BMI;
            bn.CALO = Calo;

            data.SubmitChanges();

            return RedirectToAction("KetQuaTinhToanChiSo", "Home");
        }

        public ActionResult KetQuaTinhToanChiSo()
        {

            BENHNHAN bn = (BENHNHAN)Session["MABENHNHAN"];
            //BENHNHAN bn = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();
            if (BMI < 16)
                ViewData["ThongTinCoThe"] = "Rất Ốm";

            if (BMI >= 16 && BMI < 17)
                ViewData["ThongTinCoThe"] = "Ốm";

            if (BMI >= 17 && BMI < 18.5)
                ViewData["ThongTinCoThe"] = "Ốm Vừa";

            if (BMI >= 18.5 && BMI < 25)
                ViewData["ThongTinCoThe"] = "Cân Đối";

            if (BMI >= 25 && BMI < 30)
                ViewData["ThongTinCoThe"] = "Thừa Cân";

            if (BMI >= 30 && BMI < 35)
                ViewData["ThongTinCoThe"] = "Thừa Cân Loại 1";

            if (BMI >= 35 && BMI < 40)
                ViewData["ThongTinCoThe"] = "Thừa Cân Loại 2";

            if (BMI >= 40)
                ViewData["ThongTinCoThe"] = "Thừa Cân Loại 3";
            return View(bn);
        }

    }
}