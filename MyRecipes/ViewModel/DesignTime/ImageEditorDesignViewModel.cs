using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class ImageEditorDesignViewModel : ImageEditorViewModel
    {
        public ImageEditorDesignViewModel()
        {
            ResultImage = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"C:\Users\rapha\source\repos\MyRecipes\Burger_Buns_small.jpg"));
        }
    }
}
