using Microsoft.Extensions.DependencyInjection;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels;
using RDS.ExpenseTracker.Desktop.WPF.Views.Controls;
using System;
using System.Windows;

namespace RDS.ExpenseTracker.Desktop.WPF.Views
{
    /// <summary>
    /// Interaction logic for SinglePageWithTabs.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel viewModel, IServiceProvider serviceProvider)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
            GridPlaceholder.Children.Add(serviceProvider.GetRequiredService<TransactionGridUserControl>());
            AccountHeader.Children.Add(serviceProvider.GetRequiredService<AccountsUserControl>());
            GridFilterPlaceholder.Children.Add(serviceProvider.GetService<GridFilterOptionsUserControl>());
            MainViewModel.Refresh();
        }
    }
}
