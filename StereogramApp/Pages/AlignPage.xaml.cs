using Microsoft.Maui.Controls;
using SkiaSharp;
using System;
using System.IO;

namespace StereogramApp
{
    public partial class AlignPage : ContentPage
    {
        private int step;
        private double translateX = 0;
        private double translateY = 0;

        public AlignPage(int step)
        {
            InitializeComponent();
            this.step = step;
            LoadImages();
            AddPanGesture();
        }

        private void LoadImages()
        {
            byte[] baseImageData = step == 1 ? App.Image1 : App.Image2;
            byte[] alignImageData = step == 1 ? App.Image2 : App.Image3;

            BaseImage.Source = ImageSource.FromStream(() => new MemoryStream(baseImageData));
            AlignImage.Source = ImageSource.FromStream(() => new MemoryStream(alignImageData));
        }

        private void AddPanGesture()
        {
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            AlignImage.GestureRecognizers.Add(panGesture);
        }

		private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
		{
			switch (e.StatusType)
			{
				case GestureStatus.Running:
					// As the pan gesture is running, update the translation of the image
					AlignImage.TranslationX = e.TotalX;
					AlignImage.TranslationY = e.TotalY;
					break;

				case GestureStatus.Completed:
					// Optionally, handle when the pan gesture completes
					break;
			}
		}
        private async void OnNextClicked(object sender, EventArgs e)
        {
            byte[] alignImageData = step == 1 ? App.Image2 : App.Image3;
            byte[] adjustedImage = AdjustImageAlignment(alignImageData, translateX, translateY);

            if (step == 1)
                App.Image2 = adjustedImage;
            else
                App.Image3 = adjustedImage;

            if (step == 1)
                await Navigation.PushAsync(new AlignPage(2));
            else
				await Navigation.PushAsync(new AlignPage(2));
				//await Navigation.PushAsync(new PreviewPage());
        }

        private byte[] AdjustImageAlignment(byte[] imageData, double offsetX, double offsetY)
        {
            using var bitmap = SKBitmap.Decode(imageData);
            using var adjustedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
            using var canvas = new SKCanvas(adjustedBitmap);
            canvas.DrawBitmap(bitmap, (float)offsetX, (float)offsetY);
            using var ms = new MemoryStream();
            adjustedBitmap.Encode(ms, SKEncodedImageFormat.Jpeg, 100);
            return ms.ToArray();
        }
    }
}