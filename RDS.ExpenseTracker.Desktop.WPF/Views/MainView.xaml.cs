using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Controls;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace RDS.ExpenseTracker.Desktop.WPF.Views
{
    /// <summary>
    /// Interaction logic for SinglePageWithTabs.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly ICustomExcelReader _excelReader;
        public MainView(TransactionGridControl transactionGrid, AccountsControl accounts, ICustomExcelReader excelReader)
        {
            _excelReader = excelReader;
            InitializeComponent();
            RenderPages.Children.Add(transactionGrid);
            AccountSpace.Children.Add(accounts);
            SetupEvents();
        }

        private void SetupEvents()
        {
            ImportButton.Click += ImportButton_Click;
            RefreshButton.Click += RefreshButton_Click;
            ExitButton.Click += ExitButton_Click;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            var transactionGrid = RenderPages.Children.Cast<UIElement>().FirstOrDefault(x => typeof(TransactionGridControl).IsAssignableFrom(x.GetType()));

            if (transactionGrid != null)
            {
                ((TransactionGridControl)transactionGrid).Refresh();
            }

            var accountsControl = AccountSpace.Children.Cast<UIElement>().FirstOrDefault(x => typeof(AccountsControl).IsAssignableFrom(x.GetType()));
            if (accountsControl != null)
            {
                ((AccountsControl)accountsControl).Refresh();
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var path = ConfigurationManager.AppSettings.Get("ImportExcelFilePath")?.ToString() ?? string.Empty;

            if (!Path.Exists(path))
            {
                MessageBox.Show("Il path del file spese non esiste");
            }

            Task.Factory.StartNew(() =>
            {
                MessageBox.Show("Import iniziato in backgroud...");

                var list = _excelReader.GetTransactionsFromExcel(path);

                if (!list.Any())
                {
                    MessageBox.Show("Non sono stati recuperati dati dal file excel");
                    return;
                }

                try
                {
                    _excelReader.SaveData(list);
                    MessageBox.Show("Excel importato correttamente");
                    RefreshData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Errore durante il salvataggio dei dati");
                }
            });
        }
    }
}
