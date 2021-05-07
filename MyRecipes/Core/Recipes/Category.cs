using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Recipes
{
    public class Category : BaseData<Category>
    {
        [JsonConstructor]
        public Category(string guid, string name, string description, DateTime lastModifyDate) : base(guid, name, description, lastModifyDate)
        {

        }

        public Category(string name) : base(name)
        {

        }
    }
}
