using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core
{
    public class AppSettings : JsonFile<AppSettings>
    {
        private string recipeDirectory = App.DefaultRecipePath;
        private string cookingDataPath = App.DefaultCookingDataPath;

        public string RecipeDirectory
        {
            get => recipeDirectory;
            set => recipeDirectory = value;
        }

        public string CookingDataPath
        {
            get => cookingDataPath;
            set => cookingDataPath = value;
        }

        [JsonConstructor]
        public AppSettings(string recipeDirectory, string cookingDataPath)
        {
            this.recipeDirectory = recipeDirectory;
            this.cookingDataPath = cookingDataPath;
        }

        /// <summary>
        /// Generate a new <see cref="AppSettings"/> file.
        /// </summary>
        public AppSettings()
        {
        }

        /// <summary>
        /// Loads existing <see cref="AppSettings"/> from a json file.
        /// </summary>
        /// <param name="fi">A <see cref="FileInfo"/> object containing the path to the app settings</param>
        public AppSettings(FileInfo fi) : base(fi)
        {
            Load();
        }

        public void Load()
        {
            if (filePath == null)
            {
                return;
            }

            AppSettings appSettings = LoadFile();
            RecipeDirectory = appSettings.RecipeDirectory;
            CookingDataPath = appSettings.CookingDataPath;
        }

        public void Save(string parentPath = null)
        {
            if (!fromFile)
            {
                if (string.IsNullOrWhiteSpace(parentPath))
                {
                    throw new Exception("ParentPath needs to have a value if AppSettings file is being created!");
                }

                fileName = "settings.json";
                SaveFile(parentPath, this);
            }
            else
            {
                SaveFile(this);
            }
        }
    }
}
