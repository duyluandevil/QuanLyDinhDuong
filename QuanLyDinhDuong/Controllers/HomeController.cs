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
            taikhoan = laytaikhoan();
            return View(taikhoan.LastOrDefault());
        }

        [HttpPost]
        public ActionResult TinhToanChiSo(FormCollection collection)
        {
            var cannang = collection["cannang"];
            var chieucao = collection["txtChieuCao"];
            var calo = collection["tdee"];
            var bmi = collection["bmi"];

            float cc, cn, bmii, caloo;
            //var cc = float.Parse(chieucao.ToString());


            //if (cc <= 0)
            //    ViewData["Loi1"] = "Chiều cao bạn nhập sai";
            if (String.IsNullOrEmpty(chieucao))
            {
                ViewData["Loi1"] = "Bạn chưa nhập chiều cao";                   
                
            }

            else if(float.Parse(chieucao.ToString()) < 0)
            {
                ViewData["Loi1"] = "Bạn nhập sai chiều cao";
                cc = float.Parse(chieucao.ToString());
                
            }
            if (String.IsNullOrEmpty(cannang))
            {
                ViewData["Loi2"] = "Bạn chưa nhập cân nặng";
                
            }

            else if (float.Parse(cannang.ToString()) < 0)
            {
                ViewData["Loi2"] = "Bạn nhập sai cân nặng";
                cn = float.Parse(cannang.ToString());
                
            }
            else
            {
                bmii = float.Parse(bmi.ToString());
                caloo = float.Parse(calo.ToString());
                var benhnhan = (from b in data.BENHNHANs where b.IDTAIKHOAN == "duyluan0104" select b).Single();

                benhnhan.BMI = bmii;
                benhnhan.CALO = caloo;
                data.SubmitChanges();
                
            }    

            return this.TinhToanChiSo();
        }

    }
}