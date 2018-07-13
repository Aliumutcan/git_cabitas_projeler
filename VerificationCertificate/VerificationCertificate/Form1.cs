using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using OpenPgpLib;
using System.Net;
using System.Collections.Specialized;
using System.Net.Http;

namespace VerificationCertificate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string target_file = "", secret_key = "";
        private Process mv_prcInstaller = new Process();
        
        private bool Verification()
        {
            WebClient webClient = new WebClient(); // kullanılacak Sınız
            webClient.DownloadFile("http://localhost:52444/pin/" + txt_pin_kod.Text, "d:\\sifrelidosya.gpg");

            if (File.Exists("d:\\sifrelidosya.gpg"))
            {
                OpenPgp.DecryptFile("D:\\sifrelidosya.gpg", "D:\\yenisertifika\\cozdum.txt", privetekey_path, txt_password.Text);
                string line = "";
                using (var s = new FileStream("D:\\yenisertifika\\cozdum.txt", FileMode.Open))
                {
                    StreamReader sr = new StreamReader(s);
                    line = sr.ReadLine();
                    if (line.Split(',')[0] != txt_pin_kod.Text)
                    {
                        MessageBox.Show("Kullanıcı doğrulanamadı.");
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı doğrulandı.");
                    }
                }
                File.Delete("d:\\sifrelidosya.gpg");
                File.Delete("D:\\yenisertifika\\cozdum.txt");



                string postUrl = "http://localhost:52444/giris";
                var md5 = MD5Sifrele(line);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52444");
                    var content = new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("data_hashing", md5),
                            new KeyValuePair<string, string>("pin_kod", txt_pin_kod.Text)
                        });
                    var result = client.PostAsync("/giris", content);
                    var resultContent = result.Result.Content.ReadAsStringAsync();
                }



                //   WebClient client = new WebClient();
                //   // Post işlemini yapmak isteğimiz url bilgisini giriyoruz.
                //   string postUrl = "http://localhost:52444/giris";
                //   var md5 = MD5Sifrele(line);

                //   client.UploadValues(postUrl, new NameValueCollection()
                //{
                //    { "data_hashing", md5 },
                //    {"pin_kod",txt_pin_kod.Text }
                //});


            }
            else
            {
                //MessageBox.Show("Doğrulama oluşmadı tekrar pin alarak deneyiniz.");
            }

            return true;
        }

        public string Key_read(object pubKey)
        {
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs.Serialize(sw, pubKey);
            //get the string from the stream
            return sw.ToString();
        }
        public RSAParameters Key_write(string pubKeyString)
        {
            //get a stream from the string
            var sr = new System.IO.StringReader(pubKeyString);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            return (RSAParameters)xs.Deserialize(sr);
        }

        static byte[] DecodeOpenSSLPublicKey(String instr)
        {
            const String pempubheader = "-----BEGIN PGP PUBLIC KEY BLOCK-----";
            const String pempubfooter = "\r\n=ibOb\r\n-----END PGP PUBLIC KEY BLOCK-----";
            String pemstr = instr.Trim();
            byte[] binkey;
            if (!pemstr.StartsWith(pempubheader) || !pemstr.EndsWith(pempubfooter))
                return null;
            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pempubheader, "");  //remove headers/footers, if present
            sb.Replace(pempubfooter, "");

            String pubstr = sb.ToString().Trim();   //get string after removing leading/trailing whitespace


            binkey = Convert.FromBase64String(pubstr);
            try
            {
                binkey = Convert.FromBase64String(pubstr);
            }
            catch (System.FormatException)
            {       //if can't b64 decode, data is not valid
                return null;
            }
            return binkey;

            //return pubstr;
        }

        private void btn_decrypt_file_Click(object sender, EventArgs e)
        {
            bool result = true;
            while (result)
            {
                try
                {
                    Verification();
                    result = false;
                    ProcessStartInfo sInfo = new ProcessStartInfo("http://localhost:52444/login");
                    Process.Start(sInfo);
                }
                catch (Exception)
                {
                    result = true;
                }
            }
        }
        string privetekey_path = "";
        private void btn_privatekey_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Klasor = new FolderBrowserDialog();
            
            Klasor.ShowDialog();
            privetekey_path = Klasor.SelectedPath;
        }

        public static string MD5Sifrele(string metin)
        {
            // MD5CryptoServiceProvider nesnenin yeni bir instance'sını oluşturalım.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //Girilen veriyi bir byte dizisine dönüştürelim ve hash hesaplamasını yapalım.
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);

            //byte'ları biriktirmek için yeni bir StringBuilder ve string oluşturalım.
            StringBuilder sb = new StringBuilder();


            //hash yapılmış her bir byte'ı dizi içinden alalım ve her birini hexadecimal string olarak formatlayalım.
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürelim.
            return sb.ToString();
        }

    }
}
