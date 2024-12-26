using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICustomExcelReader _excelReader;
        private readonly IFinancialAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public MainViewModel(ICustomExcelReader excelReader, IFinancialAccountService accountService, ITransactionService transactionService)
        {
            _excelReader = excelReader;
            _accountService = accountService;
            _transactionService = transactionService;
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
            var path = ConfigurationManager.AppSettings.Get("ImportExcelFilePath")?.ToString() ?? string.Empty;

            if (!Path.Exists(path))
            {
                MessageBox.Show("Il path del file spese non esiste");
            }

            var answer = MessageBox.Show("Vuoi sovrascrivere i dati esistenti?", "Import excel", MessageBoxButton.YesNoCancel);
            if (answer == MessageBoxResult.Cancel)
            {
                return;
            }
            else if (answer == MessageBoxResult.Yes)
            {
                try
                {
                    _transactionService.DeleteAllTransactions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Problemi durante la cancellazione delle transazioni: \n{ex.Message}");
                    return;
                }

                if (ConfigurationManager.GetSection("InitValuesSection") is not NameValueCollection initValues)
                {
                    MessageBox.Show($"Configurazione non valida. Non ho trovato i valori iniziali");
                    return;
                }

                var accounts = Task.Run(async () => await _accountService.GetFinancialAccounts()).Result;

                foreach (var key in initValues.AllKeys)
                {
                    if (!int.TryParse(initValues[key], out int value))
                    {
                        MessageBox.Show($"Configurazione invalida: {key}");
                        return;
                    };

                    if (accounts.FirstOrDefault(a => string.Equals(a.Name, key, StringComparison.InvariantCultureIgnoreCase))
                        is not FinancialAccount account)
                    {
                        MessageBox.Show($"Account {key} non trovato");
                        return;
                    }

                    account.Availability = value;
                    _accountService.UpdateFinancialAccount(account);
                }
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
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante il salvataggio dei dati: \n{ex.Message}");
                }
            });
        }
    }
}
