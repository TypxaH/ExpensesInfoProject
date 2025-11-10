using System.ComponentModel.DataAnnotations;

namespace ExpensesInfo.Models
{
    public class ExpenseType
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Type of the expense")]
        public string? Name { get; set; } = null;
    }
}
