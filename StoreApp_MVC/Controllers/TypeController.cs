using Microsoft.AspNetCore.Mvc;
using StoreApp_MVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp_MVC.Controllers
{
    public class TypeController : Controller
    {
        private readonly AppDbContext _context;

        public TypeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<StoreApp_MVC.Models.Type> objectList = _context.Type;
            return View(objectList);
        }

        //Get-CreateType
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoreApp_MVC.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _context.Type.Add(type);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
            
        }

        //Get-EditType
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _context.Type.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoreApp_MVC.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _context.Type.Update(type);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }

        //Get-Delete
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _context.Type.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var result = _context.Type.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            _context.Type.Remove(result);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
