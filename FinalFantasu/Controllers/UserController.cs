using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalFantasu.Models;
using FinalFantasu.Models.Interactives;
using FinalFantasu.Utils;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalFantasu.Controllers
{
    public class UserController : Controller
    {
        AniFigDB db = new AniFigDB();
        AniFigure_DB db_u = new AniFigure_DB();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllProduct(int page = 1, string text = "", int cate = 0, int sort = 0, int pageSize = 12, int type = 0, int priceFrom = 0, int priceTo = 0)
        {
            ViewBag.ListCate = db.GetCategories();
            ViewBag.ListType = db.GetType();
            ViewBag.Cate = cate;
            ViewBag.Type = type;
            ViewBag.Sort = sort;
            
            if (priceFrom > priceTo)
                priceTo = 0;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;
 
            ViewBag.PageSize = 12;
            ViewBag.CurrentPage = page;           
            if (pageSize != 12 && pageSize % 12 == 0)
            {
                ViewBag.PageSize = pageSize;
            }
            var listfig = db.GetListFig(page, text, cate, sort, pageSize, type, priceFrom, priceTo);
            ViewBag.ListPage = HelpFunction.getNumPage(page, listfig.pages);
            ViewBag.maxPage = listfig.pages;
            ViewBag.TextSearch = text;
            //page = 2 & text = &type = 0 & cate = 0 & sort = 0 & pageSize = 12 & priceFrom = 0 & priceTo = 0
            return View(listfig.Figures);           
        }
        public ActionResult Details(int id = -1)
        {
            var itemfig = db.GetFigureDetails(id);
            if (itemfig == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var subs = itemfig.Description.ToString().Split(new string[] { ", " }, StringSplitOptions.None).ToList();
            ViewBag.TitleDes = subs[0];
            subs.RemoveAt(0);
            ViewBag.ContentDes = subs;
            var related = db_u.Figures.Take(4).ToList();
            ViewBag.related = related;
            return View(itemfig);
        }
        public ActionResult ShoppingCart()
        {
            User user = (User)Session[SessionUtils.SESSION.UserInfo];
            List<Cart> listCarts = db.GetListCarts(user.ID);
            return View(listCarts);            
        }
        public ActionResult AddToCart(int idfig, int quantity)
        {
            Figure fig = db_u.Figures.Where(x => x.ID == idfig).First();

            if (quantity < 1 || quantity > fig.Quantity) { 
                TempData["Message"] = "Lỗi số lượng đặt (Kho không đủ, lỗi nhập,...)!";
                return RedirectToAction("Details", "User", new { id = idfig });
            }
            User user = (User)Session[SessionUtils.SESSION.UserInfo];
            db.AddCart(idfig, user.ID ,quantity);
            TempData[SessionUtils.TEMPDATA.Message] = "Thêm vào giỏ hàng thành công";
            return RedirectToAction("Details", "User", new { id = idfig });
        }

        [HttpPost]
        public JsonResult ChangeAmount(int idCart, int amount)
        {
            try
            {
                db.ChangeAmount(idCart, amount);
                return Json(new { status = 1 });
            }
            catch (Exception)
            {
                return Json(new { status = 0 });
            }
        }
        public ActionResult Login()
        {
            return View();
		}
        [HttpPost]
        public ActionResult CheckOut(int[] id_cart, string totalPrice, string fullName, string phone,string address)
        {
            User user = (User)Session[SessionUtils.SESSION.UserInfo];
            int TotalPrice = int.Parse(totalPrice);
            db.CheckOut(id_cart, user.ID,TotalPrice,fullName, phone, address);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult LoginAction(string email, string password)
        {
            User user = db.Login(email, password);
            if (user != null)
            {
                if (user.is_ban == 1)
                {
                    TempData[SessionUtils.TEMPDATA.Message] = "Tài khoản của bạn đã bị khoá, vùi lòng liên hiện để biết thêm thông tin";
                    return RedirectToAction("LoginAction", "Home");
                }
                FormsAuthentication.SetAuthCookie("" + user.ID, true);
                Session[SessionUtils.SESSION.UserInfo] = user;
            }
            else
            {
                TempData[SessionUtils.TEMPDATA.Message] = "Sai số điện thoại hoặc mật khẩu";
                return RedirectToAction("LoginAction", "Home");
            }
            if (user.ID.Equals(SessionUtils.ID_ADMIN))
            {
                return RedirectToAction("Figure", "Admin", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            return View();
		}
        public ActionResult RegisterAction(string fullname, string email, string password, string gender, string phone, string birthday)
        {
            User user = db.Register(fullname, email, password, gender, phone, birthday);

            if (user != null)
            {
                Session[SessionUtils.SESSION.UserInfo] = user;
            }
            else
            {
                TempData[SessionUtils.TEMPDATA.Message] = "Số điện thoại đã tồn tại";
                return RedirectToAction("LoginAction", "Home", new { fullname, email, password, gender, phone, birthday });
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Infor()
        {
            User user = (User)Session[SessionUtils.SESSION.UserInfo];
            if (user == null) {
                return RedirectToAction("Index", "Home");
			}
            user.Password = "";
            return View(user);
        }

        public ActionResult DeleteCartItem(int idfig)
        {
            User user = (User)Session[SessionUtils.SESSION.UserInfo];
            db.DeleteCartItem(user.ID, idfig);
            return RedirectToAction("ShoppingCart", "User", new { idUser = user.ID });
        }
        public ActionResult Payment(int[] idCarts)
        {
            var listItems = db.GetBillItems(idCarts);
            User user = (User)Session[SessionUtils.SESSION.UserInfo];
            ViewBag.addresses = db.GetAddressByUser(user.ID);
            return View(listItems);
        }


        public ActionResult Logout()
        {
            Session[SessionUtils.SESSION.UserInfo] = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}