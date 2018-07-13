using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;

namespace AUTHENTİCATİON
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            int pin_kod = 0;
            if (txt_pin_kod.Text.Length==4 && int.TryParse(txt_pin_kod.Text, out pin_kod))
            {


                WebClient webClient = new WebClient(); // kullanılacak Sınız
                webClient.DownloadFile("http://localhost:52444/"+pin_kod, "d:\\sifrelidosya.sifreli");










                //HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
                //htmldoc.LoadHtml("http://www.yazilimdilleri.net/YazilimMakale-3138-C--Web-Sitesinden-Veri-Cekmek--HmtlAgilityPack-Kullanimi--ile-HTML-Parse-Etme.aspx");

                //İndirilen Html kodları, yukarıda oluşturulan htmlagilitypack'den türetilen htmldocument nesnesine aktarılıyor...

                //HtmlNodeCollection basliklar = htmldoc.DocumentNode.SelectNodes("//a");
                //İstediğimiz Element'in özelliğini yani filtrelemeyi yapacağımız alan...

                //List<string> liste = new List<string>();
                //Gelen veriyi saklayacağımız alan String tipinden oluşturuluyor.

                //foreach (var baslik in basliklar)
                //{
                //    liste.Add(baslik.InnerText);
                //    Parse ettiğimiz linklerin üzerinde yazan yazılar dizi halinde listeye ekleniyor...
                //}

                //for (int i = 0; i < liste.Count; i++)
                //{
                //    Console.WriteLine(liste[i]);
                //    Basit bir döngü ile aldığımız veriler ekrana yazılıyor...
                //}

                //Console.ReadLine();
                //Ekran beklemede :)

            }
        }
    }
}
