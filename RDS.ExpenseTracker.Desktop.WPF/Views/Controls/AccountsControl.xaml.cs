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

namespace RDS.ExpenseTracker.Desktop.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for AccountsControl.xaml
    /// </summary>
    public partial class AccountsControl : UserControl
    {
        public AccountsControl(AccountsControlViewModel viewModel)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
            ((AccountsControlViewModel)this.DataContext).Refresh();
        }

    }
}
