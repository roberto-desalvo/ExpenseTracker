using AutoMapper;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class TransactionGridViewModel : BaseViewModel
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private ObservableCollection<TransactionGridModel> transactions = new();

        public ObservableCollection<TransactionGridModel> Transactions
        {
            get { return transactions; }
            set {
                transactions = value;
                NotifyPropertyChanged();
            }
        }

        public TransactionGridViewModel(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            EventAggregator.Instance.Subscribe<RefreshMessage>(Refresh);
        }

        public void Refresh(RefreshMessage message = null)
        {
            var transactions = _transactionService.GetTransactions(null, false);
            var models = _mapper.Map<IEnumerable<TransactionGridModel>>(transactions).OrderByDescending(x => x.Date);
            Transactions = new ObservableCollection<TransactionGridModel>(models);
        }
    }
}
