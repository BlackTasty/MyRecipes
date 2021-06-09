using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.ViewModel
{
    class ExportRecipeViewModel : ViewModelBase
    {
        private ExportSet<Recipe> mRecipes = new ExportSet<Recipe>(App.AvailableRecipes, false);
        private bool? mAllSelected = false;
        private string mExportPath;
        private string mExportName;

        private bool disableIsSelectedEvent;

        public ExportSet<Recipe> Recipes
        {
            get => mRecipes;
            set
            {
                mRecipes = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
                InvokePropertiesChanged("AllSelected");
            }
        }

        public int SelectedRecipesCount => mRecipes.SelectedObjectsCount;

        public bool IsFormValid => SelectedRecipesCount > 0 &&
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

        public bool? AllSelected
        {
            get => mAllSelected;
            set
            {
                mAllSelected = value;
                if (value is bool isAllSelected)
                {
                    disableIsSelectedEvent = true;
                    foreach (ExportObject<Recipe> exportRecipe in mRecipes.ExportObjects)
                    {
                        exportRecipe.IsSelected = isAllSelected;
                    }
                    disableIsSelectedEvent = false;
                    InvokePropertiesChanged("Recipes");
                }
                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
            }
        }

        public ExportRecipeViewModel()
        {
            mRecipes.ObjectIsSelectedChanged += Recipes_ObjectIsSelectedChanged;
        }

        private void Recipes_ObjectIsSelectedChanged(object sender, EventArgs e)
        {
            if (!disableIsSelectedEvent)
            {
                RefreshAllSelected();
            }
        }


        public void RefreshAllSelected()
        {
            mAllSelected = mRecipes.AllObjectsSelected;
            InvokePropertiesChanged("AllSelected");
            RefreshSelectedCount();
        }

        public void RefreshSelectedCount()
        {
            InvokePropertiesChanged("SelectedRecipesCount");
            InvokePropertiesChanged("IsFormValid");
        }

        public void RunExport()
        {
            mRecipes.ExportSetTo(mExportPath, mExportName, "recps");
        }
    }
}
