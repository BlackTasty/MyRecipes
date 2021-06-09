using MyRecipes.Core;
using MyRecipes.Core.Mobile;
using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tasty.ViewModel;
using Tasty.ViewModel.JsonNet;

namespace MyRecipes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyRecipes");

        private static CookingData cookingData = new CookingData(false);
        private static JsonObservableCollection<Recipe> availableRecipes = new JsonObservableCollection<Recipe>("AvailableRecipes");
        private static VeryObservableStackCollection<Recipe> mHistory = new VeryObservableStackCollection<Recipe>("History", 10);

        private static MqttBroker server = new MqttBroker();

        public static string BasePath => basePath;

        public static string DefaultRecipePath => Path.Combine(BasePath, "recipes");

        public static string DefaultCookingDataPath => Path.Combine(BasePath);

        internal static CookingData CookingData => cookingData;

        public static VeryObservableStackCollection<Recipe> History => mHistory;

        public static JsonObservableCollection<Ingredient> AvailableIngredients => cookingData.AvailableIngredients;

        public static JsonObservableCollection<Category> AvailableCategories => cookingData.AvailableCategories;

        public static JsonObservableCollection<Recipe> AvailableRecipes => availableRecipes;

        public static AppSettings Settings
        {
            get; set;
        }

        public static MqttBroker Server => server;

        [STAThread]
        public static void Main()
        {
            FileInfo fi = new FileInfo("settings.json");
            if (fi.Exists)
            {
                Settings = new AppSettings(fi, true);
            }
            else
            {
                Settings = new AppSettings(true);
                Settings.Save(AppDomain.CurrentDomain.BaseDirectory);
            }

            LoadCookingData();
            LoadRecipes();
            LoadHistory();

            App app = new App()
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose
            };
            app.InitializeComponent();
            app.Run();

            SaveCookingData();
            SaveRecipes();
        }

        public static void LoadCookingData()
        {
            if (!Directory.Exists(Settings.CookingDataPath))
            {
                Directory.CreateDirectory(Settings.CookingDataPath);
            }

            string filePath = Path.Combine(Settings.CookingDataPath, "data.json");
            if (File.Exists(filePath))
            {
                cookingData = new CookingData(new FileInfo(filePath), false);
            }
            else
            {
                cookingData = new CookingData(false);
            }
        }

        public static void SaveCookingData()
        {
            cookingData.Save(Settings.CookingDataPath);
        }

        public static void LoadRecipes()
        {
            if (!Directory.Exists(Settings.RecipeDirectory))
            {
                Directory.CreateDirectory(Settings.RecipeDirectory);
            }

            AvailableRecipes.Clear();
            foreach (FileInfo fi in new DirectoryInfo(Settings.RecipeDirectory).EnumerateFiles("*.json"))
            {
                AvailableRecipes.Add(new Recipe(fi));
            }
        }

        public static void SaveRecipes()
        {
            foreach (Recipe recipe in AvailableRecipes)
            {
                recipe.Save(Settings.RecipeDirectory);
            }
        }

        public static void LoadHistory()
        {
            History.Clear();

            if (!Directory.Exists(Settings.RecipeDirectory))
            {
                Directory.CreateDirectory(Settings.RecipeDirectory);
            }

            List<Recipe> history = new List<Recipe>();
            foreach (FileInfo fi in new DirectoryInfo(Settings.RecipeDirectory).EnumerateFiles())
            {
                history.Add(new Recipe(fi));
            }

            History.AddRange(history.Where(x => x.LastAccessDate > new DateTime(0))
                .OrderBy(x => x.LastAccessDate).Reverse().Take(History.Limit));
        }
    }
}
