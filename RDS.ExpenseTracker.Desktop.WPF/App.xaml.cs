using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RDS.ExpenseTracker.Business.Helpers;
using RDS.ExpenseTracker.Business.Helpers.Abstractions;
using RDS.ExpenseTracker.Business.Mappings;
using RDS.ExpenseTracker.Business.Services;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Data;
using RDS.ExpenseTracker.Desktop.WPF.Mappings;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels;
using RDS.ExpenseTracker.Desktop.WPF.Views;
using RDS.ExpenseTracker.Desktop.WPF.Views.Controls;
using System;
using System.Configuration;
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
            // context
            services.AddDbContext<ExpenseTrackerContext>(options =>
            {
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
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
            services.AddSingleton<TransactionGridUserControl>();
            services.AddSingleton<AccountsUserControl>();
            services.AddSingleton<GridFilterOptionsUserControl>();

            // view models
            services.AddTransient<MainViewModel>();
            services.AddTransient<TransactionGridViewModel>();
            services.AddTransient<AccountsViewModel>();
            services.AddTransient<GridFilterOptionsViewModel>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainView>();
            mainWindow?.Show();
        }
    }
}
