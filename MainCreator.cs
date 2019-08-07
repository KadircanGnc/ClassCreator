using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;

namespace ClassCreator
{

    public class MainCreator
    {
        public static SqlConnection con;
        public SqlCommand cmd;
        public SqlDataReader dr;
        public string serverAdi { get; set; }
        public string kullaniciAdi { get; set; }
        public string sifre { get; set; }
        public string databaseAdi { get; set; }
        public string serverBilgileriDosyaYolu { get; set; }
        public static string tabloAdi { get; set; }
        public string classDosyaYolu { get; set; }
        public string dbProcessDosyaYolu { get; set; }
        public string aspxDosyaYolu { get; set; }
        public string aspxCsDosyaYolu { get; set; }
        public string aspxDesignerCsDosyaYolu { get; set; }
        public List<string> databaseBilgileri { get; set; }
        public List<string> kolonAdlari { get; set; }
        public string classAttributeAdi { get; set; }
        public string classAdi { get; set; }
        public FileStream ServerBilgileri(string serverAdi, string databaseAdi, string kullaniciAdi, string sifre)
        {
            FileStream result;
            string serverBilgileriDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\serverBilgileri.txt";
            if (File.Exists(serverBilgileriDosyaYolu) == false)
            {
                result = File.Create(serverBilgileriDosyaYolu);
                result.Close();
            }
            if (serverAdi == "localhost")
            {
                result = new FileStream(serverBilgileriDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter resultSw = new StreamWriter(result);
                resultSw.WriteLine(serverAdi);
                resultSw.WriteLine(databaseAdi);
                resultSw.WriteLine();
                resultSw.WriteLine();
                resultSw.Flush();
                resultSw.Close();
                result.Close();
            }
            else
            {
                result = new FileStream(serverBilgileriDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter resultSw = new StreamWriter(result);
                resultSw.WriteLine(serverAdi);
                resultSw.WriteLine(databaseAdi);
                resultSw.WriteLine(kullaniciAdi);
                resultSw.WriteLine(sifre);
                resultSw.Flush();
                resultSw.Close();
                result.Close();
            }
            return result;
        }
        public FileStream ClasslariOlusturma()
        {
            #region ServerBilgilerini Okuma            
            databaseBilgileri = new List<string>();
            kolonAdlari = new List<string>();
            serverBilgileriDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\serverBilgileri.txt";
            FileStream result = new FileStream(serverBilgileriDosyaYolu, FileMode.Open, FileAccess.Read);
            StreamReader serverBilgileriSW = new StreamReader(result);
            string gelendatabaseBilgileri = serverBilgileriSW.ReadLine();
            while (gelendatabaseBilgileri != null)
            {
                databaseBilgileri.Add(gelendatabaseBilgileri);
                gelendatabaseBilgileri = serverBilgileriSW.ReadLine();
            }
            serverBilgileriSW.Close();
            result.Close();
            serverAdi = databaseBilgileri[0];
            databaseAdi = databaseBilgileri[1];
            kullaniciAdi = databaseBilgileri[2];
            sifre = databaseBilgileri[3];
            #endregion
            #region Connection Tanımlama
            if (serverAdi == "localhost")
            {
                con = new SqlConnection("Server = localhost; Database = " + databaseAdi + "; Trusted_Connection = True;");
            }
            else
            {
                con = new SqlConnection("Server = " + serverAdi + "; Database = " + databaseAdi + "; User Id = " + kullaniciAdi + "; Password =" + sifre + ";");
            }
            #endregion
            #region Tablo Adını Çekme
            con.Open();
            cmd = new SqlCommand("SELECT name FROM " + databaseAdi + ".sys.tables WHERE name NOT LIKE 'sysdiagrams'", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tabloAdi = (string)dr[0];
            }
            con.Close();
            cmd.Dispose();
            dr.Close();
            classAttributeAdi = tabloAdi.Substring(0, 1).ToLower() + tabloAdi.Substring(1, tabloAdi.Length - 1);
            classAdi = tabloAdi.Substring(0, 1).ToUpper() + tabloAdi.Substring(1, tabloAdi.Length - 1);
            #endregion
            #region Kolon Adlarını Çekme
            con.Open();
            cmd = new SqlCommand("USE " + databaseAdi + " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tabloAdi + "'", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                kolonAdlari.Add((string)dr["COLUMN_NAME"]);
            }
            con.Close();
            dr.Close();
            cmd.Dispose();
            #endregion
            #region Tablo Adında Class Oluşturma            
            classDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\" + classAdi + ".cs";
            FileStream classOlusturmaFS = File.Create(@classDosyaYolu);
            classOlusturmaFS.Close();
            classOlusturmaFS = new FileStream(classDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter classOlusturmaSW = new StreamWriter(classOlusturmaFS);
            classOlusturmaSW.WriteLine("using System;");
            classOlusturmaSW.WriteLine("using System.Collections.Generic;");
            classOlusturmaSW.WriteLine("using System.Linq;");
            classOlusturmaSW.WriteLine("using System.Web;");
            classOlusturmaSW.WriteLine("using System.Data.SqlClient;");
            classOlusturmaSW.WriteLine("");
            classOlusturmaSW.WriteLine("namespace ClassCreator");
            classOlusturmaSW.WriteLine("{");
            classOlusturmaSW.WriteLine("    public class " + classAdi);
            classOlusturmaSW.WriteLine("    {");
            for (int i = 0; i <= kolonAdlari.Count-1; i++)
            {
                classOlusturmaSW.WriteLine("        public dynamic " + kolonAdlari[i] + " { get; set; }");
            }
            classOlusturmaSW.WriteLine("        public " + classAdi + "()");
            classOlusturmaSW.WriteLine("        {");
            classOlusturmaSW.WriteLine("         ");
            classOlusturmaSW.WriteLine("        }");
            classOlusturmaSW.WriteLine("    }");
            classOlusturmaSW.WriteLine("}");
            classOlusturmaSW.Flush();
            classOlusturmaSW.Close();
            classOlusturmaFS.Close();
            #endregion
            #region DbProcess Oluşturma            
            dbProcessDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\DbProcess.cs";
            FileStream dbProcessFS = File.Create(@dbProcessDosyaYolu);
            dbProcessFS.Close();
            dbProcessFS = new FileStream(dbProcessDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter dbProcessSW = new StreamWriter(dbProcessFS);

            dbProcessSW.WriteLine("using System;");
            dbProcessSW.WriteLine("using System.Collections.Generic;");
            dbProcessSW.WriteLine("using System.Linq;");
            dbProcessSW.WriteLine("using System.Web;");
            dbProcessSW.WriteLine("using System.Data.SqlClient;");
            dbProcessSW.WriteLine("");
            dbProcessSW.WriteLine("namespace ClassCreator");
            dbProcessSW.WriteLine("{");
            dbProcessSW.WriteLine("public class DbProcess");
            dbProcessSW.WriteLine("{");
            dbProcessSW.WriteLine("public SqlConnection con = new SqlConnection(\"" + con.ConnectionString + "\");");
            dbProcessSW.WriteLine("public SqlCommand cmd;");
            dbProcessSW.WriteLine("public SqlDataReader dr;");            
            #region Veri Ekleme bölümü
            dbProcessSW.WriteLine("public void VeriEkleme(" + classAdi + " " + classAttributeAdi + ")");
            dbProcessSW.WriteLine("{");            
            dbProcessSW.WriteLine("con.Open();");
            string kolonAdi = "";
            string parametre = "";
            for (int i = 1; i <= kolonAdlari.Count - 1; i++)
            {
                kolonAdi += kolonAdlari[i] + ",";
                parametre += "@" + kolonAdlari[i] + ",";
            }
            dbProcessSW.WriteLine("cmd = new SqlCommand(\"INSERT INTO " + tabloAdi + "(" + kolonAdi.Substring(0, kolonAdi.Length - 1) + ") VALUES(" + parametre.Substring(0, parametre.Length - 1) + ")\",con);");
            for(int y=1;y<=kolonAdlari.Count-1;y++)
            {
                dbProcessSW.WriteLine("cmd.Parameters.AddWithValue(\"@" + kolonAdlari[y] + "\"," + classAttributeAdi + "." + kolonAdlari[y] + ");");
            }
            dbProcessSW.WriteLine("cmd.ExecuteNonQuery();");            
            dbProcessSW.WriteLine("con.Close();");
            dbProcessSW.WriteLine("cmd.Dispose();");
            dbProcessSW.WriteLine("}");
            #endregion
            #region Veri Güncelleme Bölümü
            dbProcessSW.WriteLine("public void VeriGuncelleme(" + classAdi + " " + classAttributeAdi + ")");
            dbProcessSW.WriteLine("{");
            dbProcessSW.WriteLine("con.Open();");
            string updateParametre = "";
            for(int z=1;z<=kolonAdlari.Count-1;z++)
            {
                updateParametre += kolonAdlari[z] + "=@" + kolonAdlari[z] + ",";
            }
            dbProcessSW.WriteLine("cmd = new SqlCommand(\"UPDATE " + tabloAdi + " SET " + updateParametre.Substring(0, updateParametre.Length - 1) + " WHERE " + kolonAdlari[0] + "=@" + kolonAdlari[0] + "\",con);");
            for(int i=0;i<=kolonAdlari.Count-1;i++)
            {
                dbProcessSW.WriteLine("cmd.Parameters.AddWithValue(\"@" + kolonAdlari[i] + "\"," + classAttributeAdi + "." + kolonAdlari[i] + ");");
            }
            dbProcessSW.WriteLine("cmd.ExecuteNonQuery();");
            dbProcessSW.WriteLine("con.Close();");
            dbProcessSW.WriteLine("cmd.Dispose();");
            dbProcessSW.WriteLine("}");
            #endregion
            #region Veri Silme Bölümü
            dbProcessSW.WriteLine("public void VeriSilme(" + classAdi + " " + classAttributeAdi + ")");
            dbProcessSW.WriteLine("{");
            dbProcessSW.WriteLine("con.Open();");
            dbProcessSW.WriteLine("cmd = new SqlCommand(\"DELETE FROM " + tabloAdi + " WHERE " + kolonAdlari[0] + "=@" + kolonAdlari[0] + "\",con);");
            dbProcessSW.WriteLine("cmd.Parameters.AddWithValue(\"@" + kolonAdlari[0] + "\"," + classAttributeAdi + "." + kolonAdlari[0] + ");");
            dbProcessSW.WriteLine("cmd.ExecuteNonQuery();");
            dbProcessSW.WriteLine("con.Close();");
            dbProcessSW.WriteLine("cmd.Dispose();");
            dbProcessSW.WriteLine("}");
            #endregion
            #region Veri Okuma Bölümü
            dbProcessSW.WriteLine("public List<" + classAdi + "> VeriGetirme()");
            dbProcessSW.WriteLine("{");
            dbProcessSW.WriteLine("List<" + classAdi + "> result = new List<" + classAdi + ">();");
            dbProcessSW.WriteLine("con.Open();");
            dbProcessSW.WriteLine("cmd = new SqlCommand(\"SELECT * FROM " + tabloAdi + "\",con);");
            dbProcessSW.WriteLine("dr=cmd.ExecuteReader();");
            dbProcessSW.WriteLine("while(dr.Read())");
            dbProcessSW.WriteLine("{");
            dbProcessSW.WriteLine(classAdi + " " + classAttributeAdi + " = new " + classAdi + "()");
            dbProcessSW.WriteLine("{");
            for (int i=0;i<=kolonAdlari.Count-1;i++)
            {
                dbProcessSW.WriteLine(kolonAdlari[i] + " = dr[" + i + "],");
            }
            dbProcessSW.WriteLine("};");
            dbProcessSW.WriteLine("result.Add(" + classAttributeAdi + ");");
            dbProcessSW.WriteLine("}");
            dbProcessSW.WriteLine("con.Close();");
            dbProcessSW.WriteLine("dr.Close();");
            dbProcessSW.WriteLine("cmd.Dispose();");
            dbProcessSW.WriteLine("return result;");
            dbProcessSW.WriteLine("}");
            dbProcessSW.WriteLine("}");
            dbProcessSW.WriteLine("}");
            dbProcessSW.Flush();
            dbProcessSW.Close();
            dbProcessFS.Close();
            #endregion
            #endregion       
            #region AspxDosyasıOlustur
            aspxDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\" + classAdi + "WebForm.aspx";
            FileStream aspxFS = File.Create(@aspxDosyaYolu);
            aspxFS.Close();
            aspxFS = new FileStream(aspxDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter aspxSW = new StreamWriter(aspxFS);
            aspxSW.WriteLine("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeBehind=\"" + classAdi + "WebForm.aspx.cs\" Inherits=\"ClassCreator." + classAdi + "WebForm\" %>");
            aspxSW.WriteLine("<!DOCTYPE html>");            
            aspxSW.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            aspxSW.WriteLine("<head runat=\"server\">");
            aspxSW.WriteLine("<meta charset=\"utf - 8\">");
            aspxSW.WriteLine("<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\"/>");
            aspxSW.WriteLine("<title></title>");
            aspxSW.WriteLine("<style type=\"text / css\">");
            aspxSW.WriteLine(".auto-style1 { width:100%; } </style> </head> <body>");
            aspxSW.WriteLine("<link rel = \"stylesheet\" href = \"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css\" />");
            aspxSW.WriteLine("<link rel = \"stylesheet\" href = \"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css\" />");
            aspxSW.WriteLine("<script src = \"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js\" ></script>");
            aspxSW.WriteLine("<form id=\"form1\" runat=\"server\">");
            aspxSW.WriteLine("<asp:Repeater ID=\"rpt" + classAdi + "\" runat=\"server\">");
            aspxSW.WriteLine("<HeaderTemplate>");
            aspxSW.WriteLine("<table style=\"border:2px solid green\" border=\"1\">");
            aspxSW.WriteLine("<tr style=\"background-color:seagreen; color: white\">");
            for(int i=1;i<=kolonAdlari.Count-1;i++)
            {
                aspxSW.WriteLine("<td>"+kolonAdlari[i]+"</td>");
            }            
            aspxSW.WriteLine("</tr>");
            aspxSW.WriteLine("</HeaderTemplate>");
            aspxSW.WriteLine("<ItemTemplate>");
            aspxSW.WriteLine("<tr>");
            for(int i=1;i<=kolonAdlari.Count-1;i++)
            {
                aspxSW.WriteLine("<td><%#Eval(\""+kolonAdlari[i]+"\") %></td>");
            }            
            aspxSW.WriteLine("</tr>");
            aspxSW.WriteLine("</ItemTemplate>");
            aspxSW.WriteLine("<FooterTemplate>");
            aspxSW.WriteLine("</table>");
            aspxSW.WriteLine("</FooterTemplate>");
            aspxSW.WriteLine("</asp:Repeater>");
            aspxSW.WriteLine("</form>");
            aspxSW.WriteLine("</body>");
            aspxSW.WriteLine("</html>");
            aspxSW.Flush();
            aspxSW.Close();
            aspxFS.Close();            
            #endregion
            #region aspx.designer.cs
            aspxDesignerCsDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\" + classAdi + "WebForm.aspx.designer.cs";
            FileStream aspxDesignerCsFS = File.Create(@aspxDesignerCsDosyaYolu);
            aspxDesignerCsFS.Close();
            aspxDesignerCsFS = new FileStream(aspxDesignerCsDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter aspxDesignerCsSW = new StreamWriter(aspxDesignerCsFS);
            aspxDesignerCsSW.WriteLine("namespace ClassCreator {");
            aspxDesignerCsSW.WriteLine("public partial class " + classAdi + "WebForm {");
            aspxDesignerCsSW.WriteLine("protected global::System.Web.UI.HtmlControls.HtmlForm " + classAdi + "WebForm1;");
            aspxDesignerCsSW.WriteLine("protected global::System.Web.UI.WebControls.Repeater rpt" + classAdi + ";");
            aspxDesignerCsSW.WriteLine("}}");
            aspxDesignerCsSW.Flush();
            aspxDesignerCsSW.Close();
            aspxDesignerCsFS.Close();
            #endregion
            #region Aspx.csDosyasıOlusturma
            aspxCsDosyaYolu = HttpContext.Current.Request.PhysicalApplicationPath + "\\" + classAdi + "WebForm.aspx.cs";
            FileStream aspxCsFS = File.Create(@aspxCsDosyaYolu);
            aspxCsFS.Close();
            aspxCsFS = new FileStream(aspxCsDosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter aspxCsSW = new StreamWriter(aspxCsFS);
            aspxCsSW.WriteLine("using System;");
            aspxCsSW.WriteLine("using System.Collections.Generic;");
            aspxCsSW.WriteLine("using System.Linq;");
            aspxCsSW.WriteLine("using System.Web;");
            aspxCsSW.WriteLine("using System.Web.UI;");
            aspxCsSW.WriteLine("using System.Web.UI.WebControls;");
            aspxCsSW.WriteLine("namespace ClassCreator");
            aspxCsSW.WriteLine("{");
            aspxCsSW.WriteLine("public partial class " + classAdi + "WebForm "+": System.Web.UI.Page");
            aspxCsSW.WriteLine("{");
            aspxCsSW.WriteLine("protected void Page_Load(object sender, EventArgs e)");
            aspxCsSW.WriteLine("{");
            aspxCsSW.WriteLine(" DbProcess db = new DbProcess();");
            aspxCsSW.WriteLine("List<" + classAdi + "> " + classAttributeAdi + " = new List<" + classAdi + ">();");
            aspxCsSW.WriteLine(classAttributeAdi + " = db.VeriGetirme();");
            aspxCsSW.WriteLine("rpt" + classAdi + ".DataSource = " + classAttributeAdi + ";");
            aspxCsSW.WriteLine("rpt" + classAdi + ".DataBind();");
            aspxCsSW.WriteLine("}");
            aspxCsSW.WriteLine("}");
            aspxCsSW.WriteLine("}");
            aspxCsSW.Flush();
            aspxCsSW.Close();
            aspxCsFS.Close();
            #endregion            
            return result;
        }
    }
}