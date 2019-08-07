using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClassCreator
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //DbProcess db = new DbProcess();
        MainCreator mc = new MainCreator();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {   
            
            mc.ServerBilgileri(TextBox1.Text,TextBox2.Text,TextBox3.Text,TextBox4.Text);
            mc.ClasslariOlusturma();
            
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //Ogrenci t = new Ogrenci()
            //{
            //    id = 1,
            //    ad = "sura",
            //    soyad = "balli",
            //    mail = "gmail"
            //};

            //db.VeriGuncelleme(t);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}