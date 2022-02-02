using Microsoft.AspNetCore.Mvc;
using StoreApp_MVC.Data;
using StoreApp_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        //Get All
        public IActionResult Index()
        {
            IEnumerable<Category> objectList = _context.Category;
            return View(objectList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        // Get - Edit
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _context.Category.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        // Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // Get - Delete
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _context.Category.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        // Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var result = _context.Category.Find(id);
            if (result == null)
            {
                return NotFound();
            }
                _context.Category.Remove(result);
                _context.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}
