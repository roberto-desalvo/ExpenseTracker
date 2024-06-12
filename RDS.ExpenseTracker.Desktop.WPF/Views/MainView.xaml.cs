﻿using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Controls;
using System;
using System.IO;
using System.Linq;
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
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
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
            var path = @"C:\Users\Roberto\OneDrive\Spese.xlsx";

            if(!Path.Exists(path))
            {
                MessageBox.Show("Il path del file spese non esiste");
            }

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
            }
            catch (Exception)
            {
                MessageBox.Show("Errore durante il salvataggio dei dati");
            }
        }
    }
}
