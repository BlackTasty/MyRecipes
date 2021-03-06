using MaterialDesignThemes.Wpf;
using MyRecipes.Controls;
using MyRecipes.Core.Recipes;
using MyRecipes.Core.SeasonCalendar;
using MyRecipes.Core.Sidebar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tasty.ViewModel;
using Tasty.ViewModel.Observer;

namespace MyRecipes.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private RecipeList recipeList;
        private IngredientList ingredientList;
        private Home home;

        private int mSelectedSidebarIndex;
        private FrameworkElement mContent;

        private bool mIsCreateEditRecipeOpen;
        private bool mShowBackToIngredientsButton;
        private bool mShowBackToRecipesButton;
        private bool mShowSaisonCalendarButton;
        private bool mShowShoppingList;
        private bool mShowSaveRecipeButton;
        private bool mEnableSaveRecipeButton;
        private bool mShowEditRecipeButton;

        private ShoppingList mShoppingList = new ShoppingList();

        public List<SidebarEntry> Items { get; private set; }

        public FrameworkElement Content
        {
            get => mContent;
            set
            {
                if (mContent is RecipeView recipeView)
                {
                    recipeView.CategoryClicked -= RecipeView_CategoryClicked;
                }
                else if (value is RecipeList recipeList)
                {
                    recipeList.ReloadList();
                }

                ShowEditRecipeButton = value is RecipeView;

                if (value is CreateEditRecipe createEditRecipe)
                {
                    mIsCreateEditRecipeOpen = true;
                    ShowSaveRecipeButton = true;
                    createEditRecipe.Finished += CreateEditRecipe_Finished;
                    createEditRecipe.RecipeChanged += CreateEditRecipe_RecipeChanged;
                }
                else if (mIsCreateEditRecipeOpen && mContent is CreateEditRecipe createEditClosed)
                {
                    mIsCreateEditRecipeOpen = false;
                    createEditClosed.Finished -= CreateEditRecipe_Finished;
                    createEditClosed.RecipeChanged -= CreateEditRecipe_RecipeChanged;
                }
                mContent = value;
                InvokePropertyChanged();
            }
        }

        private void CreateEditRecipe_RecipeChanged(object sender, ChangeObservedEventArgs e)
        {
            EnableSaveRecipeButton = e.UnsavedChanges;
        }

        public bool ShowBackToIngredientsButton
        {
            get => mShowBackToIngredientsButton;
            set
            {
                mShowBackToIngredientsButton = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowBackToRecipesButton
        {
            get => mShowBackToRecipesButton;
            set
            {
                mShowBackToRecipesButton = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowSaisonCalendarButton
        {
            get => mShowSaisonCalendarButton;
            set
            {
                mShowSaisonCalendarButton = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowShoppingList
        {
            get => mShowShoppingList;
            set
            {
                mShowShoppingList = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowSaveRecipeButton
        {
            get => mShowSaveRecipeButton;
            set
            {
                mShowSaveRecipeButton = value;
                InvokePropertyChanged();
            }
        }

        public bool EnableSaveRecipeButton
        {
            get => mEnableSaveRecipeButton;
            set
            {
                mEnableSaveRecipeButton = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowEditRecipeButton
        {
            get => mShowEditRecipeButton;
            set
            {
                mShowEditRecipeButton = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowShoppingListButton => !mShoppingList?.IsEmpty ?? false;

        public ShoppingList ShoppingList
        {
            get => mShoppingList;
            set
            {
                mShoppingList = value;
                InvokePropertyChanged();
                InvokePropertyChanged("ShowShoppingListButton");
            }
        }

        private void CreateEditRecipe_Finished(object sender, EventArgs e)
        {
            SelectedSidebarIndex = 1;
        }

        public int SelectedSidebarIndex
        {
            get => mSelectedSidebarIndex;
            set
            {
                if (value >= 0)
                {
                    if (Items[value].HasChildren)
                    {
                        if (mSelectedSidebarIndex >= 0)
                        {
                            Items[mSelectedSidebarIndex].ToggleSubEntries(false);
                        }
                        Items[value].ToggleSubEntries(true);
                    }
                    else if (mSelectedSidebarIndex >= 0 && Items[mSelectedSidebarIndex] is SidebarSubEntry subEntry)
                    {
                        subEntry.ParentEntry.ToggleSubEntries(false);
                        Items[value].ToggleSubEntries(true);
                    }

                    Content = Items[value].Content as FrameworkElement;
                }
                else if (mSelectedSidebarIndex > -1)
                {
                    if (Items[mSelectedSidebarIndex] is SidebarSubEntry subEntry)
                    {
                        subEntry.ParentEntry.ToggleSubEntries(false);
                    }
                    else
                    {
                        Items[mSelectedSidebarIndex].ToggleSubEntries(false);
                    }
                }

                if (value == 2)
                {
                    ShowBackToIngredientsButton = false;
                    ShowSaisonCalendarButton = true;
                    ShowBackToRecipesButton = false;
                    ShowSaveRecipeButton = false;
                }
                else if (value == 3)
                {
                    ShowBackToIngredientsButton = true;
                    ShowSaisonCalendarButton = false;
                    ShowBackToRecipesButton = false;
                    ShowSaveRecipeButton = false;
                }
                else
                {
                    ShowBackToIngredientsButton = false;
                    ShowSaisonCalendarButton = false;
                    ShowBackToRecipesButton = false;
                    EnableSaveRecipeButton = false;
                    ShowSaveRecipeButton = mIsCreateEditRecipeOpen;
                }

                mSelectedSidebarIndex = value;

                InvokePropertyChanged("Items");
                InvokePropertyChanged();
            }
        }

        public MainViewModel()
        {
            recipeList = new RecipeList();
            recipeList.RecipeEditing += RecipeList_RecipeEditing;
            recipeList.RecipeOpening += RecipeList_RecipeOpening;
            recipeList.AddingToShoppingList += RecipeList_AddingToShoppingList;

            ingredientList = new IngredientList();
            ingredientList.SeasonConfiguratorOpening += IngredientList_SeasonConfiguratorOpening;

            home = new Home();
            home.RecipeOpening += RecipeList_RecipeOpening;

            SidebarEntry ingredients = new SidebarEntry("Zutaten", PackIconKind.Barley, ingredientList, 2);
            List<SidebarSubEntry> ingredientsSubEntries = new List<SidebarSubEntry>()
            {
                new SidebarSubEntry("Saisonkalendar", PackIconKind.Timetable, new SaisonCalendar(), 20, ingredients, true),
            };

            ingredients.SetSubEntries(ingredientsSubEntries);

            Items = new List<SidebarEntry>()
            {
                new SidebarEntry("Startseite", PackIconKind.Home, home, 0),
                new SidebarEntry("Rezepte", PackIconKind.ChefHat, recipeList, 1),
                ingredients,
                ingredientsSubEntries[0],
                new SidebarEntry("Kategorien", PackIconKind.Group, new CategoryList(), 3),
                new SidebarEntry("Remote Verbindung", PackIconKind.AccessPointNetwork, new RemoteConfiguration(), 4)
            };

            Content = Items[0].Content as FrameworkElement;
        }

        public void RefreshShoppingListBinding()
        {
            InvokePropertyChanged("ShoppingList");
            InvokePropertyChanged("ShowShoppingListButton");
        }

        public void SaveRecipe()
        {
            if (Content is CreateEditRecipe createEditRecipe)
            {
                createEditRecipe.SaveRecipe();
            }
        }

        private void RecipeList_AddingToShoppingList(object sender, Core.Events.RecipeOpeningEventArgs e)
        {
            mShoppingList.AddRecipe(e.Recipe);
            RefreshShoppingListBinding();
        }

        private void IngredientList_SeasonConfiguratorOpening(object sender, SeasonConfiguratorOpeningEventArgs e)
        {
            Content = new SeasonIngredientConfigurator(e.Ingredient);
            SelectedSidebarIndex = -1;
            ShowBackToIngredientsButton = true;
            ShowBackToRecipesButton = false;
            ShowSaisonCalendarButton = false;
        }

        private void RecipeList_RecipeOpening(object sender, Core.Events.RecipeOpeningEventArgs e)
        {
            OpenRecipe(e.Recipe, false);
        }

        public void OpenRecipe(Recipe recipe, bool isEdit)
        {
            if (isEdit)
            {
                Content = new CreateEditRecipe(recipe);
            }
            else
            {
                RecipeView recipeView = new RecipeView(recipe);
                recipeView.CategoryClicked += RecipeView_CategoryClicked;
                Content = recipeView;
            }

            SelectedSidebarIndex = -1;
            ShowBackToIngredientsButton = false;
            ShowBackToRecipesButton = true;
            ShowSaisonCalendarButton = false;
        }

        private void RecipeView_CategoryClicked(object sender, Core.Events.CategoryClickedEventArgs e)
        {
            recipeList.SetCategoryFilter(e.Category);
            SelectedSidebarIndex = 1;
        }

        private void RecipeList_RecipeEditing(object sender, Core.Events.RecipeOpeningEventArgs e)
        {
            if (e.IsEdit)
            {
                OpenRecipe(e.Recipe, true);
            }
            else
            {
                Content = new CreateEditRecipe(e.RecipeName);
                SelectedSidebarIndex = -1;
                ShowBackToIngredientsButton = false;
                ShowBackToRecipesButton = true;
                ShowSaisonCalendarButton = false;
            }
        }
    }
}
