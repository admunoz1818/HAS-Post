using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using System.Device.Location;
using HAS_Post.Geo;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using System.Text.RegularExpressions;

namespace HAS_Post
{
    public partial class ViewPlace : PhoneApplicationPage
    {
        String coordenadas;
        public ViewPlace()
        {
            InitializeComponent();
            coordenadas = "";
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("coord"))
            {
                coordenadas = NavigationContext.QueryString["coord"];                
                string[] strArr = null;
                char[] splitchar = {',',' '};
                strArr = coordenadas.Split(splitchar);
                ShowMyLocationOnTheMap(strArr);
            }
        }

        //private void ReturnFromPlaceToMain_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative)); 
        //}
        /*For see you location on the map*/
        private async void ShowMyLocationOnTheMap(string[] strArr)
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
           
            GeoCoordinate myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);
            // Make my current location the center of the Map.  
            try
            {
                if (!(strArr[0] == null) && !(strArr[2] == null))
                {
                    myGeoCoordinate.Latitude = Convert.ToDouble(strArr[0].Replace('.', ','));
                    myGeoCoordinate.Longitude = Convert.ToDouble(strArr[2].Replace('.', ','));
                }
            }
            catch 
            {
                MessageBox.Show("No se puede ubicar el lugar.", "¡Aviso!", MessageBoxButton.OK);
            }
            
            this.mapPlaceRecommendation.Center = myGeoCoordinate;
            this.mapPlaceRecommendation.ZoomLevel = 13;
            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;
            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;
            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            // Add the MapLayer to the Map.
            mapPlaceRecommendation.Layers.Add(myLocationLayer);
        }
    }
}