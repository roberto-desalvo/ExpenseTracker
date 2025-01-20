using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;
using RDS.ExpenseTracker.Business.DataImport;
using RDS.ExpenseTracker.Desktop.WPF.Helpers;
using RDS.ExpenseTracker.Importer.Parsers.Abstractions;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly CustomExcelImportService? _excelImporter;

        public MainViewModel(IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService, IExcelFileReader excelReader)
        {
            _accountService = accountService;
            _transactionService = transactionService;

            if (ConfigHelper.GetImporterConfig(out var errorMessage) is ExcelImporterConfiguration config)
            {
                var parser = new ExcelTransactionDataParser(excelReader, config);
                _excelImporter = new CustomExcelImportService(parser, accountService, transactionService, categoryService);
            }
            else
            {
                MessageBox.Show(errorMessage);
            }
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        public static void Refresh()
        {
            EventAggregator.Instance.Publish(new RefreshMessage());
        }

        [RelayCommand]
        private void ImportExcel()
        {
            if (_excelImporter == null)
            {
                MessageBox.Show("Problems with configuration, cannot import data");
                return;
            }

            var answer = MessageBox.Show("You are going to override existing data. You still want to proceed?", "Excel import", MessageBoxButton.YesNo);
            if (answer != MessageBoxResult.Yes)
            {
                return;
            }

            Task.Factory.StartNew(async () =>
            {
                MessageBox.Show("Import started in backgroud...");

                try
                {
                    await _excelImporter.ImportTransactions();
                    MessageBox.Show("Excel successfully imported");
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while importing transactions: \n{ex}\n{ex.Message}");
                }
            });
        }
    }
}
