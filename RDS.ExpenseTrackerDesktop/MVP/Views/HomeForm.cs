using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RDS.ExpenseTracker.Business.Helpers;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Desktop.Mappings;
using RDS.ExpenseTracker.Desktop.MVP.Components;
using RDS.ExpenseTracker.Desktop.MVP.Models;
using RDS.ExpenseTracker.Desktop.MVP.Presenters;
using RDS.ExpenseTracker.Desktop.MVP.Presenters.Abstractions;
using RDS.ExpenseTracker.Desktop.MVP.Views;
using RDS.ExpenseTracker.Desktop.MVP.Views.Abstractions;

namespace RDS.ExpenseTrackerDesktop
{
    public partial class HomeForm : Form
    {
        private IHomeFormView _view;
        private IHomeFormPresenter _presenter;

        // TODO put in configuration file
        string path = @"C:\Users\Roberto\OneDrive\Spese.xlsx";
        string connectionString = "Server=MAIN;Database=ExpenseTracker_Main2;User Id=fantadepo;Password=fantadepo;Trusted_Connection=True;TrustServerCertificate=True;";
        public HomeForm()
        {
            InitializeComponent();

            var model = new HomeFormViewModel();
            var mapper = GetMapper();
            var context = GetContext();
            var accountService = new FinancialAccountService(mapper, context);
            _view = new HomeFormView(GetComponents());
            _presenter = new HomeFormPresenter(model, accountService, _view);            
        }

        private HomeFormComponents GetComponents()
        {
            return new HomeFormComponents
            {
                SellaLabel = SellaAvailabilityLabel,
                SatispayLabel = SatispayAvailabilityLabel,
                ContantiLabel = ContantiAvailabilityLabel,
                HypeLabel = HypeAvailabilityLabel
            };
        }

        private void LoadExcelDataBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var mapper = GetMapper();
            var context = GetContext();
            var accountService = new FinancialAccountService(mapper, context);
            var transactionService = new TransactionService(mapper, context, accountService);
            var transferService = new MoneyTransferService(mapper, context, transactionService, accountService);
            var excelReader = new ExcelReader(accountService, transferService, transactionService, context);

            var list = excelReader.GetTransactionsFromExcel(path);

            if (!list.Any())
            {
                MessageBox.Show("Problemi durante la lettura del file excel", "Xlsx Data Reader", MessageBoxButtons.OK);
            }
            excelReader.SaveData(list);
            Cursor = Cursors.Default;
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpenseTrackerDesktopProfile>();
                cfg.AddProfile<ExpenseTrackerBusinessProfile>();
            });
            return config.CreateMapper();
        }

        private ExpenseTrackerContext GetContext()
        {
            var builder = new DbContextOptionsBuilder<ExpenseTrackerContext>();
            builder.UseSqlServer(connectionString);
            return new ExpenseTrackerContext(builder.Options);
        }

    }
}