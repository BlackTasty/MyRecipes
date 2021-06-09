using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel.JsonNet;
using Tasty.ViewModel.Observer;

namespace MyRecipes.Core.Recipes
{
    public class Recipe : BaseData<Recipe>
    {
        private string nameCurrent;

        private JsonObservableCollection<RecipeIngredient> mIngredients = new JsonObservableCollection<RecipeIngredient>("Ingredients", true);
        private JsonObservableCollection<string> mPreparationSteps = new JsonObservableCollection<string>("PreparationSteps", true);
        private JsonObservableCollection<Category> mCategories = new JsonObservableCollection<Category>("Categories", true);
        //private VeryObservableCollection<RecipeImage> mRecipeImages = new VeryObservableCollection<RecipeImage>("RecipeImages");

        private RecipeImage mRecipeImage;
        private int mServings;
        private int mPriority = -1; //0 = top priority; -1 = ignore priority
        private int mTime;

        private bool ignoreHasImageFlag;

        public int Priority
        {
            get => mPriority;
            set
            {
                observerManager.ObserveProperty(value);
                mPriority = value;
                InvokePropertyChanged();
            }
        }

        public int Time
        {
            get => mTime;
            set
            {
                observerManager.ObserveProperty(value);
                mTime = Math.Max(1, value);
                InvokePropertiesChanged();
            }
        }

        public JsonObservableCollection<RecipeIngredient> Ingredients
        {
            get => mIngredients;
            set
            {
                observerManager.ObserveProperty(value);
                mIngredients = value;
                InvokePropertyChanged();
            }
        }

        public JsonObservableCollection<string> PreparationSteps
        {
            get => mPreparationSteps;
            set
            {
                observerManager.ObserveProperty(value);
                mPreparationSteps = value;
                InvokePropertyChanged();
            }
        }

        public JsonObservableCollection<Category> Categories
        {
            get => mCategories;
            set
            {
                observerManager.ObserveProperty(value);
                mCategories = value;
                InvokePropertyChanged();
            }
        }

        [JsonIgnore]
        public bool HasImage => mRecipeImage?.IsImageSet ?? false;

        [JsonIgnore]
        public bool IsImporting { get; set; }

        public RecipeImage RecipeImage
        {
            get => IsImporting || ignoreHasImageFlag || HasImage ? mRecipeImage : null; //new RecipeImage(Utils.BitmapToBitmapImage(Properties.Resources.no_image))
            set
            {
                observerManager.ObserveProperty(value);
                if (mRecipeImage != null)
                {
                    ObserverManager.UnregisterChild(mRecipeImage.ObserverManager);
                }
                mRecipeImage = value;
                if (value != null)
                {
                    ObserverManager.RegisterChild(value.ObserverManager);
                }
                InvokePropertyChanged();
                InvokePropertyChanged("HasImage");
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
                observerManager.ObserveProperty(value);
                mRecipeImages = value;
                InvokePropertyChanged();
            }
        }*/

        public int Servings
        {
            get => mServings;
            set
            {
                observerManager.ObserveProperty(value);
                mServings = Math.Max(1, value);
                InvokePropertyChanged();
            }
        }

        [JsonConstructor]
        public Recipe(string guid, string name, string description, DateTime lastModifyDate, List<RecipeIngredient> ingredients, List<string> preparationSteps,
            List<Category> categories, RecipeImage recipeImage, int servings, int priority, int time) : base(guid, name, description, lastModifyDate)
        {
            Ingredients.AddRange(ingredients);
            PreparationSteps.AddRange(preparationSteps);
            categories = categories.OrderBy(x => x.Name).ToList();
            Categories.AddRange(categories);
            RecipeImage = recipeImage;
            Servings = servings;
            Priority = priority;
            Time = time;
            nameCurrent = Name;
        }

        public Recipe(FileInfo fi) : base(fi)
        {
            Load();
        }

        public Recipe(string name) : base(name)
        {
            LastAccessDate = new DateTime(0);
            Servings = 1;
            Time = 1;
            Priority = -1;
            nameCurrent = Name;
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
            Time = recipe.Time;
            Priority = recipe.Priority;
            nameCurrent = Name;
            observerManager.ResetObservers();
        }

        public void Save(string parentPath)
        {
            Directory.CreateDirectory(parentPath);

            if (nameCurrent != null && filePath != null && 
                File.Exists(Path.Combine(filePath, nameCurrent + ".json")))
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

        public void RegisterChildObservers()
        {
            ObserverManager.RegisterChild(Ingredients.ObserverManager);
            ObserverManager.RegisterChild(PreparationSteps.ObserverManager);
            ObserverManager.RegisterChild(Categories.ObserverManager);

            ObserverManager.ResetObservers();
        }

        public void UnregisterChildObservers()
        {
            ObserverManager.UnregisterChild(Ingredients.ObserverManager);
            ObserverManager.UnregisterChild(PreparationSteps.ObserverManager);
            ObserverManager.UnregisterChild(Categories.ObserverManager);
        }
    }
}
