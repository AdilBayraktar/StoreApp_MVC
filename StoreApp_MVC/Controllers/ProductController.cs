using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp_MVC.Data;
using StoreApp_MVC.Models;
using StoreApp_MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnviroment = webHostEnvironment;
        }

        //Get All
        public IActionResult Index()
        {
            IEnumerable<Product> objectList = _context.Product.Include(p => p.Category).Include(p => p.Type);

            //foreach (var item in objectList)
            //{
            //    item.Category = _context.Category.FirstOrDefault(c => c.Id == item.CategoryId);
            //    item.Type = _context.Type.FirstOrDefault(c => c.Id == item.TypeId);
            //}
            return View(objectList);
        }

        //Get - UpdateInsert
        public IActionResult UpdateInsert(int? id)
        {
            //IEnumerable <SelectListItem> CategoryDropDownList = _context.Category.Select(c => new SelectListItem
            //{
            //    Text = c.Name,
            //    Value = c.Id.ToString()
            //});
            //ViewBag.CategoryDropDownList = CategoryDropDownList;


            //Product product = new Product();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _context.Category.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),

                TypeSelectList = _context.Type.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id.ToString()
                })
            };

            if (id == null)
            {
                //Create Product
                return View(productVM);
            }
            else
            {
                //Update Product
                productVM.Product = _context.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //Post - UpdateInsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateInsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnviroment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    string upload = webRootPath + WebConstance.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.Image = fileName + extension;
                    _context.Product.Add(productVM.Product);
                }
                else
                {
                    var objectFromDatabase = _context.Product.AsNoTracking().FirstOrDefault(p => p.Id == productVM.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstance.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objectFromDatabase.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objectFromDatabase.Image;
                    }
                    _context.Product.Update(productVM.Product);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        // Get - Delete
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _context.Product.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Post - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var result = _context.Product.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            string upload = _webHostEnviroment.WebRootPath + WebConstance.ImagePath;
            var oldFile = Path.Combine(upload, result.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _context.Product.Remove(result);
                _context.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}
