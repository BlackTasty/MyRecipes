using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyRecipes.Core.Mobile.Transfer
{
    class RecipeImageTransfer
    {
        private byte[] imageBytes;

        public string ImageBytes => imageBytes != null ? Convert.ToBase64String(imageBytes) : null;

        public RecipeImageTransfer(RecipeImage recipeImage)
        {
            if (!string.IsNullOrWhiteSpace(recipeImage.FilePath))
            {
                imageBytes = File.ReadAllBytes(recipeImage.FilePath);
            }
        }

        private byte[] ImageToByteArray(BitmapImage image)
        {
            if (image == null)
            {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
