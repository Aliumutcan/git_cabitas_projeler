using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ENCRYPTİON_FİLES_WEB.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using OpenPgpLib;
using System.Security.Cryptography;
using System.Text;

namespace ENCRYPTİON_FİLES_WEB.Controllers
{
    public class HomeController : Controller
    {
        
        private OpenPgpLib.OpenPgp openPgp = new OpenPgpLib.OpenPgp();
        public IActionResult Index()
        {
            return View();
        }

        public string UploadFile(IFormFile file)
        {
            Guid guid = new Guid();
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/public_key/",
                        guid+".asc");
            

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }

            return path;
        }


        private string GetContentType(string path)
        {
            throw new NotImplementedException();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            byte[] pinkod;
            //HttpContext.Session.Set("name", Encoding.UTF8.GetBytes("Hosgeldiniz."));
            if (HttpContext.Session.Id!=null)
            {
                if (HttpContext.Session.TryGetValue("pin_kod", out pinkod))
                {
                    string kod = Encoding.UTF8.GetString(pinkod);
                    if (System.IO.File.Exists("wwwroot\\encrypt_file\\" + kod + ".txt"))
                        ViewBag.pin_kod = kod;
                    else
                    {
                        HttpContext.Session.TryGetValue("user", out pinkod);
                        HttpContext.Session.Set("name", pinkod);
                        HttpContext.Session.Remove("pin_kod");
                        HttpContext.Session.Remove("user");
                    }

                }
                if (HttpContext.Session.TryGetValue("name", out pinkod))
                {
                    ViewData["session"] = Encoding.UTF8.GetString(pinkod);
                }
                    
            }

            return View();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string name_and_surname)
        {

            string line = "";
            using (var stream = new FileStream("wwwroot\\into_users.txt", FileMode.OpenOrCreate))
            {
                StreamReader sr = new StreamReader(stream);

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split(',')[1] == name_and_surname)
                    {
                        break;
                    }

                    line = "";
                }
            }
            if (line == null || line == "")
            {
                return View();
            }
            string pin_kod = "";
            Random rn = new Random();
            pin_kod = rn.Next(1000, 9999).ToString();
            using (var strema = new FileStream("wwwroot\\encrypt_file\\" + pin_kod + ".txt", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(strema);
                sw.Write(pin_kod + "," + line.Split(',')[0]);
                sw.Close();

            }
            OpenPgp.EncryptFile("wwwroot\\encrypt_file\\" + pin_kod + ".txt", "wwwroot\\encrypt_file\\" + pin_kod + ".gpg", line.Split(",")[2], false, false);
            ViewBag.pin_kod = pin_kod;
            HttpContext.Session.Set("pin_kod", Encoding.UTF8.GetBytes(pin_kod));
            HttpContext.Session.Set("user", Encoding.UTF8.GetBytes(line.Split(',')[1]));
            return View();
        }
        [HttpGet]
        [Route("exist")]
        public IActionResult Exist()
        {
            HttpContext.Session.Remove("name");
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("pin/{pin_kod}")]
        public IActionResult Bring_Encryption_File(string pin_kod)
        {

            var path2 = "wwwroot\\encrypt_file\\" + pin_kod + ".gpg";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path2, FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/" + Path.GetExtension(path2), Path.GetFileName(path2));

        }

        [HttpGet]
        [Route("giris")]
        public IActionResult Enter()
        {
            return View();
        }
        [HttpPost]
        [Route("giris")]
        public IActionResult Enter(string data_hashing, string pin_kod)
        {

            int pinkod = 0;
            if (data_hashing.Length == 32 && pin_kod.Length == 4 && int.TryParse(pin_kod, out pinkod))
            {
                var path = "wwwroot\\encrypt_file\\" + pin_kod + ".txt";
                var s = new FileStream(path, FileMode.Open);


                StreamReader sr = new StreamReader(s);
                string readfile = sr.ReadLine();
                sr.Close();
                s.Close();
                if (MD5Sifrele(readfile) == data_hashing)
                {
                    if (System.IO.File.Exists("wwwroot\\encrypt_file\\" + pin_kod + ".txt"))
                    {
                        System.IO.File.Delete("wwwroot\\encrypt_file\\" + pin_kod + ".txt");
                        System.IO.File.Delete("wwwroot\\encrypt_file\\" + pin_kod + ".gpg");
                    }

                    HttpContext.Session.Set("name", Encoding.UTF8.GetBytes("Hosgeldiniz."));
                    Response.Cookies.Append("name", "Ali Umutcan KUL");
                }
                else
                    ViewData["message"] = "gelen veri uyusmadi";


            }
            else
                ViewData["message"] = "gelen veri yanlıs";
            return View();
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create_Regisration()
        {
            return View();
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create_Regisration(string name, IFormFile publickey)
        {
            string publickey_path= UploadFile(publickey);
            FileStream fs = new FileStream("wwwroot/into_users.txt",FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            Guid g = new Guid();
            sw.WriteLine(g.ToString()+","+name+","+publickey_path);
            sw.Close();
            fs.Close();
            return RedirectToAction("Login");
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
