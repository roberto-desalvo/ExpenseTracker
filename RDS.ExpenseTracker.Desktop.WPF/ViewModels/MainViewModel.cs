using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RDS.ExpenseTracker.Business.TransactionImport.Abstractions;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using RDS.ExpenseTracker.Business.TransactionImport.Parsers.Models;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IXlsTransactionImporter? _xlsImporter;

        public MainViewModel(ITransactionImporterFactory importerFactory, IFinancialAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;

            if (GetXlsImporterConfig() is XlsImporterConfiguration config)
            {
                _xlsImporter = importerFactory.CreateXlsImporter(config);
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
            if (_xlsImporter == null)
            {
                MessageBox.Show("Problems with configuration, cannot import data");
                return;
            }

            var answer = MessageBox.Show("Would you like to overwrite existing data?", "Import excel", MessageBoxButton.YesNoCancel);
            if (answer == MessageBoxResult.Cancel)
            {
                return;
            }

            var overwriteExistingData = answer == MessageBoxResult.Yes;            

            Task.Factory.StartNew(async () =>
            {
                MessageBox.Show("Import started in backgroud...");                

                try
                {
                    await _xlsImporter.ImportTransactions(overwriteExistingData, true);
                    MessageBox.Show("Xls successfully imported");
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while importing transactions: \n{ex}\n{ex.Message}");
                }
            });
        }

        private static XlsImporterConfiguration? GetXlsImporterConfig()
        {
            if (ConfigurationManager.GetSection("ImportSection") is not NameValueCollection values)
            {
                MessageBox.Show($"Transaction import config is not valid, please check it.");
                return null;
            }

            var xlsFilePath = values["XlsFilePath"];
            var sheetsToIgnore = values["XlsSheetsToIgnore"]?.Split(';', StringSplitOptions.None).Select(x => x.Trim()).ToList() ?? new List<string>();

            if (xlsFilePath is null)
            {
                MessageBox.Show($"Config is not valid, please check xls file path.");
                return null;
            }

            if (ConfigurationManager.GetSection("SetupSection") is not NameValueCollection setupValues)
            {
                MessageBox.Show($"Config is not valid, please check setup values.");
                return null;
            }

            var config = new XlsImporterConfiguration(xlsFilePath, sheetsToIgnore);


            foreach (var key in setupValues.AllKeys)
            {
                if(key is null)
                {
                    continue;
                }

                if (!int.TryParse(setupValues[key], out int value))
                {
                    MessageBox.Show($"Config is not valid, please check setup value: {key}");
                    return null;
                };

                config.AccountInitialAmounts[key] = value;
            }

            return config;
        }
    }
}
