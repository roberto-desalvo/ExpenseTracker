using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Domain.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess;
using RDS.ExpenseTracker.DataAccess.Entities;

namespace RDS.ExpenseTracker.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ExpenseTrackerContext _context;
        private readonly IFinancialAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public TransactionService(IMapper mapper, ExpenseTrackerContext context, IFinancialAccountService accountService, ICategoryService categoryService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddTransactions(IEnumerable<Transaction> transactions)
        {
            var entites = _mapper.Map<IEnumerable<ETransaction>>(transactions);
            await _context.Transactions.AddRangeAsync(entites);
            await _context.SaveChangesAsync();            
        }

        public async Task<int> AddTransaction(Transaction transaction)
        {
            return await AddTransaction(transaction, true);
        }

        public async Task<int> AddTransaction(Transaction transaction, bool saveChanges)
        {
            var entity = _mapper.Map<ETransaction>(transaction);

            await _context.Transactions.AddAsync(entity);

            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public async Task DeleteTransaction(int id)
        {
            var entity = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Transactions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Transaction?> GetTransaction(int id)
        {
            var entity = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<Transaction?>(entity);
        }

        public async Task UpdateTransaction(Transaction modified)
        {
            var current = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == modified.Id);
            if (current != null)
            {
                if (current.Amount != modified.Amount)
                {
                    current.Amount = modified.Amount;

                    var delta = modified.Amount - current.Amount;

                    await _accountService.UpdateAvailability(current.FinancialAccountId, delta, false);
                }
                current.Description = modified.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            return await GetTransactions(null);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(Func<IQueryable<ETransaction>, IQueryable<ETransaction>> filter)
        {
            var query = _context.Transactions.AsQueryable();

            if (filter != null)
            {
                query = filter.Invoke(query);
            }

            query = query.Include(x => x.FinancialAccount);
            query = query.Include(x => x.Category);

            var results = await query.ToListAsync();

            return _mapper.Map<IEnumerable<Transaction>>(results);
        }

        public async Task DeleteAllTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            _context.Transactions.RemoveRange(transactions);
            await _context.SaveChangesAsync();
        }
    }
}
