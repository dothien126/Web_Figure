using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalFantasu.Models.Interactives;
using FinalFantasu.Models;
using FinalFantasu.Utils;
using FinalFantasu.Controllers;

namespace FinalFantasu.Areas.Admin.Controllers
{
    //[Authorize(Users = "admin")]
    public class AdminController : BaseController
    {

        AniFigDB db = new AniFigDB();
        AniFigure_DB db_u = new AniFigure_DB();
        public ActionResult Index()
        {
            return RedirectToAction("Figure", "Admin");
        }
        // GET: Admin
        public ActionResult Figure(int page = 1, string text = "", int cate = 0, int sort = 0, int pageSize = 12, int type = 0, int priceFrom = 0, int priceTo = 0)
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
            switch (sort)
            {
                case 0:
                    ViewBag.TextSort = SessionUtils.DROPDOWN_SORT.NEWEST;
                    break;
                case 1:
                    ViewBag.TextSort = SessionUtils.DROPDOWN_SORT.OLDEST;
                    break;
                case 2:
                    ViewBag.TextSort = SessionUtils.DROPDOWN_SORT.LOWEST_PRICE;
                    break;
                case 3:
                    ViewBag.TextSort = SessionUtils.DROPDOWN_SORT.HIGHEST_PRICE;
                    break;
            }

            if (pageSize != 12 && pageSize % 12 == 0 && pageSize <= 64)
            {
                ViewBag.PageSize = pageSize;
            }
            ListFigure listFig = db.GetListFig(page, text, cate, sort, pageSize, type, priceFrom, priceTo);
            ViewBag.ListPage = HelpFunction.getNumPage(page, listFig.pages);
            ViewBag.maxPage = listFig.pages;
            ViewBag.TextSearch = text;
            ViewBag.list = listFig.Figures;
            return View();
        }


        public ActionResult Order()
        {
            List<Order> processingOrder = db.GetProcessingOrders();
            List<Order> doneOrder = db.GetDoneOrders();
            ViewBag.ProcessingOrders = processingOrder;
            ViewBag.ProcessingOrdersAddresses = processingOrder.Select(x => db.GetAddressByIdAddress(x.idAddress)).ToList();
            ViewBag.DoneOrders = doneOrder;
            ViewBag.DoneOrdersAddresses = doneOrder.Select(x => db.GetAddressByIdAddress(x.idAddress)).ToList();
            return View();
        }


        public ActionResult User()
        {
            ViewBag.list = db.GetUsers();
            return View();
        }
        public ActionResult Category()
        {
            ViewBag.list = db.GetCategories();
            return View();
        }
        public ActionResult Edit(int id = -1)
        {
            ViewBag.listCategories = db.GetCategories();
            ViewBag.listType = db.GetType();
            try
            {
                if (id == -1)
                    throw new Exception("Not found");
                var item = db.GetFigureDetails(id);
                return View(item);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public ActionResult EditFig(int ID,
             IEnumerable<HttpPostedFileBase> Images,
             IEnumerable<int> images_delete,
            string Title,
            int Price,
            string ReleaseDate,
            int Quantity,
            string Description,
            int idPublisher,
            int idType)
        {
            if (Price < 0 || Quantity < 0) {
                TempData["Message"] = "Lỗi nhập liệu, vui lòng kiểm tra lại thông tin (tiền âm,...)";
                return RedirectToAction("Edit", "Admin");
            }
            db.EditFig(ID, images_delete, Images, Title, Price, ReleaseDate, Quantity, Description, idPublisher, idType);
            return RedirectToAction("Figure", "Admin");
        }

        [HttpPost]
        public ActionResult Delete(int id = -1)
        {
            try
            {
                if (id == -1)
                    throw new Exception("Not found");
                db.DeleteFig(id, false);
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Figure", "Admin");
        }


        [HttpPost]
        public ActionResult AddFig(
            IEnumerable<HttpPostedFileBase> Images,
            string Title,
            int Price,
            string ReleaseDate,
            int Quantity,
            string Description,
            int idPublisher,
            int idType,
            int isHidden = 0,
            string keySearch = "figure test")
        {
            if (Price < 0 || Quantity < 0)
            {
                TempData["Message"] = "Lỗi nhập liệu, vui lòng kiểm tra lại thông tin (tiền âm,...)";
                return RedirectToAction("Add", "Admin");
            }
            Figure fig = db.AddFig(Images, Title, Price, ReleaseDate, Quantity, Description, idPublisher, idType, isHidden, keySearch);
            return RedirectToAction("Figure", "Admin");
        }
        // new { id = fig.ID }

        public ActionResult Add()
        {
            ViewBag.listCategories = db.GetCategories();
            ViewBag.listType = db.GetType();
            return View();
        }

        public ActionResult BanUser(int id, int ban = 0)
        {
            db.BanUser(id, ban);
            return RedirectToAction("User", "Admin");
        }

        public ActionResult UpdateUser(string phone, string email, string fullname, string gender, string birthday)
        {
            db.UpdateUser(phone, email, fullname, gender, birthday);
            return RedirectToAction("User", "Admin");
        }

        public ActionResult UpdateCategory(int id, string name)
        {
            db.UpdateCategory(id, name);
            return RedirectToAction("Category", "Admin");
        }

        public ActionResult AddCategory(string name)
        {
            db.AddCategory(name);
            return RedirectToAction("Category", "Admin");
        }

        public ActionResult RemoveCategory(int id)
        {
            db.RemoveCategory(id);
            return RedirectToAction("Category", "Admin");
        }

        [HttpPost]
        public ActionResult ConfirmOrder(int id)
        {
            db.ConfirmOrder(id);
            return RedirectToAction("Order", "Admin");
        }
    }
}