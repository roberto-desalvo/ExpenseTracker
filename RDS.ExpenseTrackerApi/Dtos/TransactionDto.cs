namespace RDS.ExpenseTrackerApi.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public int FinancialAccountId { get; set; }
    }
}
