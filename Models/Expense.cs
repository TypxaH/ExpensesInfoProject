using System.ComponentModel.DataAnnotations;

namespace ExpensesInfo.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Range(0,double.MaxValue)]
        public decimal Value { get; set; }
        [Required, MinLength(3)]
        public string? Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Display(Name = "Type of expense")]
        [Required]
        public int ExpenseTypeId { get; set; }

        public ExpenseType? ExpenseType { get; set; }
    }
}
