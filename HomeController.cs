using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using VegHouse.Areas.Identity.Data;
using VegHouse.Models;


namespace VegHouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly VegHouseDbContext _context;


        private Cart Cart { get; set; }
        public Product? Product { get; set; }

        public const string CartSessionKey = "CartId";
        public int ProductId { get; set; }
        public int CartDetailsId { get; set; }

        public int Quantity { get; set; }//Adet





        public HomeController(VegHouseDbContext context)
        {
            _context = context;
         
        }
        public IActionResult Index()
        {
            int id = 7; // Aromaterapi 7. kaetgori olduğu için 7 yaptık
            //ViewBag.Aromaterapi = _context.Products.Where(d => d.CategoryId == id).ToList(); //BURAYA BAKILACAK
            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            //List<Product> products = _context.Products.ToList();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //Kategoriye göre ürün getirme List
        public IActionResult List(int id)
        {
            //List<Product> products = _context.Products.Where(c => c.CategoryId == id).ToList();
            ViewBag.ProductList = _context.Products.ToList();
            ViewBag.CategoryId = id;

            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            return View();
            //return RedirectToAction("Index", "Home");

            //List<Product> products = _context.Products.Where(c => c.CategoryId == id).ToList();
            //return View(id);
        }

        //Arama
        public IActionResult Search(string searchString)
        {
            //ViewBag.Search = _context.Products.Where(c => c.ProductName.Contains(searchString) || c.ProductDescription.Contains(searchString)).ToList();
            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");

            List<Product> products = _context.Products.Where(c => c.ProductName.Contains(searchString) || c.ProductDescription.Contains(searchString)).ToList();
            ViewBag.AramaSonuc = products;
            return View();

        }


     


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.ProductList = _context.Products.ToList();
            ViewBag.ProductId = id;



            //ViewBag.Aromaterapi = _context.Products.Where(d => d.CategoryId == id).ToList(); //BURAYA BAKILACAK
            ViewBag.Aromaterapi = _context.Products.ToList();
            ViewBag.CartPartial = _context.CartDetails.ToList();
            ViewBag.CartPartial2 = _context.Carts.ToList();
            ViewBag.CartPartial3 = _context.Customers.ToList();
            ViewBag.CokSatanlar = _context.Products.ToList();

            ViewBag.KulaniciEmail = HttpContext.Session.GetString("CustomerEmail");
            ViewBag.KulaniciID = HttpContext.Session.GetInt32("CustomerID");


            //TempData["SepetMesaj2"] = "Ürün eklenmiştir.";
            return View();
        }




   

        public IActionResult AddCart(CartDetails CartDetails, int Id,int adet)
        {
         
            var usr = HttpContext.Session.GetInt32("CustomerID");
            if (usr != null)
            {
                var user = _context.Customers.FirstOrDefault(x => x.UserId == usr);
            }


            var TempCart = _context.Carts.FirstOrDefault(x => x.Customers.UserId == usr);
            if (TempCart==null)
            {
                
                Cart = new Cart
                {
                    Customers = _context.Customers.FirstOrDefault(x => x.UserId == usr),
                    IsDeleted = false
                    
                };
                _context.Add(Cart);
                
                _context.SaveChanges();

            }


            
            var TempCart2 = _context.Carts.FirstOrDefault(x => x.Customers.UserId == usr);


            var sep1 = _context.CartDetails.FirstOrDefault(
                c => c.Cart.CartId == TempCart2.CartId
                && c.ProductId == Id);



            if (sep1 == null)
            {
                              
                sep1 = new CartDetails
                {

                    Quantity = adet,
                    ProductPrice = _context.Products.FirstOrDefault(X=> X.ProductId == Id).ProductPrice,
                    CreateDate = DateTime.Now,
                    //CartId = TempCart.CartId,
                    Product = _context.Products.FirstOrDefault(
                    p => p.ProductId == Id),
                    Cart = TempCart2
                };

                _context.CartDetails.Add(sep1);
                _context.SaveChanges();
            }
           
            else
            {                               
                sep1.Quantity= sep1.Quantity+adet;           
                _context.Update(sep1);
                _context.SaveChanges();
            }

            //ViewBag.SepetMesaj = "Ürün eklenmiştir.";
            TempData["SepetMesaj2"] = "Urun eklendi.";


            return RedirectToAction("Index", "Home", new
            {
                Id = Id.ToString(),
             

            }) ; 




        }

        public IActionResult RemoveCart(CartDetails CartDetails, int Id)
        {
            var product = _context.CartDetails.FirstOrDefault(x => x.CartDetailsId == Id);
            if (product != null)
            {
                _context.CartDetails.Remove(product);
            }
            _context.SaveChanges();

            return RedirectToAction("Carts", "Customer");
            // Return mevcut sayfaya geri dönme. Kullanıcı hangi sayfada sildiyse ürünü yine o sayfa açılsın.

        }



        public decimal GetTotal()
        {
            //CartDetails = GetCartId();

            decimal? total = decimal.Zero;
            total = (decimal?)(from CartDetails in _context.CartDetails
                               where CartDetails.CartId == CartDetailsId
                               select (int?)CartDetails.Quantity *
                               CartDetails.Product.ProductPrice).Sum();
            return total ?? decimal.Zero;
        }

      


    }


}

