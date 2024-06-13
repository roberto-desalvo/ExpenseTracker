using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RDS.ExpenseTracker.Desktop.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AccountsControl.xaml
    /// </summary>
    public partial class AccountsControl : UserControl
    {
        private readonly AccountsControlViewModel _viewModel;

        public AccountsControl(IFinancialAccountService accountService)
        {
            _viewModel = new(accountService);
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            _viewModel.Refresh();
        }
    }
}
