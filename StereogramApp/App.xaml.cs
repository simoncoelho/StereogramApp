using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;

namespace StereogramApp // Ensure this matches your project's namespace
{
    public partial class App : Application
    {
        public static byte[] SelectedImageData { get; set; }
        public static byte[] Image1 { get; set; }
        public static byte[] Image2 { get; set; }
        public static byte[] Image3 { get; set; }
        public static byte[] WigglegramData { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
    }
}