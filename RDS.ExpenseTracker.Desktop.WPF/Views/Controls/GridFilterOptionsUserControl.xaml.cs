using RDS.ExpenseTracker.Desktop.WPF.ViewModels;
using System;
using System.Windows.Controls;

namespace RDS.ExpenseTracker.Desktop.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for GridFilterOptionsUserControl.xaml
    /// </summary>
    public partial class GridFilterOptionsUserControl : UserControl
    {
        public GridFilterOptionsUserControl(GridFilterOptionsViewModel viewModel)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
        }
    }
}
