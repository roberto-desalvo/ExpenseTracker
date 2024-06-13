using AutoMapper;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace RDS.ExpenseTracker.Desktop.WPF.Controls
{
    /// <summary>
    /// Interaction logic for TransactionGrid.xaml
    /// </summary>
    public partial class TransactionGridControl : UserControl
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private ObservableCollection<TransactionDataGridViewModel> _transactions = new ObservableCollection<TransactionDataGridViewModel>();
        public ObservableCollection<TransactionDataGridViewModel> Transactions
        {
            get => _transactions;
            set => _transactions = value;
        }

        public TransactionGridControl(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            var transactions = _transactionService.GetTransactions(null, false);
            var models = _mapper.Map<IEnumerable<TransactionDataGridViewModel>>(transactions).OrderByDescending(x => x.Date);
            _transactions = new ObservableCollection<TransactionDataGridViewModel>(models);
            TransactionGrid.ItemsSource = Transactions;
        }
    }
}
