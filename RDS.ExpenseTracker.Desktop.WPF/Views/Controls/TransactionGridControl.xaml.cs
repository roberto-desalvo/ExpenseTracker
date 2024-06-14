using AutoMapper;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Models;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace RDS.ExpenseTracker.Desktop.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for TransactionGrid.xaml
    /// </summary>
    public partial class TransactionGridControl : UserControl
    {
        public TransactionGridControl(TransactionGridControlViewModel viewModel)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
            ((TransactionGridControlViewModel)this.DataContext).Refresh();
        }
    }
}
