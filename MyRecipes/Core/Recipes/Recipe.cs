using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Recipes
{
    public class Recipe : BaseData<Recipe>
    {
        private string nameCurrent;

        private VeryObservableCollection<RecipeIngredient> mIngredients = new VeryObservableCollection<RecipeIngredient>("Ingredients");
        private VeryObservableCollection<string> mPreparationSteps = new VeryObservableCollection<string>("PreparationSteps");
        private VeryObservableCollection<Category> mCategories = new VeryObservableCollection<Category>("Categories");
        //private VeryObservableCollection<RecipeImage> mRecipeImages = new VeryObservableCollection<RecipeImage>("RecipeImages");

        private RecipeImage mRecipeImage;
        private int mServings;
        private int mPriority = -1; //0 = top priority; -1 = ignore priority

        private bool ignoreHasImageFlag;

        public int Priority
        {
            get => mPriority;
            set
            {
                changeManager.ObserveProperty(value);
                mPriority = value;
                InvokePropertyChanged();
            }
        }

        public VeryObservableCollection<RecipeIngredient> Ingredients
        {
            get => mIngredients;
            set
            {
                changeManager.ObserveProperty(value);
                mIngredients = value;
                InvokePropertyChanged();
            }
        }

        public VeryObservableCollection<string> PreparationSteps
        {
            get => mPreparationSteps;
            set
            {
                changeManager.ObserveProperty(value);
                mPreparationSteps = value;
                InvokePropertyChanged();
            }
        }

        public VeryObservableCollection<Category> Categories
        {
            get => mCategories;
            set
            {
                changeManager.ObserveProperty(value);
                mCategories = value;
                InvokePropertyChanged();
            }
        }

        [JsonIgnore]
        public bool HasImage => mRecipeImage != null;

        public RecipeImage RecipeImage
        {
            get => ignoreHasImageFlag || HasImage ? mRecipeImage : new RecipeImage(Utils.BitmapToBitmapImage(Properties.Resources.no_image));
            set
            {
                changeManager.ObserveProperty(value);
                if (mRecipeImage != null)
                {
                    mRecipeImage.ChangeManager.UnregisterParent(mRecipeImage.ChangeManager);
                }
                mRecipeImage = value;
                if (value != null)
                {
                    mRecipeImage.ChangeManager.RegisterParent(value.ChangeManager);
                }
                InvokePropertyChanged();
            }
        }

        /*public VeryObservableCollection<RecipeImage> RecipeImages
        {
            get
            {
                return mRecipeImages;

                //if (mRecipeImages.Count > 0)
                //{
                //    return mRecipeImages;
                //}
                //else
                //{
                //    var defaultImages = new VeryObservableCollection<RecipeImage>("RecipeImages");
                //    defaultImages.Add(new RecipeImage(Utils.BitmapToBitmapImage(Properties.Resources.no_image)));
                //    return defaultImages;
                //}
            }
            set
            {
                changeManager.ObserveProperty(value);
                mRecipeImages = value;
                InvokePropertyChanged();
            }
        }*/

        public int Servings
        {
            get => mServings;
            set
            {
                changeManager.ObserveProperty(value);
                mServings = Math.Max(1, value);
                InvokePropertyChanged();
            }
        }

        [JsonConstructor]
        public Recipe(string guid, string name, string description, DateTime lastModifyDate, List<RecipeIngredient> ingredients, List<string> preparationSteps,
            List<Category> categories, RecipeImage recipeImage, int servings, int priority) : base(guid, name, description, lastModifyDate)
        {
            Ingredients.AddRange(ingredients);
            PreparationSteps.AddRange(preparationSteps);
            categories = categories.OrderBy(x => x.Name).ToList();
            Categories.AddRange(categories);
            RecipeImage = recipeImage;
            Servings = servings;
            Priority = priority;
            nameCurrent = Name;
            RegisterChildObservers();
        }

        public Recipe(FileInfo fi) : base(fi)
        {
            RegisterChildObservers();
            Load();
        }

        public Recipe(string name) : base(name)
        {
            LastAccessDate = new DateTime(0);
            Servings = 1;
            nameCurrent = Name;
            RegisterChildObservers();
        }

        ~Recipe()
        {
            UnregisterChildObservers();
        }

        public void Load()
        {
            Recipe recipe = LoadFile();

            Ingredients.Clear();
            Ingredients.AddRange(recipe.mIngredients);

            PreparationSteps.Clear();
            PreparationSteps.AddRange(recipe.mPreparationSteps);

            Categories.Clear();
            Categories.AddRange(recipe.mCategories);

            RecipeImage = recipe.RecipeImage;

            Servings = recipe.mServings;
            nameCurrent = Name;
            changeManager.ResetObservers();
        }

        public void Save(string parentPath)
        {
            Directory.CreateDirectory(parentPath);

            if (nameCurrent != null)
            {
                string oldFileName = Path.Combine(filePath, nameCurrent + ".json");
                if (fromFile && Name != nameCurrent && File.Exists(oldFileName)) //Recipe has been renamed, remove old file
                {
                    File.Delete(oldFileName);
                }
            }

            fileName = Name + ".json";
            Categories.Set(Categories.OrderBy(x => x.Name).ToList());

            ignoreHasImageFlag = true;
            SaveFile(parentPath, this);
            ignoreHasImageFlag = false;
            nameCurrent = Name;
        }

        private void RegisterChildObservers()
        {
            Ingredients.RegisterParent(ChangeManager);
            Categories.RegisterParent(ChangeManager);
        }

        private void UnregisterChildObservers()
        {
            Ingredients.UnregisterParent(ChangeManager);
            Categories.UnregisterParent(ChangeManager);
        }
    }
}
