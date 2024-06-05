using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ExpenseTrackerContext _context;
        private readonly IFinancialAccountService _accountService;

        public TransactionService(IMapper mapper, ExpenseTrackerContext context, IFinancialAccountService accountService) 
        {
            _accountService = accountService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context;
        }

        public void AddTransactions(IEnumerable<Transaction> transactions)
        {
            foreach(var transaction in transactions)
            {
                AddTransaction(transaction, false);                
            }
            _context.SaveChanges();
        }

        public void AddTransaction(Transaction transaction, bool saveChanges)
        {
            transaction.Id = 0;
            var entity = _mapper.Map<ETransaction>(transaction);

            if (entity != null)
            {
                _context.Transactions.Add(entity);
                var account = _context.FinancialAccounts.Where(x => x.Id == entity.FinancialAccountId).FirstOrDefault() ?? throw new Exception();
                account.Availability += entity.Amount;

                if (saveChanges)
                {
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteTransaction(string id)
        {
            if (!int.TryParse(id, out int convertedId))
                return;
            DeleteTransaction(convertedId);
        }

        public void DeleteTransaction(int id)
        {
            var entity = _context.Transactions.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {

                _context.Transactions.Remove(entity);
                var account = _context.FinancialAccounts.Where(x => x.Id == entity.FinancialAccountId).FirstOrDefault() ?? throw new Exception();
                account.Availability -= entity.Amount;

                _context.SaveChanges();

            }
        }

        public void DeleteTransaction(Transaction transaction)
        {

            _context.Transactions.Remove(_mapper.Map<ETransaction>(transaction));
            var account = _context.FinancialAccounts.Where(x => x.Id == transaction.FinancialAccountId).FirstOrDefault() ?? throw new Exception();
            account.Availability -= transaction.Amount;

            _context.SaveChanges();

        }

        public Transaction? GetTransaction(string id)
        {
            if (!int.TryParse(id, out int convertedId))
                throw new InvalidCastException(nameof(id));
            return GetTransaction(convertedId);
        }

        public Transaction? GetTransaction(int id)
        {
            var entity = _context.Transactions.FirstOrDefault(x => x.Id == id);
            return _mapper.Map<Transaction?>(entity);
        }

        public IEnumerable<Transaction> GetTransactions(Func<ETransaction, bool>? filter = null)
        {
            var entities = filter == null ? _context.Transactions : _context.Transactions.Where(filter);
            var transactions = _mapper.Map<IEnumerable<Transaction>>(entities).ToList();
            var accounts = _accountService.GetFinancialAccounts();

            foreach(var transaction in transactions)
            {
                transaction.FinancialAccountName = accounts.Where(x => x.Id == transaction.FinancialAccountId).FirstOrDefault()?.Name ?? string.Empty;
            }
            return transactions;
        }        

        public void UpdateTransaction(Transaction transaction)
        {
            var entity = _context.Transactions.FirstOrDefault(x => x.Id == transaction.Id);
            if (entity != null)
            {
                if (entity.Amount != transaction.Amount)
                {
                    var delta = entity.Amount - transaction.Amount;
                    var account = _context.FinancialAccounts.Where(x => x.Id == entity.FinancialAccountId).FirstOrDefault() ?? throw new Exception();
                    account.Availability -= delta;
                    entity.Amount = transaction.Amount;
                }
                entity.Description = transaction.Description;
                _context.SaveChanges();

            }
        }
    }
}
