namespace RDS.ExpenseTracker.Data.Entities
{
    public class ETransaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime? Date {  get; set; }
        public int FinancialAccountId { get; set; }
        public EFinancialAccount FinancialAccount { get; set; }
        public bool IsTransfer {  get; set; }
    }
}
