using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core
{
    class CookingData : JsonFile<CookingData>
    {
        private VeryObservableCollection<Ingredient> availableIngredients = new VeryObservableCollection<Ingredient>("AvailableIngredients");
        private VeryObservableCollection<Category> availableCategories = new VeryObservableCollection<Category>("AvailableCategories");

        public VeryObservableCollection<Ingredient> AvailableIngredients => availableIngredients;

        public VeryObservableCollection<Category> AvailableCategories => availableCategories;

        [JsonConstructor]
        public CookingData(List<Ingredient> availableIngredients, List<Category> availableCategories)
        {
            this.availableCategories.AddRange(availableCategories);
            this.availableIngredients.AddRange(availableIngredients);
        }

        public CookingData(FileInfo fi) : base(fi)
        {
            Load();
        }

        public CookingData()
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
