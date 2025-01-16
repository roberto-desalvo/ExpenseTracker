using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.DataAccess.Entities;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class TransactionGridViewModel : ObservableObject
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private ObservableCollection<TransactionGridRowModel> transactions = new();

        public TransactionGridViewModel(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            EventAggregator.Instance.Subscribe<RefreshMessage>(Refresh);
        }

        public void Refresh(RefreshMessage message)
        {
            var filter = message.GetTransactionFilter();
            Refresh(filter);
        }

        public void Refresh(Func<IQueryable<ETransaction>, IQueryable<ETransaction>>? filter)
        {
            var transactions = Task.Run(async () => await _transactionService.GetTransactions(filter)).Result;
            var models = _mapper.Map<IEnumerable<TransactionGridRowModel>>(transactions).OrderByDescending(x => x.Date);
            Transactions = new ObservableCollection<TransactionGridRowModel>(models);
        }
    }
}
