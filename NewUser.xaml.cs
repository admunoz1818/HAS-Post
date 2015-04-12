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

namespace HAS_Post
{
    public partial class NewUser : PhoneApplicationPage, Mongo<User>.IMongo
    {
        //Mongo
        Mongo<User> userMongo;
        DataModel dmUsers;

        public NewUser()
        {
            InitializeComponent();
            //Mongo
            userMongo = new Mongo<User>("9NlswL-HnWVU8mwH5zi8B8mgF7us7wHl", "hereandshare", "users");
            dmUsers = Application.Current.Resources["dataModel"] as DataModel;
            userMongo.findAllDocuments(this);    
        }

        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            Boolean findUser = false; 

            for (int i = 0; i < dmUsers.DataUser.Count; i++)
            {
                if (newUser.Text.Equals(dmUsers.DataUser.ElementAt(i).NameUser))
                {
                    i = dmUsers.dataUser.Count;
                    findUser = true;
                }                
            }

            if (!findUser)
            {
                if (newPass1.Password.Equals(newPass2.Password))
                {
                    try
                    {
                        User nUser = new User()
                        {
                            Name = newName.Text,
                            NameUser = newUser.Text,
                            Photo = "http://res.cloudinary.com/admunoz/image/upload/v1428709228/NoImage_ie9zsf.jpg",
                            Email = newEmail.Text,
                            Password = newPass1.Password
                        };
                        userMongo.insertDocument(nUser);
                        MessageBox.Show("Registrado con éxito!", "¡Mensaje!", MessageBoxButton.OK);
                        PhoneApplicationService.Current.State["Username"] = newUser.Text;
                        userMongo.findAllDocuments(this);
                        NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Compruebe la conexión a internet", "Error de conexión", MessageBoxButton.OK);
                        throw;
                    }
                }
                else
                {
                    MessageBox.Show("Contraseñas no son iguales.", "Error, verificar contraseñas", MessageBoxButton.OK);
                    newPass1.Password = "";
                    newPass2.Password = "";
                }  
            }
            else
            {
                MessageBox.Show("Nombre de usuario ya existe, debe cambiar su nombre de usuario.", "¡Inicio de Sesión!", MessageBoxButton.OK);
            }
        }

        private void GoFromRegistrationToMain_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["Username"] = "Default";
            NavigationService.Navigate(new Uri("/Main.xaml", UriKind.Relative));
        }

        public void loadDocuments(List<User> documents)
        {
            dmUsers.Data.Clear();
            for (int i = 0; i < documents.Count; i++)
            {
                dmUsers.DataUser.Add(documents.ElementAt(i));
            }
        }
       
    }
}