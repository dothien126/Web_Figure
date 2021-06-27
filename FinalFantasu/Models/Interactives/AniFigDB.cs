using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalFantasu.Models;
using FinalFantasu.Models.Interactives;
using FinalFantasu.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IO;

namespace FinalFantasu.Models.Interactives
{
    public class BillItem
    {
        public int id { get; set; }
        public string figName { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
    public class ListFigure
    {


        public int pages { get; set; }

        public Publisher cate { get; set; }

        public List<Figure> Figures { get; set; }

        public ListFigure(int pages, List<Figure> Figures)
        {
            this.pages = pages;
            this.Figures = Figures;
        }
    }

    public class AniFigDB
    {
        AniFigure_DB aniFigDB = new AniFigure_DB();

        public AniFigDB()
        {

        }
        public ListFigure GetListFig(int page = 1, string text = "", int cate = 0, int sort = 0, int pageSize = 10, int type = 0, int priceFrom = 0, int priceTo = 0)
        {

            var removeUnicode = HelpFunction.RemoveUnicode(text);
            var listItem = aniFigDB.Figures.Where(x => x.isHidden != 1);
            listItem = listItem.Where(x => x.KeySearch.Contains(removeUnicode));
     
            if (cate != 0)
                listItem = listItem.Where(x => x.idPublisher == cate);

            if (type != 0)
                listItem = listItem.Where(x => x.idType == type);

            if (priceTo > 0)
                listItem = listItem.Where(x => (x.Price > priceFrom && x.Price < priceTo));
            else if (priceFrom > 0)
                listItem = listItem.Where(x => x.Price > priceFrom);

            switch (sort)
            {
                case 0:
                    listItem = listItem.OrderByDescending(x => x.ID);
                    break;
                case 1:
                    listItem = listItem.OrderBy(x => x.ID);
                    break;
                case 2:
                    listItem = listItem.OrderBy(x => x.Price);
                    break;
                case 3:
                    listItem = listItem.OrderByDescending(x => x.Price);
                    break;
            }

            int maxPage = (int)Math.Ceiling((double)(listItem.Count() / pageSize));
            return new ListFigure(maxPage, listItem.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
        public List<Figure> GetHomePage()
        {
            List<Figure> figures = new List<Figure>();                                               
            return figures;
        }
        public List<Publisher> GetCategories()
        {
            return aniFigDB.Publishers.ToList();
        }
        public List<Type> GetType()
        {
            return aniFigDB.Types.ToList();
        }

        public List<User> GetUsers()
        {
            return aniFigDB.Users.Where(x => !x.ID.Equals(SessionUtils.ID_ADMIN)).ToList();
        }

        public Figure GetFigureDetails(int id)
        {
            try
            {
                return aniFigDB.Figures.Where(x => x.ID == id).First();
            }
            catch
            {
                return null;
            }
        }


        public User Login(string email,string password)
        {
            string hashpass = HelpFunction.sha256(password);
            var users = aniFigDB.Users.Where(x => x.Email == email && x.Password == hashpass);
            if (users.Count() > 0)
            {
                User user = users.First();
                //remove password when return to client
                user.Password = "";
                return user;
            }
            return null;
        }


       
        public User Register(string fullname, string email, string password, string gender, string phone, string birthday)

        {
            if (aniFigDB.Users.Where(x => x.Email == email).Count() > 0)
                return null;
            User user = new User();

            user.Fullname = fullname;
            user.Email = email;
            user.Password = HelpFunction.sha256(password);
            user.Gender = gender;
            user.Birthday = Convert.ToDateTime(birthday);
            user.Phone = phone;
            user.is_ban = 0;
            aniFigDB.Users.Add(user);
            aniFigDB.SaveChanges();
            //remove password when return to client
            user.Password = "";
            return user;
		}

        public Cart AddCart(int idfig, int idUser, int quantity = 1)
        {
            Cart cart;
            var find_cart = aniFigDB.Carts.Where(x => x.idFigure == idfig && x.idUser == idUser);
            if (find_cart.Count() > 0)
            {
                find_cart.First().Quantity += quantity;
                cart = find_cart.First();
            }
            else
            {
                cart = new Cart();
                cart.idFigure = idfig;                
                cart.Quantity = quantity;
                cart.idUser = idUser;
                aniFigDB.Carts.Add(cart);
            }
            aniFigDB.SaveChanges();
            return cart;
        }
        public void DeleteCartItem(int idUser, int idfig)
        {
            Cart deleteItem = aniFigDB.Carts.Where(x => x.idUser == idUser && x.idFigure == idfig).First();
            aniFigDB.Carts.Remove(deleteItem);
            aniFigDB.SaveChanges();
        }
        public void ChangeAmount(int idCart, int amount)
        {
            aniFigDB.Carts.Where(x => x.ID == idCart).First().Quantity = amount;
            aniFigDB.SaveChanges();
        }

        public List<Cart> GetListCarts(int idUser)
        {
            var listCarts = aniFigDB.Carts.Where(x => x.idUser == idUser).ToList();
            return listCarts;
        }

        public User GetUserDetail(int id)
        {
            var user = aniFigDB.Users.Where(x => x.ID == id).First();
            return user;
        }

        public Figure EditFig(int ID,
            IEnumerable<int> images_delete,
            IEnumerable<HttpPostedFileBase> Images,
            string Title,
            int Price,
            string ReleaseDate,
            int Quantity,
            string Description,
            int idPublisher,
            int idType
            )
        {
            if (images_delete != null)
                aniFigDB.Images.RemoveRange(aniFigDB.Images.Where(x => images_delete.Contains(x.IDImage)));
            Figure fig = aniFigDB.Figures.Where(x => x.ID == ID).First();
            fig.Title = Title;
            fig.Price = Price;
            fig.ReleaseDate = Convert.ToDateTime(ReleaseDate);
            fig.Quantity = Quantity;
            fig.Description = Description;
            fig.idPublisher = idPublisher;
            fig.idType = idType;
            aniFigDB.SaveChanges();
            if (Images != null && Images.Count() > 0)
            {
                foreach (var image in Images)
                {
                    try
                    {
                        if (image != null)
                        {
                            MemoryStream target = new MemoryStream();
                            image.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();
                            var client = new RestClient("http://128.199.108.177:8001/upload_image");
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "multipart/form-data");
                            request.AlwaysMultipartFormData = true;
                            request.AddFile("book_cover", data, "image.jpeg");
                            IRestResponse response = client.Execute(request);
                            string resJsonRaw = response.Content;
                            JObject json = JObject.Parse(resJsonRaw);
                            Image imgObj = new Image();
                            imgObj.idFigure = ID;
                            imgObj.Url = json.GetValue("url").ToString();
                            aniFigDB.Images.Add(imgObj);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            aniFigDB.SaveChanges();
            return fig;
        }
        public bool DeleteFig(int id, bool permanently = false)
        {
            var item = aniFigDB.Figures.Where(x => x.ID == id).First();
            if (item != null)
                if (!permanently)
                {
                    item.isHidden = 1;
                }
                else
                {
                    aniFigDB.Carts.RemoveRange(aniFigDB.Carts.Where(x => x.idFigure == id));
                    aniFigDB.Images.RemoveRange(aniFigDB.Images.Where(x => x.idFigure == id));
                    aniFigDB.OrderDetails.RemoveRange(aniFigDB.OrderDetails.Where(x => x.idFigure == id));
                    aniFigDB.Figures.Remove(item);
                }
            aniFigDB.SaveChanges();
            return true;
        }
        public Figure AddFig(IEnumerable<HttpPostedFileBase> Images,
            string Title,
            int Price,
            string ReleaseDate,
            int Quantity,
            string Description,
            int idPublisher,
            int idType)
        {
            Figure fig = new Figure();
            fig.Title = Title;
            fig.Price = Price;
            fig.ReleaseDate = Convert.ToDateTime(ReleaseDate);
            fig.Quantity = Quantity;
            fig.Description = Description;
            fig.idPublisher = idPublisher;
            fig.idType = idType;
            aniFigDB.Figures.Add(fig);
            aniFigDB.SaveChanges();
            if (Images != null && Images.Count() > 0)
            {
                foreach (var image in Images)
                {
                    try
                    {
                        MemoryStream target = new MemoryStream();
                        image.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        var client = new RestClient("http://128.199.108.177:8001/upload_image");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "multipart/form-data");
                        request.AlwaysMultipartFormData = true;
                        request.AddFile("book_cover", data, "image.jpeg");
                        IRestResponse response = client.Execute(request);
                        string resJsonRaw = response.Content;
                        JObject json = JObject.Parse(resJsonRaw);
                        Image imgObj = new Image();
                        imgObj.idFigure = fig.ID;
                        imgObj.Url = json.GetValue("url").ToString();
                        aniFigDB.Images.Add(imgObj);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            aniFigDB.SaveChanges();
            return fig;
        }
        public void BanUser(int ID, int ban = 0)
        {
            User user = aniFigDB.Users.Where(x => x.ID == ID).First();
            if (user == null)
                return;
            user.is_ban = ban;
            aniFigDB.SaveChanges();
        }
        public User UpdateUser(string Phone, string Email, string Fullname, string Gender, string Birthday)
        {
            User user = aniFigDB.Users.Where(x => x.Phone == Phone).First();
            if (user == null)
                return null;
            user.Email = Email;
            user.Fullname = Fullname;
            user.Gender = Gender;
            user.Birthday = Birthday.Length == 0 ? DateTime.Now : Convert.ToDateTime(Birthday);
            aniFigDB.SaveChanges();
            return user;
        }
        public void UpdateCategory(int id, string name)
        {
            Publisher cate = aniFigDB.Publishers.Where(x => x.ID == id).First();
            if (cate == null)
                return;
            cate.Name = name;
            aniFigDB.SaveChanges();
        }
        public void AddCategory(string name)
        {
            if (aniFigDB.Publishers.Where(e => e.Name.Equals(name)).Count() > 0)
                return;
            Publisher cate = new Publisher();
            cate.Name = name;
            aniFigDB.Publishers.Add(cate);
            aniFigDB.SaveChanges();
        }
        public List<BillItem> GetBillItems(int[] ids)
        {

            List<BillItem> billItems = new List<BillItem>();


            foreach (var id in ids)
            {

                BillItem billItem = new BillItem();
                Cart cart = aniFigDB.Carts.Where(x => x.ID == id).ToList().First();
                billItem.id = cart.ID;
                billItem.price = (int)cart.Figure.Price;
                billItem.quantity = (int)cart.Quantity;
                billItem.figName = cart.Figure.Title;
                billItems.Add(billItem);
            }

            return billItems;
        }

        public List<Address> GetAddressByUser(int idUser)
        {
            List<Address> addresses = new List<Address>();
            addresses = aniFigDB.Addresses.Where(x => x.idUser == idUser).ToList();
            return addresses;
        }

        public void RemoveCategory(int id)
        {
            try
            {
                Publisher publisher = aniFigDB.Publishers.Where(x => x.ID == id).First();
                aniFigDB.Publishers.Remove(publisher);
                aniFigDB.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        public void CheckOut(int[] id_cart, int idUser, int totalPrice, string fullName, string phone, string address)
        {
            Order order = new Order();
            order.CreateDate = DateTime.Now;
            order.idUser = idUser;           
            Address newAdress = new Address();
            newAdress.idUser = idUser;
            newAdress.Name = fullName;
            newAdress.Phone = phone;
            newAdress.Address1 = address;
            aniFigDB.Addresses.Add(newAdress);
			aniFigDB.SaveChanges();
			order.idAddress = newAdress.ID;
                           
            
            order.CustomerName = fullName;           
            order.TotalPrice = totalPrice;
            order.PaymentStatus = 0;                       
            aniFigDB.Orders.Add(order);
            aniFigDB.SaveChanges();
            foreach (var idItem in id_cart)
            {
                OrderDetail orderDetail = new OrderDetail();
                Cart cartItem = aniFigDB.Carts.Where(x => x.ID == idItem).First();
                orderDetail.idOrder = aniFigDB.Orders.OrderByDescending(x => x.ID).First().ID;
                orderDetail.idFigure = (int)cartItem.idFigure;
                orderDetail.Quantity = (int)cartItem.Quantity;
                aniFigDB.OrderDetails.Add(orderDetail);
                aniFigDB.Carts.Remove(cartItem);
            }
            aniFigDB.SaveChanges();
        }       

        public Address GetAddressByIdAddress(int? idAddress)
        {
            return aniFigDB.Addresses.Where(x => x.ID == idAddress).First();
        }
        public List<Order> GetProcessingOrders(int id = -1)
        {
            List<Order> orders = id.Equals(-1) ?
                aniFigDB.Orders.Where(x => x.PaymentStatus == 0).ToList()
                : aniFigDB.Orders.Where(x => x.idUser == id && x.PaymentStatus == 0).ToList();
            orders = orders.OrderByDescending(x => x.CreateDate).ToList();
            return orders;
        }

        public List<Order> GetDoneOrders(int id = -1)
        {
            List<Order> orders = id.Equals(-1) ?
                aniFigDB.Orders.Where(x => x.PaymentStatus == 1).ToList()
                : aniFigDB.Orders.Where(x => x.idUser == id && x.PaymentStatus == 1).ToList();
            orders = orders.OrderByDescending(x => x.CreateDate).ToList();
            return orders;

        }
        public void ConfirmOrder(int id)
        {
            Order order = aniFigDB.Orders.Where(x => x.ID.Equals(id)).First();
            if (order != null)
            {
                order.PaymentStatus = 1;
                order.CreateDate = DateTime.Now;
                aniFigDB.SaveChanges();
            }
        }
    }
}