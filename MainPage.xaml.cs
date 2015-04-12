using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HAS_Post.Resources;
using HAS_Post.Net;
using HAS_Post.Models;

namespace HAS_Post
{
    public partial class MainPage : PhoneApplicationPage, Mongo<User>.IMongo
    {
        //Mongo
        Mongo<User> userMongo;
        DataModel dmUsers;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            userMongo = new Mongo<User>("9NlswL-HnWVU8mwH5zi8B8mgF7us7wHl", "hereandshare", "users");
            dmUsers = Application.Current.Resources["dataModel"] as DataModel;
            userMongo.findAllDocuments(this);
        }

        private void LinkGoToMain_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["Username"] = "Default";
            NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative));
        }

        private void LinkRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewUser.xaml", UriKind.Relative));
        }

        private void LinkLoginUser(object sender, RoutedEventArgs e)
        {
            Boolean findUser = false;
            for (int i = 0; i < dmUsers.DataUser.Count; i++)
			{
                if (loginEmail.Text.Equals(dmUsers.DataUser.ElementAt(i).Email) && loginPass.Password.Equals(dmUsers.DataUser.ElementAt(i).Password))
                {
                    PhoneApplicationService.Current.State["Username"] = dmUsers.DataUser.ElementAt(i).NameUser; 
                    i = dmUsers.DataUser.Count;
                    findUser = true;
                }                
			}
            if (findUser)
            {
                NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Usuario no encontrado, por favor verificar sus datos.", "¡Inicio de Sesión!", MessageBoxButton.OK);
            }
        }

        public void loadDocuments(List<User> documents)
        {            
            dmUsers.DataUser.Clear();
            for (int i = 0; i < documents.Count; i++)
            {
                dmUsers.DataUser.Add(documents.ElementAt(i));
            }
        }
    }
}