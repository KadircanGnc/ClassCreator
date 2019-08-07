using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ClassCreator
{
    public class Ogrenci
    {
        public dynamic id { get; set; }
        public dynamic ad { get; set; }
        public dynamic soyad { get; set; }
        public dynamic mail { get; set; }
        public Ogrenci()
        {
         
        }
    }
}
