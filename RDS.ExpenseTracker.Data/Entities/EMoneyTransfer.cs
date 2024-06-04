namespace RDS.ExpenseTracker.Data.Entities
{
    public class EMoneyTransfer
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int DepositId { get; set; }
        public ETransaction Deposit { get; set; }
        public int WithdrawId { get; set; }
        public ETransaction Withdraw { get; set; }
    }
}
