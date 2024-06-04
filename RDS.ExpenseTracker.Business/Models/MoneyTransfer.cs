namespace RDS.ExpenseTracker.Business.Models
{
    public class MoneyTransfer
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int DepositId { get; set; }
        public int WithdrawId { get; set; }
    }
}
