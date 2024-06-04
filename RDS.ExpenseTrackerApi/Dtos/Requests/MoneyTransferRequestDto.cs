using RDS.ExpenseTracker.Business.Models;

namespace RDS.ExpenseTrackerApi.Dtos.Requests
{
    public class MoneyTransferRequestDto
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string? Description { get; set; }
        public int FinancialAccountFromId { get; set; }
        public int FinancialAccountToId { get; set; }
    }
}
