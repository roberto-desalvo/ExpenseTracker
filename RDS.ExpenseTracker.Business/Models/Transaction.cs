namespace RDS.ExpenseTracker.Business.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public CategoryEnum Category { get; set; } 
        public int FinancialAccountId { get; set; }
        public string FinancialAccountName { get; set; }
        public int? MoneyTransferId { get; set; }
    }
}
