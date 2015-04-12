using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAS_Post.Models
{
    public class DataModel
    {
        //Atribute
        public ObservableCollection<ItemPost> data;       
        
        //Property
        public ObservableCollection<ItemPost> Data
        {
            get
            {
                if (data == null)
                    data = new ObservableCollection<ItemPost>();
                return data;
            }
            set
            {
                data = value;
            }
        }

        public ObservableCollection<User> dataUser;
        public ObservableCollection<User> DataUser
        {
            get
            {
                if (dataUser == null)
                    dataUser = new ObservableCollection<User>();
                return dataUser;
            }
            set
            {
                dataUser = value;
            }
        }

        public ObservableCollection<User> dataUserOne;
        public ObservableCollection<User> DataUserOne
        {
            get
            {
                if (dataUserOne == null)
                    dataUserOne = new ObservableCollection<User>();
                return dataUserOne;
            }
            set
            {
                dataUserOne = value;
            }
        }
    }
}
