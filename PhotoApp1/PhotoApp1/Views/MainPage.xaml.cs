using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace PhotoApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (InMemoryStorage.Photos.Any())
            {
                photoContainer.Children.Clear();

                foreach (var item in InMemoryStorage.Photos)
                {
                    var imageSource = ImageSource.FromStream(() =>
                    {

                        return new MemoryStream(item);
                    });

                    photoContainer.Children.Add(new Image() { HeightRequest = 300, Source = imageSource });
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CameraPage());
        }
    }
}
