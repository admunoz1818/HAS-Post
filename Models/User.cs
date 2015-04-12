using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAS_Post.Models
{
    public class User
    {
        public Oid _id { get; set; }
        public String Name { get; set; }
        public String NameUser { get; set; }
        public String Photo { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
