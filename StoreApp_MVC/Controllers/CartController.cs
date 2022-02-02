using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp_MVC.Data;
using StoreApp_MVC.Models;
using StoreApp_MVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp_MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null &&
                    HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart);
            }
            List<int> productInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _context.Product.Where(i => productInCart.Contains(i.Id));
            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null &&
                    HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(i => i.ProductId == id));
            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
