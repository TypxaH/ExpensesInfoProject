using ExpensesInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpensesInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpensesInfoDbContext _context;

        public HomeController(ILogger<HomeController> logger,ExpensesInfoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Expenses(int? typeId)
        {
            // Зареждаме всички видове за филтъра
            var types = _context.ExpenseTypes.ToList();
            ViewBag.Types = types;
            ViewBag.SelectedTypeId = typeId;

            // Базова заявка
            var query = _context.Expenses
                .Include(e => e.ExpenseType)
                .AsQueryable();

            // Ако има избран тип – филтрираме
            if (typeId.HasValue)
            {
                query = query.Where(e => e.ExpenseTypeId == typeId.Value);
            }

            var allExpenses = query.ToList();
            ViewBag.TotalExpenses = allExpenses.Sum(e => e.Value);

            return View(allExpenses);
        }
        public IActionResult CreateEditExpense(int? Id)
        {
            var types = _context.ExpenseTypes.ToList();
            ViewBag.Types = types;
            if (Id != null)
            {                
                var expenseToCreate = _context.Expenses.SingleOrDefault(expense => expense.Id == Id);
                return View(expenseToCreate);

            }
            
            return View();
        }
        public IActionResult DeleteExpense(int id)
        {
            var expenseToDelete = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseToDelete);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            /*if (!ModelState.IsValid)
            {
               
                ViewBag.Types = _context.ExpenseTypes.ToList();
                return View("CreateEditExpense", model);
            }*/
            if (model.Id == 0)
            {
                _context.Expenses.Add(model);
            }
            else
            {
                _context.Update(model);
            }
                _context.SaveChanges();
            return RedirectToAction("Expenses");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
