using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ClassCreator
{
public class DbProcess
{
public SqlConnection con = new SqlConnection("Server = localhost; Database = test; Trusted_Connection = True;");
public SqlCommand cmd;
public SqlDataReader dr;
public void VeriEkleme(Ogrenci ogrenci)
{
con.Open();
cmd = new SqlCommand("INSERT INTO ogrenci(ad,soyad,mail) VALUES(@ad,@soyad,@mail)",con);
cmd.Parameters.AddWithValue("@ad",ogrenci.ad);
cmd.Parameters.AddWithValue("@soyad",ogrenci.soyad);
cmd.Parameters.AddWithValue("@mail",ogrenci.mail);
cmd.ExecuteNonQuery();
con.Close();
cmd.Dispose();
}
public void VeriGuncelleme(Ogrenci ogrenci)
{
con.Open();
cmd = new SqlCommand("UPDATE ogrenci SET ad=@ad,soyad=@soyad,mail=@mail WHERE id=@id",con);
cmd.Parameters.AddWithValue("@id",ogrenci.id);
cmd.Parameters.AddWithValue("@ad",ogrenci.ad);
cmd.Parameters.AddWithValue("@soyad",ogrenci.soyad);
cmd.Parameters.AddWithValue("@mail",ogrenci.mail);
cmd.ExecuteNonQuery();
con.Close();
cmd.Dispose();
}
public void VeriSilme(Ogrenci ogrenci)
{
con.Open();
cmd = new SqlCommand("DELETE FROM ogrenci WHERE id=@id",con);
cmd.Parameters.AddWithValue("@id",ogrenci.id);
cmd.ExecuteNonQuery();
con.Close();
cmd.Dispose();
}
public List<Ogrenci> VeriGetirme()
{
List<Ogrenci> result = new List<Ogrenci>();
con.Open();
cmd = new SqlCommand("SELECT * FROM ogrenci",con);
dr=cmd.ExecuteReader();
while(dr.Read())
{
Ogrenci ogrenci = new Ogrenci()
{
id = dr[0],
ad = dr[1],
soyad = dr[2],
mail = dr[3],
};
result.Add(ogrenci);
}
con.Close();
dr.Close();
cmd.Dispose();
return result;
}
}
}
