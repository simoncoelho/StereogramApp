using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StereogramApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSelectPhotoClicked(object sender, EventArgs e)
        {
			var pickOptions = new PickOptions{
				FileTypes = FilePickerFileType.Images,
				PickerTitle = "Select a wigglegram image..."
			};
#if MACCATALYST
			var result = await MacFilePicker.PickAsync(pickOptions);
#else
			var result = await FilePicker.Default.PickAsync(pickOptions);
#endif

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                App.SelectedImageData = ReadFully(stream);
                await Navigation.PushAsync(new CropPage());
            }
        }

        private byte[] ReadFully(Stream input)
        {
            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}