using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel.JsonNet;

namespace MyRecipes.Core
{
    class CookingData : JsonFile<CookingData>
    {
        private JsonObservableCollection<Ingredient> availableIngredients = new JsonObservableCollection<Ingredient>("AvailableIngredients");
        private JsonObservableCollection<Category> availableCategories = new JsonObservableCollection<Category>("AvailableCategories");

        public JsonObservableCollection<Ingredient> AvailableIngredients => availableIngredients;

        public JsonObservableCollection<Category> AvailableCategories => availableCategories;

        [JsonConstructor]
        public CookingData(List<Ingredient> availableIngredients, List<Category> availableCategories, bool observersEnabled) : this(observersEnabled)
        {
            this.availableCategories.AddRange(availableCategories);
            this.availableIngredients.AddRange(availableIngredients);
        }

        public CookingData(FileInfo fi, bool observersEnabled) : base(fi, observersEnabled)
        {
            Load();
        }

        public CookingData(bool observersEnabled) : base(observersEnabled)
        {

        }

        public void Load()
        {
            CookingData cookingData = LoadFile();

            availableCategories.Clear();
            availableCategories.AddRange(cookingData.availableCategories);
            availableIngredients.Clear();
            availableIngredients.AddRange(cookingData.availableIngredients);
        }

        public void Save(string parentPath)
        {
            Directory.CreateDirectory(parentPath);
            fileName = "data.json";

            SaveFile(parentPath, this);
        }
    }
}
