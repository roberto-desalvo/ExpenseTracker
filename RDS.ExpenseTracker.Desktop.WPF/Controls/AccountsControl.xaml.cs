using RDS.ExpenseTracker.Business.Services.Abstractions;
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
        private readonly IFinancialAccountService _accountService;

        public AccountsControl(IFinancialAccountService accountService)
        {
            _accountService = accountService;
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            var accounts = _accountService.GetFinancialAccounts();

            SellaTextBlock.Text = accounts.Where(x => x.Name.ToLower().Contains("sella")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            HypeTextBlock.Text = accounts.Where(x => x.Name.ToLower().Contains("hype")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            SatispayTextBlock.Text = accounts.Where(x => x.Name.ToLower().Contains("satispay")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
            ContantiTextBlock.Text = accounts.Where(x => x.Name.ToLower().Contains("contanti")).FirstOrDefault()?.Availability.ToString() ?? string.Empty;
        }
    }
}
