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

namespace MyRecipes.ViewModel
{
    class ImportCookingDataViewModel : ViewModelBase
    {
        public event EventHandler<EventArgs> DataChanged;

        private ExportSet<Ingredient> mIngredients = new ExportSet<Ingredient>(App.AvailableIngredients, true);
        private ExportSet<Category> mCategories = new ExportSet<Category>(App.AvailableCategories, true);
        private bool? mAllIngredientsSelected = false;
        private bool? mAllCategoriesSelected = false;
        private string mFilePath;

        private bool disableIsSelectedEvent;

        public ExportSet<Ingredient> Ingredients
        {
            get => mIngredients;
            set
            {
                if (mIngredients != null)
                {
                    mIngredients.ObjectIsSelectedChanged -= Recipes_ObjectIsSelectedChanged;
                }
                mIngredients = value;
                if (mIngredients != null)
                {
                    mIngredients.ObjectIsSelectedChanged += Recipes_ObjectIsSelectedChanged;
                }
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
                InvokePropertiesChanged("AllIngredientsSelected");
            }
        }

        public ExportSet<Category> Categories
        {
            get => mCategories;
            set
            {
                if (mCategories != null)
                {
                    mCategories.ObjectIsSelectedChanged -= Recipes_ObjectIsSelectedChanged;
                }
                mCategories = value;
                if (mCategories != null)
                {
                    mCategories.ObjectIsSelectedChanged += Recipes_ObjectIsSelectedChanged;
                }
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
                InvokePropertiesChanged("AllCategoriesSelected");
            }
        }

        public int SelectedIngredientsCount => mIngredients.SelectedObjectsCount;

        public int SelectedCategoriesCount => mCategories.SelectedObjectsCount;

        public bool IsFormValid => SelectedIngredientsCount > 0 && SelectedCategoriesCount > 0 &&
            !string.IsNullOrWhiteSpace(mFilePath) &&
            File.Exists(mFilePath);

        public string FilePath
        {
            get => mFilePath;
            set
            {
                mFilePath = value;
                InvokePropertiesChanged();

                if (File.Exists(value))
                {
                    string json = File.ReadAllText(value);

                    CookingData imported = JsonConvert.DeserializeObject<CookingData>(json);
                    Ingredients = new ExportSet<Ingredient>(imported.AvailableIngredients, true);
                    Categories = new ExportSet<Category>(imported.AvailableCategories, true);
                    //Recipes = new ExportSet<Recipe>(fileRecipes, true);
                    OnDataChanged(EventArgs.Empty);
                }
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

        private void Recipes_ObjectIsSelectedChanged(object sender, EventArgs e)
        {
            if (!disableIsSelectedEvent)
            {
                RefreshAllSelected();
            }
            OnDataChanged(EventArgs.Empty);
        }

        public void RefreshAllSelected()
        {
            mAllCategoriesSelected = mCategories.AllObjectsSelected;
            mAllIngredientsSelected = mIngredients.AllObjectsSelected;
            InvokePropertiesChanged("AllCategoriesSelected");
            InvokePropertiesChanged("AllIngredientsSelected");
            RefreshSelectedCount();
        }

        public void RefreshSelectedCount()
        {
            InvokePropertiesChanged("SelectedCategoriesCount");
            InvokePropertiesChanged("SelectedIngredientsCount");
            InvokePropertiesChanged("IsFormValid");
        }

        protected virtual void OnDataChanged(EventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }
    }
}
