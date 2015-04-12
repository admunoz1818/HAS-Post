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
using System.Windows.Shapes;
using System.Windows.Media;
using HAS_Post.Geo;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using HAS_Post.Net;
using HAS_Post.Models;

namespace HAS_Post
{
    public partial class NewPost : PhoneApplicationPage, CloudinaryClient.ICloudinary
    {
        //Mongo
        Mongo<ItemPost> itemPostMongo;

        //Cloudinary
        const String url = "https://api.cloudinary.com/v1_1/admunoz/image/upload";
        PhotoChooserTask photoTask;
        CameraCaptureTask cameraTask;
        BitmapImage image;
        ImageBrush imgBrush;
        CloudinaryClient client;
        String urlI;
        String place;

        public NewPost()
        {
            InitializeComponent();

            //Cloudinary
            photoTask = new PhotoChooserTask();
            photoTask.Completed += new EventHandler<PhotoResult>(task_Completed);
            cameraTask = new CameraCaptureTask();
            cameraTask.Completed += new EventHandler<PhotoResult>(task_Completed);
            image = new BitmapImage();
            imgBrush = new ImageBrush();
            client = new CloudinaryClient(this, url);

            //Map
            ShowMyLocationOnTheMap();

            //Mongo
            itemPostMongo = new Mongo<ItemPost>("9NlswL-HnWVU8mwH5zi8B8mgF7us7wHl", "hereandshare", "itemsPost");
        }

        /*For see you location on the map*/
        private async void ShowMyLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);
            // Make my current location the center of the Map. 
            //MessageBox.Show("Latitude" + myGeoCoordinate.Latitude+ "Longitud: " + myGeoCoordinate.Longitude + "Altitud:"+myGeoCoordinate.Altitude);
            place = myGeocoordinate.ToGeoCoordinate().ToString();
            this.mapWithMyLocation.Center = myGeoCoordinate;
            this.mapWithMyLocation.ZoomLevel = 13;
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
            mapWithMyLocation.Layers.Add(myLocationLayer);
        }

        private void ButtonCamaraTake_Click(object sender, RoutedEventArgs e)
        {
            cameraTask.Show();
        }

        private void ButtonGaleryImage_Click(object sender, RoutedEventArgs e)
        {
            photoTask.Show();
        }

        private void ButtonNewPost_Click(object sender, RoutedEventArgs e)
        {
            uploadImage();
            MessageBox.Show("Su recomendación será publicada en segundos.", "¡Aviso!", MessageBoxButton.OK);
            NavigationService.Navigate(new Uri("/Main.xaml?PivotMain.SelectedIndex = 1", UriKind.Relative));          
        }

        /*Methods to Photos*/
        private void task_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                info.Text = "Imagen cargada";
                image.SetSource(e.ChosenPhoto);
                imgBrush.ImageSource = image;
                newImage.Fill = imgBrush;
            }
        }
        
        public void urlImg(String urlI)
        {
            info.Text = "Imagen subida";
            this.urlI = urlI;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(0xff, 0, 0, 0);
            newImage.Fill = brush;

            //*ItemPost*//
            try
            {
                ItemPost newPost = new ItemPost()
                {
                    Place = NewPlace.Text,
                    Product = newProduct.Text,
                    Usuario = PhoneApplicationService.Current.State["Username"].ToString(),
                    ImageProduct = urlI,
                    Time = DateTime.Now.ToShortDateString(),
                    CoordPlace = place
                };
                itemPostMongo.insertDocument(newPost);
                MessageBox.Show("Recomendación publicada", "¡Aviso!", MessageBoxButton.OK);
                NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative)); 
            }
            catch (Exception)
            {
                MessageBox.Show("Compruebe la conexión a internet", "Error de conexión", MessageBoxButton.OK);
                throw;
            }            
        }
       
        private void downloadImage()
        {
            info.Text = "Cargando imagen";
            image = new BitmapImage(new Uri(urlI));
            imgBrush.ImageSource = image;
            newImage.Fill = imgBrush;
        }
        
        private void uploadImage()
        {
            info.Text = "Subiendo imagen";
            String name = "img_" + DateTime.Now;
            client.updateImage(image, name);
        }

        private void NewPlace_GotFocus(object sender, RoutedEventArgs e)
        {
            NewPlace.Text = "";
        }

        private void NewPlace_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NewPlace.Text == "")
            {
                NewPlace.Text = "¿Dónde estás?";
            }
        }

        //To convert date in string
        public String convertTime(DateTime date)
        {
            int remMon = System.DateTime.Now.Month - date.Month;
            int remDay = System.DateTime.Now.Day - date.Day;
            int remHour = System.DateTime.Now.Hour - date.Hour;
            int remMin = System.DateTime.Now.Minute - date.Minute;

            if (remMin < 0)
                remMin = remMin * (-1);

            String answer = "0 tiempo";

            if (remHour < 1)
            {
                answer = remMin + " mins";
            }
            else
            {
                if (remHour == 1)
                {
                    answer = remHour + " hor";
                }
                else
                {
                    if (remHour >= 2 && remHour <= 24)
                    {
                        answer = remHour + " horas";
                    }
                    else
                    {
                        if (remDay < 7)
                        {
                            answer = remDay + " días";
                        }
                        else
                        {
                            if (remDay >= 7 && remDay <= 14) { answer = "2 semanas"; }
                            if (remDay >= 15 && remDay <= 21) { answer = "3 semanas"; }
                            if (remDay >= 22 && remDay <= 28) { answer = "4 semanas"; }
                            if (remDay >= 29) { answer = "1 mes"; }
                            if (remMon > 1) { answer = remMon + " meses"; }
                        }

                    }
                }
            }
            return answer;
        }
                  
    }
}