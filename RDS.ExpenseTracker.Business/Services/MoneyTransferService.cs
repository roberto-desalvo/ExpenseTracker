using AutoMapper;
using RDS.ExpenseTracker.Business.Helpers;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Data.Entities;

namespace RDS.ExpenseTracker.Business.Services
{
    public class MoneyTransferService : MoneyTransactor, IMoneyTransferService
    {
        private readonly IMapper _mapper;
        private readonly ExpenseTrackerContext _context;
        private readonly ITransactionService _transactionService;
        private readonly IFinancialAccountService _accountService;

        public MoneyTransferService(IMapper mapper, ExpenseTrackerContext context, ITransactionService transactionService, IFinancialAccountService accountService) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _context = context;
        }

        public void AddMoneyTransfer(MoneyTransfer moneyTransfer)
        {
            var depositAccount = _accountService.GetFinancialAccount(moneyTransfer.DepositId);
            var withdrawAccount = _accountService.GetFinancialAccount(moneyTransfer.WithdrawId);

            var depositDescription = $"Received {moneyTransfer.Amount} from {withdrawAccount.Name}";
            var withdrawDescription = $"Moved {moneyTransfer.Amount} to {depositAccount.Name}";

            var deposit = new Transaction()
            {
                Id = 0,
                Date = moneyTransfer.Date,
                Amount = moneyTransfer.Amount,
                FinancialAccountId = moneyTransfer.DepositId,
                Description = depositDescription
            };

            var withdraw = new Transaction()
            {
                Id = 0,
                Date = moneyTransfer.Date,
                Amount = -moneyTransfer.Amount,
                FinancialAccountId = moneyTransfer.WithdrawId,
                Description = withdrawDescription
            };

            var entity = _mapper.Map<EMoneyTransfer>(moneyTransfer);
            entity.Deposit = _mapper.Map<ETransaction>(deposit);
            entity.Withdraw = _mapper.Map<ETransaction>(withdraw);

            var depositAccountEntity = _context.FinancialAccounts.Where(x => x.Id == depositAccount.Id).FirstOrDefault() ?? throw new Exception();
            var withdrawAccountEntity = _context.FinancialAccounts.Where(x => x.Id == withdrawAccount.Id).FirstOrDefault() ?? throw new Exception();
            withdrawAccountEntity.Availability += withdraw.Amount;
            depositAccountEntity.Availability += deposit.Amount;

            _context.MoneyTransfers.Add(entity);

            _context.SaveChanges();

        }

        public void DeleteMoneyTransfer(int id)
        {

            var entity = _context.MoneyTransfers.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                _transactionService.DeleteTransaction(_mapper.Map<Transaction>(entity.Deposit));
                _transactionService.DeleteTransaction(_mapper.Map<Transaction>(entity.Withdraw));
                _context.MoneyTransfers.Remove(entity);
                _context.SaveChanges();
            }

        }
        public void DeleteMoneyTransfer(string id)
        {
            var parsedId = int.Parse(id);
            DeleteMoneyTransfer(parsedId);
        }

        public MoneyTransfer? GetMoneyTransfer(string id)
        {
            var convertedId = int.Parse(id);
            return GetMoneyTransfer(convertedId);
        }

        public MoneyTransfer? GetMoneyTransfer(int id)
        {
            var entity = _context.MoneyTransfers.FirstOrDefault(x => x.Id == id);
            return _mapper.Map<MoneyTransfer>(entity);
        }

        public IList<MoneyTransfer> GetMoneyTransfers()
        {
            throw new NotImplementedException();
        }

        public void UpdateMoneyTransfer(MoneyTransfer moneyTransfer)
        {

            // TODO

        }
    }
}
