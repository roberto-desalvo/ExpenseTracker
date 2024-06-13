namespace RDS.ExpenseTrackerApi.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public int FinancialAccountId { get; set; }
    }
}
