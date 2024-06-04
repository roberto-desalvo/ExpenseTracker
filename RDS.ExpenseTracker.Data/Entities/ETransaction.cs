namespace RDS.ExpenseTracker.Data.Entities
{
    public class ETransaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public DateTime? Date {  get; set; }
        public int FinancialAccountId { get; set; }
        public EFinancialAccount FinancialAccount { get; set; }
        public EMoneyTransfer WithdrawMoneyTransfer { get; set; }
        public EMoneyTransfer DepositMoneyTransfer { get; set; }
    }
}
