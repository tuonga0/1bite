using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using _1bite.Models;
using System.Net;
using System.Globalization;

namespace _1bite.Controllers
{
    public class HomeController : Controller
    {
        ViewModel mymodel = new ViewModel();
        public new void Session()
        {

        }
        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString();
        }
        public int getStaffRank()
        {
            int rank;
            string user_name;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            user_name = reqCookies["userName"].ToString();
            rank = AccountDAO.checkAccRank(user_name);
            return rank;
        }
        public bool isAdmin()
        {
            if (getStaffRank() == 1)
            {
                return true;
            }
            return false;
        }

        public ActionResult LoggedIn()
        {
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                user_name = reqCookies["userName"].ToString();
                ViewBag.messagestaffname = AccountDAO.GetStaffName(user_name);
            }
            return RedirectToAction("Index");
        }


        public ActionResult Login()
        {
            if (Request.Cookies["userinfo"] == null)
            {
                return View();
            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            HttpCookie userinfo1 = new HttpCookie("userinfo");
            userinfo1.Expires = DateTime.Now.AddDays(-12);
            Response.Cookies.Add(userinfo1);
            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string d = form["txtUser"];
            string p = form["txtpass"];
            if (AccountDAO.AccountVerify(d, p))
            {
                HttpCookie userinfo = new HttpCookie("userinfo");
                userinfo["username"] = d;
                userinfo.Expires.Add(new TimeSpan(0, 30, 0));
                Response.Cookies.Add(userinfo);
                return RedirectToAction("Manage");
            }
            return RedirectToAction("index");
        }
        public ActionResult Index()
        {
            //System.Web.HttpContext.Current.Session["dathang"] = null;
            if (Request.Cookies["userinfo"] == null)
            {
                return View();
            }
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                user_name = reqCookies["userName"].ToString();
            }
            ViewBag.messagestaffname = AccountDAO.GetStaffName(user_name);
            return View("LoggedIn");
        }

        public ActionResult Banhang()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                user_name = reqCookies["userName"].ToString();
            }
            if (AccountDAO.GetStaffId(user_name) == null)
            {
                var c = new HttpCookie("userinfo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                return View("Error");
            }
            ViewBag.messagestaffname = AccountDAO.GetStaffName(user_name);
            if (reqCookies != null)
            {
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                mymodel.Dish = AccountDAO.getDish();
                List<Order> lo = new List<Order>();
                List<OrderDetails> lod = new List<OrderDetails>();
                lo = AccountDAO.getOrdersToday();
                foreach (Order o in lo)
                {
                    o.staffName = AccountDAO.GetStaffNameWithID(o.staffid);
                    o.status = AccountDAO.GetStatusWithID(o.statusId);
                    if (o.note == "")
                    {
                        o.note = "Không có ghi chú";
                    }
                    lod = AccountDAO.getOrderDetailsWithId(o.id);
                    foreach (OrderDetails od in lod)
                    {
                        o.Total += od.amount * AccountDAO.getDishPrice(od.id);
                    }
                    o.Total -= o.discount;
                }
                mymodel.Order = lo;
                return View(mymodel);
            }
            return View("Error");
        }
        public ActionResult ViewAllOrders()
        {
            if (Request.Cookies["userinfo"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["userinfo"];
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                List<Order> lo = AccountDAO.getOrders();
                foreach (Order o in lo)
                {
                    o.staffName = AccountDAO.GetStaffNameWithID(o.staffid);
                    o.status = AccountDAO.GetStatusWithID(o.statusId);
                    if (o.note == "")
                    {
                        o.note = "Không có ghi chú";
                    }
                    List<OrderDetails> lod = AccountDAO.getOrderDetailsWithId(o.id);
                    foreach (OrderDetails od in lod)
                    {
                        o.Total += od.amount * AccountDAO.getDishPrice(od.id);
                    }
                    o.Total -= o.discount;
                }
                mymodel.Order = lo;
                return View(mymodel);
            }
            return View("Error");
        }
        public ActionResult ViewAllImport()
        {
            if (Request.Cookies["userinfo"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["userinfo"];
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                List<Import> lo = AccountDAO.getImport();
                foreach (Import o in lo)
                {
                    o.staffname = AccountDAO.GetStaffNameWithID(o.staffId);
                    List<ImportDetail> lod = AccountDAO.getImportDetailsWithId(o.id);
                    foreach (ImportDetail od in lod)
                    {
                        o.total += (od.amount * od.unitPrice);
                        o.source = AccountDAO.GetSourceNameWithID(o.sourceid);
                    }
                    o.total = o.total - o.overallDiscount + o.shipFee;
                }
                mymodel.Import = lo;
                return View(mymodel);
            }
            return View("Error");
        }
        public ActionResult Order()
        {
            return View();
        }
        public ActionResult Nhaphang()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                user_name = reqCookies["userName"].ToString();
            }
            if (AccountDAO.GetStaffId(user_name) == null)
            {
                var c = new HttpCookie("userinfo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                return View("Error");
            }
            ViewBag.messagestaffname = AccountDAO.GetStaffName(user_name);
            if (reqCookies != null)
            {
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                mymodel.Source = AccountDAO.GetSources();
                mymodel.Product = AccountDAO.GetProducts();
                List<Import> li = new List<Import>();
                List<ImportDetail> lid = new List<ImportDetail>();
                li = AccountDAO.getImportToday();
                foreach (Import i in li)
                {
                    i.staffname = AccountDAO.GetStaffNameWithID(i.staffId);
                    i.source = AccountDAO.GetSourceNameWithID(i.sourceid);
                    lid = AccountDAO.getImportDetailsWithId(i.id);
                    foreach (ImportDetail id in lid)
                    {
                        i.total += (id.amount * id.unitPrice) - id.discounted;
                    }
                    i.total -= i.overallDiscount;
                }
                mymodel.Import = li;
                return View(mymodel);
            }
            return View("Error");
        }
        public ActionResult QuanliDish()
        {
            if (Request.Cookies["userinfo"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["userinfo"];
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                if (isAdmin())
                {
                mymodel.Dish = AccountDAO.getDish();
                mymodel.DishType = AccountDAO.getDishType();
                return View(mymodel);
                }
                else
                {
                    return RedirectToAction("Manage");
                }
            }
            return View("Error");
        }
        public ActionResult QuanliProduct()
        {
            if (Request.Cookies["userinfo"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["userinfo"];
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                if (isAdmin())
                {
                    mymodel.Product = AccountDAO.GetProducts();
                    mymodel.Source = AccountDAO.GetSources();
                    return View(mymodel);
                }
                else
                {
                    return RedirectToAction("Manage");
                }
            }
            return View("Error");
        }
        public ActionResult QuanliAccount()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                user_name = reqCookies["userName"].ToString();
            }
            if (AccountDAO.GetStaffId(user_name) == null)
            {
                var c = new HttpCookie("userinfo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                return View("Error");
            }
            ViewBag.messagestaffname = AccountDAO.GetStaffName(user_name);
            if (reqCookies != null)
            {
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                if (isAdmin())
                {
                    mymodel.Rank = AccountDAO.getRank();
                    List<Account> la = new List<Account>();
                    la = AccountDAO.GetAccount();
                    foreach (Account a in la)
                    {
                        a.role = AccountDAO.getRankwithId(a.rank);
                    }
                    mymodel.Account = la;
                    return View(mymodel);
                }
                else
                {
                    return RedirectToAction("Manage");
                }
            }
            return View("Error");
        }
        
        public ActionResult Stats()
        {
            if (Request.Cookies["userinfo"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["userinfo"];
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                int profit = 0;
                List<OrderDetails> lod = new List<OrderDetails>();
                lod = AccountDAO.GetOrderDetailGrouped();
                foreach (OrderDetails od in lod)
                {
                    od.orderedAmount += od.amount;
                    od.name = AccountDAO.GetDishName(od.id.ToString());
                    od.moneyReceived = od.orderedAmount * AccountDAO.getDishPrice(od.id);
                    profit += od.moneyReceived;
                }
                mymodel.OrderDetails = lod;
                profit = profit - AccountDAO.GetAllDiscounted() - AccountDAO.GetSpend() + AccountDAO.GetBuyingDiscount();
                CultureInfo ci = new CultureInfo("vi-VN");
                mymodel.profit = profit.ToString("C", ci);
                return View(mymodel);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult addDish(int id,string name, int amount)
        {
            List<_1bite.Models.OrderDetails> odl = new List<OrderDetails>();
            if (System.Web.HttpContext.Current.Session["dathang"] != null)
            {
                odl = (List<OrderDetails>)System.Web.HttpContext.Current.Session["dathang"];
            }
            OrderDetails od;
            int dishprice = AccountDAO.GetDishPrice(id);
            if (odl.ToList().Any(m => m.id == id))
            {
                od = odl.Find(m => m.id == id);
                od.amount += amount;
                od.total = od.amount * dishprice;
            }
            else
            {
                od = new OrderDetails();
                od.id = id;
                od.name = name;
                od.amount = amount;
                od.price = dishprice;
                od.total = amount * dishprice;
                odl.Add(od);
            }
            System.Web.HttpContext.Current.Session["dathang"] = odl;
            mymodel.OrderDetails = odl;
            return PartialView(mymodel);
        }

        [HttpPost]
        public ActionResult addProduct(int id, int amount,int unitPrice, int discount)
        {
            List<_1bite.Models.ImportDetail> idl = new List<ImportDetail>();
            if (System.Web.HttpContext.Current.Session["nhaphangg"] != null)
            {
                idl = (List<ImportDetail>)System.Web.HttpContext.Current.Session["nhaphangg"];
            }
            ImportDetail importDetail;
            if (idl.ToList().Any(m => m.productId == id))
            {
                importDetail = idl.Find(m => m.productId == id);
                importDetail.amount += amount;
                importDetail.discounted = discount;
                importDetail.paid = (importDetail.amount * importDetail.unitPrice) - discount;
            }
            else
            {
                importDetail = new ImportDetail
                {
                    productId = id,
                    productName = AccountDAO.GetProductNameWithID(id),
                    amount = amount,
                    discounted = discount,
                    unitPrice = unitPrice,
                    paid = (amount * unitPrice) - discount
                };
                idl.Add(importDetail);
            }
            System.Web.HttpContext.Current.Session["nhaphangg"] = idl;
            mymodel.importDetail = idl;
            return PartialView(mymodel);
        }

        [HttpPost]
        public ActionResult ChangeTheValue(ViewModel model, int discount, string type)
        {
            return Json(Discount(discount, type));
        }
        [HttpPost]
        public ActionResult ImportDiscount(ViewModel model, int discount, int shipFee)
        {
            var m = new ViewModel();
            List<ImportDetail> del = (List<ImportDetail>)System.Web.HttpContext.Current.Session["nhaphangg"];
            if (del != null)
            {
                foreach (ImportDetail id in del)
                {
                    m.total += id.paid;
                }
            }
            m.total = m.total + shipFee;
            //return Json(bill - discount + shipFee);
            return Json(m.total);
        }
        public int Discount(int discount, string type)
        {
                var m = new ViewModel();
                int discounted;
                List<OrderDetails> del = (List<OrderDetails>)System.Web.HttpContext.Current.Session["dathang"];
                if (del != null)
                {
                    foreach (OrderDetails od in del)
                    {
                        m.total += od.total;
                    }
                }
                if (type == "%")
                {
                    discounted = m.total / 100 * discount;
                }
                else
                {
                    discounted = discount;
                }
                m.total = m.total - discounted;
                return m.total;
        }
        public int Discounted(int discount, string type)
        {
            var m = new ViewModel();
            int discounted;
            List<OrderDetails> del = (List<OrderDetails>)System.Web.HttpContext.Current.Session["dathang"];
            if (del != null)
            {
                foreach (OrderDetails od in del)
                {
                    m.total += od.total;
                }
            }
            if (type == "%")
            {
                discounted = m.total / 100 * discount;
            }
            else
            {
                discounted = discount;
            }
            return discounted;
        }
        [HttpPost]
        public ActionResult delDish(int id)
        {
            List<OrderDetails> del = (List<OrderDetails>)System.Web.HttpContext.Current.Session["dathang"];
            if(del != null && del.Count>0)
            {
                foreach (var d in del.ToList())
                {
                    if (del != null && del.Count() > 0)
                    {
                        if (d.id == id)
                        {
                            del.Remove(d);
                            if (del.Count() == 0)
                                break ;
                        }
                    }
                   
                }
            }
            System.Web.HttpContext.Current.Session["dathang"] = del;
            return PartialView(del);
        }
        public ActionResult delProduct(int id)
        {
            List<ImportDetail> del = (List<ImportDetail>)System.Web.HttpContext.Current.Session["nhaphangg"];
            if (del != null && del.Count > 0)
            {
                foreach (var d in del.ToList())
                {
                    if (del != null && del.Count() > 0)
                    {
                        if (d.productId == id)
                        {
                            del.Remove(d);
                            if (del.Count() == 0)
                                break;
                        }
                    }

                }
            }
            System.Web.HttpContext.Current.Session["nhaphangg"] = del;
            mymodel.importDetail = del;
            return PartialView(mymodel);
        }

        public ActionResult addOrder(int discount, string type, string note, string address)
        {
            List<OrderDetails> add = (List<OrderDetails>)System.Web.HttpContext.Current.Session["dathang"];
            int discounted = Discounted(discount, type);
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                user_name = reqCookies["userName"].ToString();
            }
            int staffId = Convert.ToInt32(AccountDAO.GetStaffId(user_name));
            AccountDAO.addOrder(staffId,discounted,note,6,address,add); //4 mean the dish is cooking 6 mean waiting for confirm in the kitchen
            return RedirectToAction("Banhang");
        }
        public ActionResult addImport(int discount, int shipFee, int sourceId)
        {
            List<ImportDetail> add = (List<ImportDetail>)System.Web.HttpContext.Current.Session["nhaphangg"];
            string user_name = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userinfo"];
            if (reqCookies != null)
            {
                user_name = reqCookies["userName"].ToString();
            }
            int staffId = Convert.ToInt32(AccountDAO.GetStaffId(user_name));
            AccountDAO.addImport(discount, shipFee, sourceId, staffId,add);
            return RedirectToAction("Nhaphang");
        }
        public ActionResult newProduct(string name, string unit)
        {
            AccountDAO.addProduct(name, unit);
            return RedirectToAction("QuanliProduct");
        }
        public ActionResult newDish(string name, int type,string des, int price)
        {
            AccountDAO.addDish(name, type,price,des);
            return RedirectToAction("QuanliDish");
        }
        public ActionResult newSource(string name, string address)
        {
            AccountDAO.addSource(name, address);
            return RedirectToAction("QuanliProduct");
        }
        public ActionResult DeleleDish(int id)
        {
            AccountDAO.deleteDish(id);
            return RedirectToAction("QuanliDish");
        }
        public ActionResult DeleleProduct(int id)
        {
            AccountDAO.deleteProduct(id);
            return RedirectToAction("QuanliProduct");
        }
        public ActionResult DeleleSource(int id)
        {
            AccountDAO.deleteSource(id);
            return RedirectToAction("QuanliProduct");
        }
        public ActionResult addAccount(string username, string password, string name, string phone, string email,int rankId)
        {
            AccountDAO.addAcc(username, password, name, phone, email, rankId);
            return RedirectToAction("QuanliAccount");
        }
        public ActionResult DeleteAccount(int id)
        {
            if (id != 2)
            {
                AccountDAO.deleteAccount(id);
                return RedirectToAction("QuanliAccount");
            }
            else
            {
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult DeleteOrder(int id)
        {
            AccountDAO.deleteOrders(id);
            return RedirectToAction("Banhang");
        }
        public ActionResult DeleteImport(int id)
        {
            AccountDAO.deleteImport(id);
            return RedirectToAction("Nhaphang");
        }
        [HttpPost]
        public ActionResult seeOrder(int id)
        {
            List<OrderDetails> lod2 = new List<OrderDetails>();
            lod2 = AccountDAO.getOrderDetailsWithId(id);
            foreach(OrderDetails od in lod2)
            {
                od.name = AccountDAO.GetDishName(od.id.ToString());
                od.price = AccountDAO.getDishPrice(od.id);
                od.total = od.amount * od.price;
            }
            mymodel.OrderDetails = lod2;
            return PartialView(mymodel);
        }
        public ActionResult seeImport(int id)
        {
            List<ImportDetail> lid2 = new List<ImportDetail>();
            lid2 = AccountDAO.getImportDetailsWithId(id);
            foreach (ImportDetail impd in lid2)
            {
                impd.productName = AccountDAO.GetProductNameWithID(impd.productId);
                impd.Unit = AccountDAO.GetProductUnitWithID(impd.productId);
                impd.paid = (impd.amount * impd.unitPrice) - impd.discounted;
            }
            mymodel.importDetail = lid2;
            return PartialView(mymodel);
        }
        [HttpPost]
        public ActionResult seeAccountInfo(int id)
        {
            List<Staff> ls = new List<Staff>();
            ls = AccountDAO.GetStaffWithId(id);  
            mymodel.Staff = ls;
            return PartialView(mymodel);
        }
        public ActionResult Manage()
        {
            if (Request.Cookies["userinfo"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["userinfo"];
                if (AccountDAO.GetStaffId(reqCookies["userName"].ToString()) == null)
                {
                    var c = new HttpCookie("userinfo");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                    return View("Error");
                }
                return View();
            }
            return View("Error");
        }
        public ActionResult updatePassword(int id, string pass)
        {
            AccountDAO.changePass(id,pass);
            return RedirectToAction("QuanliAccount");
        }
        public ActionResult updateStaff(int id, string name,string phone, string email)
        {
            AccountDAO.changeStaffInfo(id, name,phone,email);
            return RedirectToAction("QuanliAccount");
        }
        public ActionResult Cart()
        {
            ViewBag.Message = "Your cart page.";

            return View();
        }



    }
}