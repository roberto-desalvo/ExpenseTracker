using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RDS.ExpenseTracker.Business.Helpers;
using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Business.Mappings;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Desktop.WPF.Controls;
using RDS.ExpenseTracker.Desktop.WPF.Mappings;
using RDS.ExpenseTracker.Desktop.WPF.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RDS.ExpenseTracker.Desktop.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            // context
            services.AddDbContext<ExpenseTrackerContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // automapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ExpenseTrackerBusinessProfile>();
                cfg.AddProfile<ExpenseTrackerDesktopProfile>();
            });

            // services
            services.AddScoped<IFinancialAccountService, FinancialAccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomExcelReader, CustomExcelReader>();

            // views
            services.AddSingleton<MainView>();
            services.AddSingleton<TransactionGridControl>();
            services.AddSingleton<AccountsControl>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainView>();
            mainWindow?.Show();
        }
    }
}
