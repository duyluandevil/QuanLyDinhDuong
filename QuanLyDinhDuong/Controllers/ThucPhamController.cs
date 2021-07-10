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

            var thucphammoi = layThucPham(10);
            //var lstthucpham = (from ltp in data.THUCPHAMs select ltp).ToList();
            return View(thucphammoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult LoaiThucPham()
        {
            var loaithucpham = from ltp in data.LOAITHUCPHAMs select ltp;
            return PartialView(loaithucpham);
        }


        public ActionResult TPTheoLoai(int id)
        {
            var thucpham = from t in data.THUCPHAMs where t.MALOAITHUCPHAM == id select t;
            return View(thucpham);
        }

        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var thucphammoi = layThucPham(10);
            return View(thucphammoi.ToPagedList(pageNum, pageSize));
        }
    }
}