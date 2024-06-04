namespace RDS.ExpenseTracker.Data.Entities
{
    public class EFinancialAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Availability { get; set; }

        public ICollection<ETransaction>? Transactions { get; set; }
    }
}
