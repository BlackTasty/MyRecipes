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
        private VeryObservableCollection<RecipeIngredient> mIngredients = new VeryObservableCollection<RecipeIngredient>("Ingredients");
        private VeryObservableCollection<string> mPreparationSteps = new VeryObservableCollection<string>("PreparationSteps");
        private VeryObservableCollection<Category> mCategories = new VeryObservableCollection<Category>("Categories");
        //private VeryObservableCollection<RecipeImage> mRecipeImages = new VeryObservableCollection<RecipeImage>("RecipeImages");

        private RecipeImage mRecipeImage;
        private int mServings;

        private bool ignoreHasImageFlag;

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
                mRecipeImage = value;
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
            List<Category> categories, RecipeImage recipeImage, int servings) : base(guid, name, description, lastModifyDate)
        {
            mIngredients.AddRange(ingredients);
            mPreparationSteps.AddRange(preparationSteps);
            mCategories.AddRange(categories);
            mRecipeImage = recipeImage;
            Servings = servings;
        }

        public Recipe(FileInfo fi) : base(fi)
        {
            Load();
        }

        public Recipe(string name) : base(name)
        {
            LastAccessDate = new DateTime(0);
            mServings = 1;
        }

        public void Load()
        {
            Recipe recipe = LoadFile();

            mIngredients.Clear();
            mIngredients.AddRange(recipe.mIngredients);

            mPreparationSteps.Clear();
            mPreparationSteps.AddRange(recipe.mPreparationSteps);

            mCategories.Clear();
            mCategories.AddRange(recipe.mCategories);

            mRecipeImage = recipe.RecipeImage;

            Servings = recipe.mServings;
        }

        public void Save(string parentPath)
        {
            Directory.CreateDirectory(parentPath);

            if (fromFile && File.Exists(fileName) && fileName != Name + ".json") //Recipe has been renamed, remove old file
            {
                File.Delete(fileName);
            }

            fileName = Name + ".json";

            ignoreHasImageFlag = true;
            SaveFile(parentPath, this);
            ignoreHasImageFlag = false;
        }
    }
}
