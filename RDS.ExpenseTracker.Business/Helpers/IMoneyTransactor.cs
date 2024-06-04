namespace RDS.ExpenseTracker.Business.Helpers
{
    public interface IMoneyTransactor
    {
        public bool AtomicTransaction(Action action);
    }
}
