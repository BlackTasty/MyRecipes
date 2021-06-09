using MyRecipes.Core;
using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;
using Tasty.ViewModel.JsonNet;

namespace MyRecipes.ViewModel
{
    class ExportCookingDataViewModel : ViewModelBase
    {
        private ExportSet<Ingredient> mIngredients = new ExportSet<Ingredient>(App.AvailableIngredients, true);
        private ExportSet<Category> mCategories = new ExportSet<Category>(App.AvailableCategories, true);
        private bool? mAllIngredientsSelected = false;
        private bool? mAllCategoriesSelected = false;
        private string mExportPath;
        private string mExportName;

        private bool disableIsSelectedEvent;

        public ExportSet<Ingredient> Ingredients
        {
            get => mIngredients;
            set
            {
                mIngredients = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("AllIngredientsSelected");
            }
        }

        public ExportSet<Category> Categories
        {
            get => mCategories;
            set
            {
                mCategories = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("AllCategoriesSelected");
            }
        }

        public int SelectedIngredientsCount => mIngredients.SelectedObjectsCount;

        public int SelectedCategoriesCount => mCategories.SelectedObjectsCount;

        public bool IsFormValid => SelectedIngredientsCount > 0 && SelectedCategoriesCount > 0 &&
            !string.IsNullOrWhiteSpace(mExportPath) &&
            !string.IsNullOrWhiteSpace(mExportName) &&
            Directory.Exists(mExportPath);

        public string ExportPath
        {
            get => mExportPath;
            set
            {
                mExportPath = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
            }
        }

        public string ExportName
        {
            get => mExportName;
            set
            {
                mExportName = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
            }
        }

        public bool? AllIngredientsSelected
        {
            get => mAllIngredientsSelected;
            set
            {
                mAllIngredientsSelected = value;
                if (value is bool isAllSelected)
                {
                    disableIsSelectedEvent = true;
                    foreach (ExportObject<Ingredient> exportIngredient in mIngredients.ExportObjects)
                    {
                        exportIngredient.IsSelected = isAllSelected;
                    }
                    disableIsSelectedEvent = false;
                    InvokePropertiesChanged("Ingredients");
                }
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
            }
        }

        public bool? AllCategoriesSelected
        {
            get => mAllCategoriesSelected;
            set
            {
                mAllCategoriesSelected = value;
                if (value is bool isAllSelected)
                {
                    disableIsSelectedEvent = true;
                    foreach (ExportObject<Category> exportCategory in mCategories.ExportObjects)
                    {
                        exportCategory.IsSelected = isAllSelected;
                    }
                    disableIsSelectedEvent = false;
                    InvokePropertiesChanged("Categories");
                }
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
            }
        }

        public void RunExport()
        {
            List<Ingredient> selectedIngredients = new List<Ingredient>();
            foreach (ExportObject<Ingredient> exportIngredient in mIngredients.ExportObjects
                                                        .Where(x => x.IsSelected))
            {
                if (exportIngredient.Data is Ingredient ingredient)
                {
                    selectedIngredients.Add(ingredient);
                }
            }

            List<Category> selectedCategories =  new List<Category>();
            foreach (ExportObject<Category> exportCategory in mCategories.ExportObjects
                                                        .Where(x => x.IsSelected))
            {
                if (exportCategory.Data is Category category)
                {
                    selectedCategories.Add(category);
                }
            }

            CookingData exportData = new CookingData(selectedIngredients, selectedCategories, false);
            string json = JsonConvert.SerializeObject(exportData);
            string filePath = Path.Combine(mExportPath, mExportName + ".como");

            File.WriteAllText(filePath, json);
        }
    }
}
