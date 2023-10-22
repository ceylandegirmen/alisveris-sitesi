using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using VegHouse.Areas.Identity.Data;
using VegHouse.Models;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Http;

namespace VegHouse.Controllers
{
    public class CustomerController : Controller

    {
        private readonly VegHouseDbContext _context;


        private Cart Cart { get; set; }
        private CartDetails CartDetails{ get; set; }
        //private Order Order { get; set; }
        public Order? Order { get; set; }
        private OrderDetails OrderDetails { get; set; }


        public const string CartSessionKey = "CartId";
        public int ProductId { get; set; }
        public int CartDetailsId { get; set; }

        public int Quantity { get; set; }//Adet


        public CustomerController(VegHouseDbContext context)
        {
            _context = context;
           
        }


        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.CartPartial = _context.CartDetails.ToList();
            
           

            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login([Bind("Email,Password")] Customers customers)
        {

            var datavalue = _context.Customers.FirstOrDefault(x => x.Email == customers.Email && x.Password == customers.Password);

            var User = from data in _context.Customers select data;
            User = User.Where(s => s.Email.Contains(customers.Email));



            if (User.Count() != 0)
            {
                if (User.First().Password == customers.Password)
                {

                    //HttpContext.Session.SetString("CustomerSession", JsonConvert.SerializeObject(customers));
                    HttpContext.Session.SetString("CustomerEmail", datavalue.Email);
                    HttpContext.Session.SetInt32("CustomerID", datavalue.UserId);

                    TempData["HataMesajLogin"] = "";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    TempData["HataMesajLogin"] = "Kullanıcı Adı yada Şifre Hatalı!";                    
                    return RedirectToAction("Login", "Customer");
                }
            }

            if (ModelState.IsValid)
            {
                var kullanici = _context.Customers.FirstOrDefault(i => i.Email == customers.Email && i.Password == customers.Password);
                return RedirectToAction("Index", "Home");
            }
            return View(customers);
        }






        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Name,SurName,Email,Password,RePassword")] Customers customers)
        {
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            try
            {
                var data = new Customers()
                {
                    UserId = customers.UserId,                                        
                    Name = customers.Name,
                    SurName = customers.SurName,
                    Email = customers.Email,
                    Password = customers.Password,
                    RePassword = customers.RePassword
                  

                };
                _context.Customers.Add(data);
                _context.SaveChanges();
                ViewBag.Status = 1;
                ViewBag.RegisterBasarili = "Kayıt başarılı";
            }
            catch (Exception)
            {
                ViewBag.Status = 0;
                ViewBag.RegisterHata = "Kayıt başarısız";
            }
            return View();


        }

        public IActionResult Pay([Bind("Ad,Soyad,Il,Ilce,Adres,Eposta,Tel,OrderPrice")] Order order)
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            ViewBag.Aromaterapi = _context.Products.ToList();

            ViewBag.CartPartial3 = _context.Customers.ToList();


           

            ViewBag.CokSatanlar = _context.Products.ToList();

            var usr = HttpContext.Session.GetInt32("CustomerID");
            if (usr != null)
            {
                var user = _context.Customers.FirstOrDefault(x => x.UserId == usr);
            }


            Random rastgele = new Random();
            string KargoNo = rastgele.Next(111111111, 999999999).ToString();


            Order = new Order // sipariş oluşuyor
            {
                Customers = _context.Customers.FirstOrDefault(x => x.UserId == usr),
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Ad = order.Ad,
                Soyad = order.Soyad,
                Adres = order.Adres,
                Il = order.Il,
                Ilce = order.Ilce,
                Eposta = order.Eposta,
                Tel = order.Tel,
                OrderPrice = order.OrderPrice,
                KargoAdi = "Mng Kargo",
                KargoTakipNo = KargoNo,
                OrderStatus = "Siparişiniz Hazırlanıyor..",
                OdemeYontemi = "Kredi Kartı"

            };
            _context.Add(Order);

            _context.SaveChanges();

            //var TempOrder2 = _context.Orders.FirstOrDefault(x => x.Customers.UserId == usr); // burası orderby ile yapılacak(en son order tespit edilecek)

            var TempOrder2 = _context.Orders.OrderByDescending(x => x.Customers.UserId == usr).ThenByDescending(x => x.CreateDate).FirstOrDefault(); // sonuna eklenen koda göre ürün getiriyor order a eklenen iki ürünün sonuncusunu getiriyor
            //List<Order> orders = _context.Orders.OrderByDescending(x => x.Customers.UserId == usr).ThenByDescending(x => x.CreateDate).FirstOrDefault();
           
            ViewBag.OrderPartial = TempOrder2;


            //List<Product> products = _context.Products.Where(c => c.ProductName.Contains(searchString) || c.ProductDescription.Contains(searchString)).ToList();
            //ViewBag.AramaSonuc = products;


            //ViewBag.OrderPartial = _context.Orders.ToList();
            ////var TempOrder2 = _context.Orders.OrderByDescending(i => i.CreateDate).ToList(); 

            var urunler = _context.CartDetails.Where((y => y.Cart.Customers.UserId == usr));

            foreach (var item in urunler)
            {
                OrderDetails = new OrderDetails
                {
                    Quantity = item.Quantity,
                    ProductPrice = item.ProductPrice,
                    Product = item.Product,
                    Order = TempOrder2
                };
                _context.Add(OrderDetails);
            }
            _context.SaveChanges();
            // burada sepet boşaltılacak
            var sepet = _context.Carts.FirstOrDefault(x => x.Customers.UserId == usr);
            if (sepet != null)
            {
                _context.Carts.Remove(sepet);
            }
            _context.SaveChanges();
            TempData["SepetMesaj2"] = "Siparişiniz oluşturuldu.";
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            //List<Product> products = _context.Products.ToList();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("CustomerEmail"); //VegCook
            return RedirectToAction("Login");

        }

        public IActionResult Carts()
        {
           

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();

            ViewBag.CokSatanlar = _context.Products.ToList();


            //List<Product> products = _context.Products.ToList();
            return View();
        }

        public IActionResult CheckOut()
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();


            var user = _context.Customers.FirstOrDefault(x => x.UserId == HttpContext.Session.GetInt32("CustomerID"));

            ViewBag.isim = user.Name;
            ViewBag.soyisim = user.SurName;
            ViewBag.email = user.Email;
            ViewBag.adresim = user.Address;
            ViewBag.tel = user.Gsm;

            return View();
        }


        public IActionResult PaySusses()
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();

            return RedirectToAction("Pay_Susses", "Customer", new
            {

            });
        }


        public IActionResult Pay_Susses()
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();


            return View();
        }

        public IActionResult MyOrders ()
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");
            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();

            ViewBag.Siparislerim = _context.Orders.ToList();

            return View();
        }


        public IActionResult MyOrderDetails(int id)
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");
            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();

            ViewBag.Siparislerim = _context.Orders.ToList();
            ViewBag.SiparisDetaylarim = _context.OrderDetails.ToList();
            ViewBag.Siparisim = id;


            return View();
        }



        public IActionResult Account()
        {

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();

            ViewBag.CokSatanlar = _context.Products.ToList();

            var user = _context.Customers.FirstOrDefault(x => x.UserId == HttpContext.Session.GetInt32("CustomerID"));


            ViewBag.isim = user.Name;
            ViewBag.soyisim = user.SurName;
            ViewBag.email = user.Email;
            ViewBag.adresim = user.Address;
            ViewBag.tel = user.Gsm;

            //List<Product> products = _context.Products.ToList();
            return View();
        }

    


    }
}
