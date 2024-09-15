using Microsoft.Maui.Controls;
using SkiaSharp;
using System.IO;

namespace StereogramApp
{
    public partial class CropPage : ContentPage
    {
        public CropPage()
        {
            InitializeComponent();
            LoadImage();
        }

        private void LoadImage()
        {
            OriginalImage.Source = ImageSource.FromStream(() => new MemoryStream(App.SelectedImageData));
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {
            using var bitmap = SKBitmap.Decode(App.SelectedImageData);
            int width = bitmap.Width / 3;
            int height = bitmap.Height;

            App.Image1 = CropBitmap(bitmap, new SKRectI(0, 0, width, height));
            App.Image2 = CropBitmap(bitmap, new SKRectI(width, 0, 2 * width, height));
            App.Image3 = CropBitmap(bitmap, new SKRectI(2 * width, 0, 3 * width, height));

            await Navigation.PushAsync(new AlignPage(1));
        }

        private byte[] CropBitmap(SKBitmap source, SKRectI rect)
        {
            using var cropped = new SKBitmap(rect.Width, rect.Height);
            using var canvas = new SKCanvas(cropped);
            canvas.DrawBitmap(source, rect, new SKRectI(0, 0, rect.Width, rect.Height));
            using var ms = new MemoryStream();
            cropped.Encode(ms, SKEncodedImageFormat.Jpeg, 100);
            return ms.ToArray();
        }
    }
}