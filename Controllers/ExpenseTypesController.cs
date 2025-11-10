using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesInfo.Models;

namespace ExpensesInfo.Controllers
{
    public class ExpenseTypesController : Controller
    {
        private readonly ExpensesInfoDbContext _context;

        public ExpenseTypesController(ExpensesInfoDbContext context)
        {
            _context = context;
        }

        // Списък с всички видове
        public IActionResult Index()
        {
            var types = _context.ExpenseTypes.ToList();
            return View(types);
        }

        // Форма за създаване / редакция
        public IActionResult CreateEdit(int? id)
        {
            if (id == null)
            {
                return View(new ExpenseType());
            }

            var type = _context.ExpenseTypes.SingleOrDefault(x => x.Id == id);
            if (type == null) return NotFound();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ExpenseType model)
        {
            /*if (!ModelState.IsValid)
            {
                return View(model);
            }*/

            if (model.Id == 0)
            {
                _context.ExpenseTypes.Add(model);
            }
            else
            {
                var existing = _context.ExpenseTypes.SingleOrDefault(x => x.Id == model.Id);
                if (existing == null) return NotFound();

                existing.Name = model.Name;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Изтриване
        public IActionResult Delete(int id)
        {
            var type = _context.ExpenseTypes.SingleOrDefault(x => x.Id == id);
            if (type == null) return NotFound();

            _context.ExpenseTypes.Remove(type);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
