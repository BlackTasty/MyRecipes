using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.ViewModel
{
    class ImportRecipeViewModel : ViewModelBase
    {
        public event EventHandler<EventArgs> RecipesChanged;

        private ExportSet<Recipe> mRecipes = ExportSet<Recipe>.Empty;
        private bool? mAllSelected = false;
        private string mFilePath;

        private bool disableIsSelectedEvent;

        public ExportSet<Recipe> Recipes
        {
            get => mRecipes;
            set
            {
                if (mRecipes != null)
                {
                    mRecipes.ObjectIsSelectedChanged -= Recipes_ObjectIsSelectedChanged;
                }
                mRecipes = value;

                if (mRecipes != null)
                {
                    mRecipes.ObjectIsSelectedChanged += Recipes_ObjectIsSelectedChanged;
                }

                InvokePropertiesChanged();
                InvokePropertiesChanged("IsFormValid");
                InvokePropertiesChanged("AllSelected");
            }
        }

        public int SelectedRecipesCount => mRecipes?.SelectedObjectsCount ?? 0;

        public bool IsFormValid => SelectedRecipesCount > 0 &&
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
                    string tempPath = Path.Combine(Path.GetTempPath(), "MyRecipes", "import");
                    if (Directory.Exists(tempPath))
                    {
                        Directory.Delete(tempPath, true);
                    }

                    Directory.CreateDirectory(tempPath);

                    ZipFile.ExtractToDirectory(value, tempPath);
                    string json = File.ReadAllText(Path.Combine(tempPath, "data.json"));
                    List<Recipe> fileRecipes = JsonConvert.DeserializeObject<List<Recipe>>(json);

                    foreach (Recipe recipe in fileRecipes)
                    {
                        // Replace import identifier with path to extracted file
                        recipe.IsImporting = true;
                        if (recipe.RecipeImage?.FilePath.StartsWith("::temp::/") ?? false)
                        {
                            string fileName = recipe.RecipeImage.FilePath.Replace("::temp::/", "");
                            recipe.RecipeImage.FilePath = Path.Combine(tempPath, "img", fileName);
                        }
                        recipe.IsImporting = false;
                    }

                    Recipes = new ExportSet<Recipe>(fileRecipes, true);
                    OnRecipesChanged(EventArgs.Empty);
                }
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

        public ImportRecipeViewModel()
        {
        }

        private void Recipes_ObjectIsSelectedChanged(object sender, EventArgs e)
        {
            if (!disableIsSelectedEvent)
            {
                RefreshAllSelected();
            }
            OnRecipesChanged(EventArgs.Empty);
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

        protected virtual void OnRecipesChanged(EventArgs e)
        {
            RecipesChanged?.Invoke(this, e);
        }
    }
}
