using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1bite.Controllers
{
    public class HomeController : Controller
    {
        public new void Session()
        {

        }
        public ActionResult Index()
        {
            ViewBag.Message = "Your manage page.";
            if (Request.Cookies["dangnhap"] == null)
            {
                return View();
            }
            return View("LoggedIn");
        }
        public ActionResult LoggedIn()
        {
            ViewBag.Message = "Your manage page.";
            if (Request.Cookies["dangnhap"] != null)
            {
                return View();
            }
            return RedirectToAction("Index");
        }
        

        public ActionResult Login()
        {
            ViewBag.Message = "Your login page.";
            if (Request.Cookies["dangnhap"] == null)
            {
                return View();
            }
            return View("Error");
        }
        public ActionResult Logout()
        {
            HttpCookie userinfo1 = new HttpCookie("dangnhap");
            userinfo1.Expires = DateTime.Now.AddDays(-12);
            Response.Cookies.Add(userinfo1);
            //làm nút đăng xuất la dc
            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string d = form["txtUser"];
            string p = form["txtpass"];
            if (AccountDAO.AccountVerify(d, p))
            {
                HttpCookie userinfo = new HttpCookie("dangnhap");
                userinfo["username"] = d;
                Response.Cookies.Add(userinfo); 
                return RedirectToAction("Manage");
            }
            return RedirectToAction("index");
        }
        /*Đầu tiên, một người dùng sẽ chạy đường dẫn http://localhost:xxxx/Dammio/Index, 
         * server sẽ dò tìm và biết được đường dẫn này thuộc về tập tin DammioController.cs 
         * với phương thức Index(). Tiếp tục, server sẽ thực thi nội dung trong phương thức Index() 
         * và trả về dòng lệnh return View(). Vì đây là phương thức Index trả về View, server sẽ 
         * dò tìm đúng tập tin có tên Index.cshtml nằm trong thư mục Views/Dammio. 
         * Tiếp theo, server đọc nội dung Index.cshtml và hiển thị mã HTML trên màn hình.*/
        public ActionResult Banhang() //được gọi ở Manage (Home/Banhang)
        {
            ViewBag.Message = "Your manage page.";
            if (Request.Cookies["dangnhap"] != null)
            {
                return View();
            }
            return View("Error");
        }
        public ActionResult Nhaphang()
        {
            ViewBag.Message = "Your manage page.";
            if (Request.Cookies["userinfo"] != null)
            {
                return View();
            }
            return View("Error");
        }
        public ActionResult Quanli()
        {
            ViewBag.Message = "Your manage page.";
            if (Request.Cookies["dangnhap"] != null)
            {
                return View();
            }
            return View("Error");
        }
        public ActionResult Stats()
        {
            ViewBag.Message = "Your Stats page.";
            if (Request.Cookies["dangnhap"] != null)
            {
                return View();
            }
            return View("Error");
        }

        public ActionResult Manage()
        {
            ViewBag.Message = "Your manage page.";
            if (Request.Cookies["dangnhap"] != null) 
            {
                return View();
            }
            return View("Error");
        }
        public ActionResult Menu()
        {
            ViewBag.Message = "Your menu page.";

            return View();
        }
        public ActionResult Cart()
        {
            ViewBag.Message = "Your cart page.";

            return View();
        }

    }
}