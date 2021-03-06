using MyRecipes.Core;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;
using Tasty.ViewModel.Communication;
using Tasty.ViewModel.JsonNet;

namespace MyRecipes.ViewModel
{
    class RecipeListViewModel : ViewModelBase
    {
        private bool mSearchByRecipe = true;
        private bool mSearchByIngredient;
        private bool mSearchByCategory;

        private bool mShowFiltered;
        private VeryObservableCollection<Recipe> mFilteredRecipes = new VeryObservableCollection<Recipe>("FilteredRecipes");

        private bool mExclusiveSearch;
        private VeryObservableCollection<FilterObject> mSelectedIngredients = new VeryObservableCollection<FilterObject>("SelectedIngredients");
        private FilterObject mSelectedCategory;
        private string mSearchRecipeName;

        private Recipe mSelectedRecipeForDeletion;

        private VeryObservableCollection<FilterObject> mAvailableIngredients = new VeryObservableCollection<FilterObject>("AvailableIngredients");
        private VeryObservableCollection<FilterObject> mAvailableCategories = new VeryObservableCollection<FilterObject>("AvailableCategories");

        private string mNewRecipeName;
        private bool mSortAscending;

        private List<Recipe> mSelectedRecipes;

        public List<Recipe> SelectedRecipes
        {
            get => mSelectedRecipes;
            set
            {
                mSelectedRecipes = value;
                InvokePropertiesChanged();
            }
        }

        public Recipe SelectedRecipeForDeletion
        {
            get => mSelectedRecipeForDeletion;
            set
            {
                mSelectedRecipeForDeletion = value;
                InvokePropertyChanged();
            }
        }

        public List<Recipe> AvailableRecipes
        {
            get
            {
                var data = ShowFiltered ? mFilteredRecipes : App.AvailableRecipes;

                if (mSortAscending)
                {
                    return data.OrderBy(x => x.Priority >= 0 ? x.Priority : 0).ThenBy(x => x.Name).ToList();
                }
                else
                {
                    return data.OrderByDescending(x => x.Priority >= 0 ? x.Priority : 0).ThenByDescending(x => x.Name).ToList();
                }
            }
        }

        public VeryObservableCollection<FilterObject> AvailableIngredients => mAvailableIngredients;

        public VeryObservableCollection<FilterObject> AvailableCategories => mAvailableCategories;

        public VeryObservableCollection<FilterObject> SelectedIngredients
        {
            get => mSelectedIngredients;
            set
            {
                mSelectedIngredients = value;
                InvokePropertyChanged();
            }
        }

        public bool SortAscending
        {
            get => mSortAscending;
            set
            {
                mSortAscending = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AvailableRecipes");
            }
        }

        public bool ShowFiltered
        {
            get => mShowFiltered;
            set
            {
                mShowFiltered = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AvailableRecipes");
                if (!value)
                {
                    mFilteredRecipes.Clear();
                }
            }
        }

        public string SearchRecipeName
        {
            get => mSearchRecipeName;
            set
            {
                mSearchRecipeName = value;
                InvokePropertyChanged();
            }
        }

        public FilterObject SelectedCategory
        {
            get => mSelectedCategory;
            set
            {
                mSelectedCategory = value;
                InvokePropertyChanged();
            }
        }

        public bool SearchByRecipe
        {
            get => mSearchByRecipe;
            set
            {
                mSearchByRecipe = value;
                if (value)
                {
                    ShowFiltered = false;
                }
                else
                {
                    SearchRecipeName = "";
                }
                InvokePropertyChanged();
            }
        }

        public bool SearchByIngredient
        {
            get => mSearchByIngredient;
            set
            {
                mSearchByIngredient = value;
                if (value)
                {
                    ShowFiltered = false;
                }
                else
                {
                    mSelectedIngredients.Clear();
                }
                InvokePropertyChanged();
            }
        }

        public bool SearchByCategory
        {
            get => mSearchByCategory;
            set
            {
                mSearchByCategory = value;
                if (value)
                {
                    ShowFiltered = false;
                }
                else
                {
                    SelectedCategory = null;
                }
                InvokePropertyChanged();
            }
        }

        public bool ExclusiveSearch
        {
            get => mExclusiveSearch;
            set
            {
                mExclusiveSearch = value;
                InvokePropertyChanged();
            }
        }

        public string NewRecipeName
        {
            get => mNewRecipeName;
            set
            {
                mNewRecipeName = value;
                InvokePropertyChanged();
            }
        }

        public RecipeListViewModel()
        {
            UpdateIngredientsAndCategories();
            App.AvailableRecipes.ObserveChanges += AvailableRecipes_ObserveChanges;

            Mediator.Instance.Register(o =>
            {
                RefreshRecipeBinding();
            }, ViewModelMessage.RecipesImported);
        }

        public void SetFilteredRecipes(List<Recipe> filteredRecipes)
        {
            mFilteredRecipes.Clear();
            mFilteredRecipes.AddRange(filteredRecipes);
            ShowFiltered = true;
        }

        public void RefreshRecipeBinding()
        {
            InvokePropertiesChanged("AvailableRecipes");
        }

        private void AvailableRecipes_ObserveChanges(object sender, EventArgs e)
        {
            UpdateIngredientsAndCategories();
        }

        private void UpdateIngredientsAndCategories()
        {
            List<FilterObject> filteredIngredients = new List<FilterObject>();
            List<FilterObject> filteredCategories = new List<FilterObject>();

            foreach (Recipe recipe in App.AvailableRecipes)
            {
                foreach (RecipeIngredient ingredient in recipe.Ingredients)
                {
                    var existing = filteredIngredients.FirstOrDefault(x => x.Name == ingredient.Ingredient.Name);
                    if (existing != null)
                    {
                        existing.Counted++;
                    }
                    else
                    {
                        filteredIngredients.Add(new FilterObject(ingredient.Ingredient.Name));
                    }
                }

                foreach (Category category in recipe.Categories)
                {
                    var existing = filteredCategories.FirstOrDefault(x => x.Name == category.Name);
                    if (existing != null)
                    {
                        existing.Counted++;
                    }
                    else
                    {
                        filteredCategories.Add(new FilterObject(category));
                    }
                }
            }

            mAvailableCategories.Clear();
            mAvailableCategories.AddRange(filteredCategories);

            mAvailableIngredients.Clear();
            mAvailableIngredients.AddRange(filteredIngredients);
        }
    }
}
