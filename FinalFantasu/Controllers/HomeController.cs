using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalFantasu.Models;
using FinalFantasu.Models.Interactives;
using FinalFantasu.Utils;

namespace FinalFantasu.Controllers
{
	public class HomeController : Controller
	{
		AniFigDB db = new AniFigDB();
		AniFigure_DB db_u = new AniFigure_DB();
	
		public ActionResult Intro()
        {
			return View();
        }		
		public ActionResult FigureCate()
        {
			return View();
        }
		public ActionResult FakeFig()
        {
			return View();
        }		
		public ActionResult AFQ()
        {
			return View();
        }
		public ActionResult Blogs()
        {
			return View();
        }
		public ActionResult Error()
        {
			return View();
        }
		public ActionResult Index()
		{		
			var sample = db_u.Figures.Take(30).ToList();
			ViewBag.sample = sample;
			ViewBag.TotalUser = db_u.Users.Count();
			ViewBag.ActionNum = db_u.Figures.Where(x => x.idType == 1).Count();
			ViewBag.ScaleNum = db_u.Figures.Where(x => x.idType == 2).Count();
			ViewBag.ChibiNum = db_u.Figures.Where(x => x.idType == 3).Count();
			db.GetHomePage();
			return View();
		}

		public ActionResult ModalLogin()
		{
			User user = (User)Session[SessionUtils.SESSION.UserInfo];
			return View(user);
		}
		public ActionResult ModalInfo()
        {
			return View();
        }
		public ActionResult ModalRegister()
		{
			return View();
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
					return RedirectToAction("Login", "User");
				}
				FormsAuthentication.SetAuthCookie("" + user.ID, true);
				Session[SessionUtils.SESSION.UserInfo] = user;
			}
			else
			{
				TempData[SessionUtils.TEMPDATA.Message] = "Sai số điện thoại hoặc mật khẩu";
				return RedirectToAction("Login", "User");
			}
			if (user.ID.Equals(SessionUtils.ID_ADMIN)) {
				return RedirectToAction("Figure", "Admin");
			}

			return RedirectToAction("Index", "Home");
		}	
		public ActionResult RegisterAction(string fullname, string email, string password, string gender, string phone, string birthday)
		{
			User user = db.Register(fullname,email,password,gender,phone,birthday);

			if (user != null)
			{
				Session[SessionUtils.SESSION.UserInfo] = user;
			}
			else
			{
				TempData[SessionUtils.TEMPDATA.Message] = "Số điện thoại đã tồn tại";
				return RedirectToAction("Register", "User", new { fullname, email, password, gender, phone, birthday });
			}

			return RedirectToAction("Index", "Home");
		}	
		public ActionResult Logout()
		{
			Session[SessionUtils.SESSION.UserInfo] = null;
			FormsAuthentication.SignOut();
			Session.Abandon();
			return RedirectToAction("Index", "Home");			
		}

		public ActionResult AboutUs()
		{
			return View();
		}
	}
}