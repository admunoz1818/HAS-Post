using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace HAS_Post
{
    public partial class ViewImageProduct : PhoneApplicationPage
    {
        String urlImage;
        BitmapImage image;

        public ViewImageProduct()
        {
            InitializeComponent();
            urlImage = "";
            image = new BitmapImage();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("urlImage"))
            {
                urlImage = NavigationContext.QueryString["urlImage"];
                image = new BitmapImage(new Uri(urlImage));                
                extendImage.Source = image; 
            }
        }

        private void ReturnToMain_Click(object sender, RoutedEventArgs e)
        {
            urlImage = null;
            image = null;
            NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative)); 
        }


    }
}