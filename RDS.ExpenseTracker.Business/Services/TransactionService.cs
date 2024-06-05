using AutoMapper;
using RDS.ExpenseTracker.Business.Helpers;
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

        public void AddTransaction(Transaction transaction)
        {
            transaction.Id = 0;
            var entity = _mapper.Map<ETransaction>(transaction);

            if (entity != null)
            {
                _context.Transactions.Add(entity);
                var account = _context.FinancialAccounts.Where(x => x.Id == entity.FinancialAccountId).FirstOrDefault() ?? throw new Exception();
                account.Availability += entity.Amount;
                _context.SaveChanges();
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

        public IList<Transaction> GetTransactions(Func<ETransaction, bool>? filter = null)
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

        public void UpdateCategories()
        {
            var straordinarie = _context.Transactions.Where(x => x.Description.Contains("xx"));
            foreach(var s in straordinarie)
            {
                s.Category = CategoryEnum.Straordinarie.ToString();
            }

            var psicologa = _context.Transactions.Where(x => x.Description.Contains("psicolog"));
            foreach (var s in psicologa)
            {
                s.Category = CategoryEnum.Psicologa.ToString();
            }

            var sigarette = _context.Transactions.Where(x => x.Description.Contains("tabacc"));
            foreach (var s in sigarette)
            {
                s.Category = CategoryEnum.Sigarette.ToString();
            }

            var glovo = _context.Transactions.Where(x => x.Description.Contains("glovo") || x.Description.Contains("uber") || x.Description.Contains("just eat"));
            foreach (var s in glovo)
            {
                s.Category = CategoryEnum.Cibo_a_Domicilio.ToString();
            }

            var spesa = _context.Transactions.Where(x => x.Description.Contains("spesa"));
            foreach (var s in spesa)
            {
                s.Category = CategoryEnum.Spesa.ToString();
            }

            var svago = _context.Transactions.Where(x => x.Description.Contains("svago"));
            foreach (var s in svago)
            {
                s.Category = CategoryEnum.Svago.ToString();
            }

            var spostamenti = _context.Transactions.Where(x => x.Description.Contains("gtt") || x.Description.Contains("dott") || x.Description.Contains("metro") || x.Description.Contains("spostament"));
            foreach (var s in spostamenti)
            {
                s.Category = CategoryEnum.Spostamenti.ToString();
            }

            var abbonamenti = _context.Transactions.Where(x => x.Description.Contains("abbonament"));
            foreach (var s in abbonamenti)
            {
                s.Category = CategoryEnum.Abbonamento.ToString();
            }

            var salute = _context.Transactions.Where(x => x.Description.Contains("salute") || x.Description.Contains("farmacia"));
            foreach (var s in salute)
            {
                s.Category = CategoryEnum.Salute.ToString();
            }

            var fuori = _context.Transactions.Where(x => x.Description.Contains("fuori"));
            foreach (var s in fuori)
            {
                s.Category = CategoryEnum.Fuori.ToString();
            }

            var bere = _context.Transactions.Where(x => x.Description.Contains("bere"));
            foreach (var s in bere)
            {
                s.Category = CategoryEnum.Bere.ToString();
            }

            var pranzo = _context.Transactions.Where(x => x.Description.Contains("pranzo"));
            foreach (var s in pranzo)
            {
                s.Category = CategoryEnum.Pranzo.ToString();
            }

            var cena = _context.Transactions.Where(x => x.Description.Contains("cena"));
            foreach (var s in cena)
            {
                s.Category = CategoryEnum.Cena.ToString();
            }

            var spostamentiDenaro = _context.Transactions.Where(x => x.Description.Contains("hype") || x.Description.Contains("sella") || x.Description.Contains("satispay") || x.Description.Contains("contanti"));
            foreach (var s in spostamentiDenaro)
            {
                s.Category = CategoryEnum.SpostamentiDenaro.ToString();
            }

            var casa = _context.Transactions.Where(x => x.Description.Contains("casa") || x.Description.Contains("bollett"));
            foreach (var s in casa)
            {
                s.Category = CategoryEnum.Spese_di_Casa.ToString();
            }

            var lavoro = _context.Transactions.Where(x => x.Description.Contains("lavoro") || x.Description.Contains("salsedine") || x.Description.Contains("msc") || x.Description.Contains("mare nostrum"));
            foreach (var s in lavoro)
            {
                s.Category = CategoryEnum.Lavoro.ToString();
            }

            _context.SaveChanges();
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
