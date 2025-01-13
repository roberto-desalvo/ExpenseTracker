using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser;
using RDS.ExpenseTracker.Business.DataImport;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly CustomExcelImportService? _excelImporter;

        public MainViewModel(IFinancialAccountService accountService, ITransactionService transactionService, ICategoryService categoryService)
        {
            _accountService = accountService;
            _transactionService = transactionService;

            if (GetXlsImporterConfig() is CustomExcelImporterConfiguration config)
            {
                var parser = new CustomExcelTransactionDataParser(config);
                _excelImporter =new CustomExcelImportService(parser, accountService, transactionService, categoryService);
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

            var answer = MessageBox.Show("You are going to override existing data. You want still want to proceed?", "Excel import", MessageBoxButton.YesNo);
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

        private static CustomExcelImporterConfiguration? GetXlsImporterConfig()
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

            var config = new CustomExcelImporterConfiguration(xlsFilePath, sheetsToIgnore);


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
