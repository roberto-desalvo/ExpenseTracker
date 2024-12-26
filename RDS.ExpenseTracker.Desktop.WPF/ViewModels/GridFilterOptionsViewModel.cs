using RDS.ExpenseTracker.Business.Models;
using RDS.ExpenseTracker.Business.Services.Abstractions;
using RDS.ExpenseTracker.Desktop.WPF.Commands;
using RDS.ExpenseTracker.Desktop.WPF.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.ViewModels
{
    public class GridFilterOptionsViewModel : BaseViewModel
    {
        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                if (value != categories)
                {
                    categories = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (value != selectedCategory)
                {
                    selectedCategory = value;
                    NotifyPropertyChanged();
                    EventAggregator.Instance.Publish(new RefreshMessage { SelectedCategory = selectedCategory });
                }
            }
        }

        private DateOnly startDate;
        public DateOnly StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    NotifyPropertyChanged();
                    EventAggregator.Instance.Publish(new RefreshMessage { StartDate = startDate});
                }
            }
        }

        private DateOnly endDate;
        public DateOnly EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    NotifyPropertyChanged();
                    EventAggregator.Instance.Publish(new RefreshMessage { EndDate = endDate });
                }
            }
        }

        private ObservableCollection<FinancialAccount> accounts;
        public ObservableCollection<FinancialAccount> Accounts
        {
            get { return accounts; }
            set
            {
                if (value != accounts)
                {
                    accounts = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private FinancialAccount selectedAccount;
        public FinancialAccount SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                if (value != selectedAccount)
                {
                    selectedAccount = value;
                    NotifyPropertyChanged();
                    EventAggregator.Instance.Publish(new RefreshMessage { SelectedAccount = selectedAccount });
                }
            }
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
