using RDS.ExpenseTracker.Business.Models;
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
        private ObservableCollection<Category> categories = new();
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
                }
            }
        }
    }
}
