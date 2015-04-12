using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HAS_Post.Models
{
    public class ItemPost
    {
        public Oid _id { get; set; }
        public String Place { get; set; }
        public String Product { get; set; }
        public String Usuario { get; set; }
        public String ImageProduct { get; set; }
        public String Time { get; set; }
        public String CoordPlace { get; set; } 
    }
}
