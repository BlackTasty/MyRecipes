using MyRecipes.Core.Observer;
using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class CreateEditRecipeViewModel : RecipeViewViewModel
    {
        private bool mIsEdit;
        private bool mShowOpenFileDialog;

        private double mIngredientAmount = 1;
        private int mSelectedMeasurementTypeIndex;
        private int mSelectedIngredientIndex;

        private string mNewPreparationStepText;
        private bool mIsEditPreparationStep;
        private Category mSelectedCategory;

        public VeryObservableCollection<Ingredient> AvailableIngredients => App.AvailableIngredients;

        public VeryObservableCollection<Category> AvailableCategories => App.AvailableCategories;

        public Category SelectedCategory
        {
            get => mSelectedCategory;
            set
            {
                mSelectedCategory = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AddCategoryEnabled");
            }
        }

        public bool UnsavedChanges => Recipe?.UnsavedChanges ?? false;

        public bool AddCategoryEnabled => SelectedCategory != null;

        public bool IsEditPreparationStep
        {
            get => mIsEditPreparationStep;
            set
            {
                mIsEditPreparationStep = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowOpenFileDialog
        {
            get => mShowOpenFileDialog;
            set
            {
                mShowOpenFileDialog = value;
                InvokePropertyChanged();
            }
        }

        public bool IsEdit
        {
            get => mIsEdit;
            set
            {
                mIsEdit = value;
                InvokePropertyChanged();
            }
        }

        public double IngredientAmount
        {
            get => mIngredientAmount;
            set
            {
                mIngredientAmount = value;
                InvokePropertyChanged();
            }
        }

        public int SelectedMeasurementTypeIndex
        {
            get => mSelectedMeasurementTypeIndex;
            set
            {
                mSelectedMeasurementTypeIndex = value;
                InvokePropertyChanged();
            }
        }

        public int SelectedIngredientIndex
        {
            get => mSelectedIngredientIndex;
            set
            {
                mSelectedIngredientIndex = value;
                InvokePropertyChanged();
            }
        }

        public string NewPreparationStepText
        {
            get => mNewPreparationStepText;
            set
            {
                mNewPreparationStepText = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AddPreparationStepEnabled");
            }
        }

        public bool AddPreparationStepEnabled => !string.IsNullOrWhiteSpace(mNewPreparationStepText);

        protected override void OnRecipeChanged(ChangeObservedEventArgs e)
        {
            base.OnRecipeChanged(e);
            InvokePropertyChanged("UnsavedChanges");
        }
    }
}
