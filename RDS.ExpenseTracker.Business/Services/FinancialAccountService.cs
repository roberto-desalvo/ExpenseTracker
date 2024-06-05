using AutoMapper;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Data.Entities;
using System.Linq;

namespace RDS.ExpenseTracker.Business.Services
{
    public class FinancialAccountService : IFinancialAccountService
    {
        private readonly IMapper _mapper;
        private readonly ExpenseTrackerContext _context;

        public FinancialAccountService(IMapper mapper, ExpenseTrackerContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context;
        }

        public int AddFinancialAccount(FinancialAccount account)
        {
            var item = _mapper.Map<EFinancialAccount>(account);
            var entry = _context.FinancialAccounts.Add(item);
            _context.SaveChanges();
            return item.Id;
        }

        public void DeleteFinancialAccount(string id)
        {
            var parsedId = int.Parse(id);
            DeleteFinancialAccount(parsedId);
        }

        public void DeleteFinancialAccount(int id)
        {
            var entity = _context.FinancialAccounts.Find(id);
            if (entity != null)
            {
                _context.FinancialAccounts.Remove(entity);
                _context.SaveChanges();
            }
        }

        public decimal GetAvailability(string accountId)
        {
            var parsedId = int.Parse(accountId);
            return GetAvailability(parsedId);
        }

        public decimal GetAvailability(int accountId)
        {
            var accountEntity = _context.FinancialAccounts.FirstOrDefault(x => x.Id == accountId);            
            return accountEntity?.Availability ?? -1;
        }

        public FinancialAccount? GetFinancialAccount(string id)
        {
            var parsedId = int.Parse(id);
            return GetFinancialAccount(parsedId);
        }

        public FinancialAccount? GetFinancialAccount(int id)
        {
            var entity = _context.FinancialAccounts.Find(id);
            return _mapper.Map<FinancialAccount>(entity);
        }

        public IList<FinancialAccount> GetFinancialAccounts(Func<EFinancialAccount, bool>? filter = null)
        {
            return _context.FinancialAccounts.Where(filter ?? (x => true))
                .Select(_mapper.Map<FinancialAccount>)
                .ToList();
        }

        public bool UpdateAvailability(int accountId, decimal amount)
        {
            var accountEntity = _context.FinancialAccounts.FirstOrDefault(x => x.Id == accountId);
            if (accountEntity == null)
            {
                return false;
            }

            accountEntity.Availability += amount;
            accountEntity.Availability = (decimal)Math.Round(accountEntity.Availability, 2);

            _context.SaveChanges();
            return true;
        }

        public bool UpdateAvailability(string accountId, decimal amount)
        {
            var parsedId = int.Parse(accountId);
            return UpdateAvailability(parsedId, amount);
        }

        public void UpdateFinancialAccount(FinancialAccount account)
        {
            _context.FinancialAccounts.Update(_mapper.Map<EFinancialAccount>(account));
            _context.SaveChanges();
        }
    }
}
