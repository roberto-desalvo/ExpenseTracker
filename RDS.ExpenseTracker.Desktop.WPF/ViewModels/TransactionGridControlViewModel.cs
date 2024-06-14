using AutoMapper;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class TransactionGridControlViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private ObservableCollection<TransactionDataGridModel> _transactions = new();

        public ObservableCollection<TransactionDataGridModel> Transactions
        {
            get { return _transactions; }
            set {
                _transactions = value;
                NotifyPropertyChanged();
            }
        }

        public TransactionGridControlViewModel(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            EventAggregator.Instance.Subscribe<RefreshMessage>(Refresh);
        }

        public void Refresh(RefreshMessage message = null)
        {
            var transactions = _transactionService.GetTransactions(null, false);
            var models = _mapper.Map<IEnumerable<TransactionDataGridModel>>(transactions).OrderByDescending(x => x.Date);
            _transactions = new ObservableCollection<TransactionDataGridModel>(models);
        }
    }
}
