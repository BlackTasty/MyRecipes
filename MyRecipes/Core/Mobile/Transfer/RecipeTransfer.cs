using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Mobile.Transfer
{
    class RecipeTransfer
    {
        private string guid;
        private string name;
        private string description;
        //private RecipeImageTransfer recipeImage;
        private int servings;

        private List<RecipeIngredient> ingredients;
        private List<string> preparationSteps;
        private List<Category> categories;

        public string Guid => guid;

        public string Name => name;

        public string Description => description;

        //public RecipeImageTransfer RecipeImage => recipeImage;

        public int Servings => servings;

        public List<RecipeIngredient> Ingredients => ingredients;

        public List<string> PreparationSteps => preparationSteps;

        public List<Category> Categories => categories;

        public string Checksum { get; set; }

        public RecipeTransfer(Recipe recipe)
        {
            guid = recipe.Guid;
            name = recipe.Name;
            description = recipe.Description;
            servings = recipe.Servings;
            ingredients = recipe.Ingredients.ToList();
            preparationSteps = recipe.PreparationSteps.ToList();
            categories = recipe.Categories.ToList();

            /*if (recipe.HasImage)
            {
                recipeImage = new RecipeImageTransfer(recipe.RecipeImage);
            }*/
        }
    }
}
