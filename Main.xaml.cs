using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HAS_Post.Net;
using HAS_Post.Models;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace HAS_Post
{
    public partial class Main : PhoneApplicationPage, Mongo<ItemPost>.IMongo, Mongo<User>.IMongo, CloudinaryClient.ICloudinary
    {
        //Mongo
        Mongo<ItemPost> itemMongo;
        Mongo<User> userMongo;

        //Cloudinary
        const String url = "https://api.cloudinary.com/v1_1/admunoz/image/upload";
        PhotoChooserTask photoTask;
        BitmapImage image;
        ImageBrush imgBrush;
        CloudinaryClient client;
        String urlI;

        //DataModel
        DataModel dmUserOne;
        DataModel dmPost;

        //Constructor
        public Main()
        {
            InitializeComponent();
            CancelEditProfile.IsEnabled = false;
            SaveChangeProfile.IsEnabled = false;            
            itemMongo = new Mongo<ItemPost>("9NlswL-HnWVU8mwH5zi8B8mgF7us7wHl", "hereandshare", "itemsPost");
            itemMongo.findAllDocuments(this);
            userMongo = new Mongo<User>("9NlswL-HnWVU8mwH5zi8B8mgF7us7wHl", "hereandshare", "users");
            if (!PhoneApplicationService.Current.State["Username"].Equals("Default"))
            {
                SaveChangeProfile.IsEnabled = true;
                userMongo.findOneDocument(this, "NameUser", PhoneApplicationService.Current.State["Username"].ToString());
            }

            //downloadImage(dmUserOne.DataUserOne.ElementAt(0).Photo);
                
            //Cloudinary
            photoTask = new PhotoChooserTask();
            photoTask.Completed += new EventHandler<PhotoResult>(task_Completed);
            image = new BitmapImage();
            imgBrush = new ImageBrush();
            client = new CloudinaryClient(this, url);
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBarDescubre"]);
                    break;
                case 1:
                    ApplicationBar = ((ApplicationBar)Application.Current.Resources["AppBarPerfil"]);
                    break;
            }
        }

        private void LinkGoToProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        public void loadDocuments(List<ItemPost> documents)
        {
            dmPost = Application.Current.Resources["dataModel"] as DataModel;
            dmPost.Data.Clear();
            for (int i = 0; i < documents.Count; i++)
            {
                dmPost.Data.Add(documents.ElementAt(i));                
            }
        }

        public void loadDocuments(List<User> documents)
        {
            dmUserOne = Application.Current.Resources["dataModel"] as DataModel;
            dmUserOne.DataUserOne.Clear();
            dmUserOne.DataUserOne.Add(documents.ElementAt(0));
            ttbName.Text = dmUserOne.DataUserOne.ElementAt(0).Name;
            ttbUser.Text = dmUserOne.DataUserOne.ElementAt(0).NameUser;
            ttbEmail.Text = dmUserOne.DataUserOne.ElementAt(0).Email;
            ttbPassword.Password = dmUserOne.DataUserOne.ElementAt(0).Password;
            image = new BitmapImage(new Uri(dmUserOne.DataUserOne.ElementAt(0).Photo));
            imgBrush.ImageSource = image;
            ImageProfile.Fill = imgBrush;
        }

        private void ImageProfile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }

        private void ChangedPhotoProfile_Click(object sender, RoutedEventArgs e)
        {
            photoTask.Show();
        }


        /*Methods to Photos*/
        private void task_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                image.SetSource(e.ChosenPhoto);
                imgBrush.ImageSource = image;
                ImageProfile.Fill = imgBrush;
            }
        }

        public void urlImg(String urlI)
        {
            this.urlI = urlI;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(0xff, 0, 0, 0);
            ImageProfile.Fill = brush;

            //*ItemPost*//
            try
            {
                User updateUser = new User()
                {
                    Name = ttbName.Text,
                    NameUser = ttbUser.Text,
                    Email = ttbEmail.Text,
                    Password = ttbPassword.Password,
                    Photo = urlI
                };
               // MessageBox.Show("iOd:" + dmUserOne.dataUserOne.ElementAt(0)._id.oid);
                userMongo.updateDocument(updateUser,dmUserOne.dataUserOne.ElementAt(0)._id.oid);
                MessageBox.Show("Perfil actualizado", "¡Aviso!", MessageBoxButton.OK);
                //NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                MessageBox.Show("Compruebe la conexión a internet", "Error de conexión", MessageBoxButton.OK);
                throw;
            }
        }

        private void imagePost_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image aux = sender as Image;
            ItemPost auxItem = new ItemPost();
            auxItem = aux.DataContext as ItemPost;
            NavigationService.Navigate(new Uri("/ViewImageProduct.xaml?urlImage=" + auxItem.ImageProduct, UriKind.Relative)); 
  
        }

        private void uploadImage()
        {            
            String name = "img_" + DateTime.Now;
            client.updateImage(image, name);
        }

        private void SaveChangeProfile_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChangeProfile.Content.Equals("Guardar"))
            {
                uploadImage();
                ChangedPhotoProfile.IsEnabled = false;
                ttbName.IsReadOnly = true;
                ttbEmail.IsReadOnly = true;
                ttbPassword.IsHitTestVisible = false;
                CancelEditProfile.IsEnabled = false;
                SaveChangeProfile.Content = "Editar";
            }
            else {
                CancelEditProfile.IsEnabled = true;
                ChangedPhotoProfile.IsEnabled = true;
                ttbName.IsReadOnly = false;
                ttbEmail.IsReadOnly = false;
                ttbPassword.IsHitTestVisible = true;
                SaveChangeProfile.Content = "Guardar";
            }
        }

        private void CancelEditProfile_Click(object sender, RoutedEventArgs e)
        {
            ChangedPhotoProfile.IsEnabled = false;
            ttbName.IsReadOnly = true;
            ttbEmail.IsReadOnly = true;
            ttbPassword.IsHitTestVisible = false;
            CancelEditProfile.IsEnabled = false;
            SaveChangeProfile.Content = "Editar";
        }

        private void BtnPlace_Click(object sender, RoutedEventArgs e)
        {
            Button coord = sender as Button;
            String text = coord.Content.ToString();
            ItemPost auxItem = new ItemPost();
            auxItem = coord.DataContext as ItemPost;
            //MessageBox.Show("Coord: " + auxItem.CoordPlace);
            NavigationService.Navigate(new Uri("/ViewPlace.xaml?coord=" + auxItem.CoordPlace, UriKind.Relative)); 
        }

        private void downloadImage(String urlImagenProfile)
        {
            
        }

          
    }
}