using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Views.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICustomExcelReader _excelReader;

        public RelayCommand? RefreshCommand { get; private set; }
        public RelayCommand? ImportExcelCommand { get; private set; }
        public RelayCommand? ExitCommand { get; private set; }

        public MainViewModel(ICustomExcelReader excelReader)
        {
            _excelReader = excelReader;
            SetupCommands();
        }

        private void SetupCommands()
        {
            RefreshCommand = new RelayCommand(x => Refresh());
            ImportExcelCommand = new RelayCommand(x => ImportExcel());
            ExitCommand = new RelayCommand(x => CloseApplication());
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private void Refresh()
        {
            EventAggregator.Instance.Publish(new RefreshMessage());
        }

        private void ImportExcel()
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
                    Refresh();
                }
                catch (Exception)
                {
                    MessageBox.Show("Errore durante il salvataggio dei dati");
                }
            });
        }
    }
}
