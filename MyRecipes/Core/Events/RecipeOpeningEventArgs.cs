using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Events
{
    public class RecipeOpeningEventArgs : EventArgs
    {
        private bool isEdit;
        private string recipeName;
        private Recipe recipe;

        public bool IsEdit => isEdit;

        public string RecipeName => recipeName;

        public Recipe Recipe => recipe;

        public RecipeOpeningEventArgs(string recipeName)
        {
            this.recipeName = recipeName;
        }

        public RecipeOpeningEventArgs(Recipe recipe) : this(recipe.Name)
        {
            this.recipe = recipe;
            isEdit = true;
        }
    }
}
