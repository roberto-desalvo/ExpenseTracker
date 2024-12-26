using CommunityToolkit.Mvvm.ComponentModel;
using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public partial class GridFilterOptionsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Category> categories;

        [ObservableProperty]
        private Category selectedCategory;
        partial void OnSelectedCategoryChanged(Category value)
        {
            EventAggregator.Instance.Publish(new RefreshMessage { SelectedCategory = this.SelectedCategory });
        }

        [ObservableProperty]
        private DateOnly startDate;
        partial void OnStartDateChanged(DateOnly value)
        {
            EventAggregator.Instance.Publish(new RefreshMessage { StartDate = this.StartDate });
        }

        [ObservableProperty]
        private DateOnly endDate;
        partial void OnEndDateChanged(DateOnly value)
        {
            EventAggregator.Instance.Publish(new RefreshMessage { EndDate = this.EndDate });
        }

        [ObservableProperty]
        private ObservableCollection<FinancialAccount> accounts;


        [ObservableProperty]
        private FinancialAccount selectedAccount;
        partial void OnSelectedAccountChanged(FinancialAccount value)
        {
            EventAggregator.Instance.Publish(new RefreshMessage { SelectedAccount = this.SelectedAccount });
        }

        private readonly IFinancialAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public GridFilterOptionsViewModel(IFinancialAccountService accountService, ICategoryService categoryService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            LoadData();
        }

        private void LoadData()
        {
            var categories = Task.Run(_categoryService.GetCategories).Result;
            var accounts = Task.Run(async () => await _accountService.GetFinancialAccounts()).Result;

            Categories = new ObservableCollection<Category>(categories);
            Accounts = new ObservableCollection<FinancialAccount>(accounts);
        }
    }
}
