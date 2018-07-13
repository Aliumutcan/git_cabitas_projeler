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
using System.Runtime.Serialization;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Xml.Serialization;
using Microsoft.Extensions.FileProviders;

namespace ENCRYPTİON_FİLES_WEB.Controllers
{
    public class HomeController : Controller
    {

        private Users users;
        private OpenPgpLib.OpenPgp openPgp = new OpenPgpLib.OpenPgp();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile publickey,string name_and_surname)
        {

            using (var stream = new FileStream("wwwroot/into_users.txt", FileMode.OpenOrCreate))
            {
                
                StreamWriter sw = new StreamWriter(stream);
                Guid guid = Guid.NewGuid();
                string path = UploadFile(publickey);
                sw.WriteLine(guid.ToString()+","+name_and_surname+","+ path);
                sw.Close();
            }

            /*
            string path= UploadFile(publickey);
            Guid guid = Guid.NewGuid();
            users = new Users(guid.ToString(),name_and_surname, openPgp.PublicPrivateKey(path));
            */
            return View();
        }

        [HttpPost]
        [Route("encryptFile")]
        public async Task<IActionResult> EncryptFile(IFormFile publickey,string user)
        {
            string line = "";
            using (var stream = new FileStream("wwwroot\\into_users.txt", FileMode.OpenOrCreate))
            {
                StreamReader sr = new StreamReader(stream);
               
                while ((line=sr.ReadLine())!=null)
                {
                    if (line.IndexOf(user)>0)
                    {
                        break;
                    }
                }
            }
            string path = UploadFile(publickey);
            string path3 = line.Split(',')[2];
            OpenPgp.EncryptFile(path, "wwwroot\\" + publickey.Name+".pgp", path3, false, false);

            var path2 = "wwwroot\\"+ publickey.Name + ".pgp";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path2, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/" + Path.GetFileName(path2) + ".pgp", Path.GetFileName(path2));
        }
        
        public string UploadFile(IFormFile file)
        {
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot.",
                        file.FileName);

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

        [Route("login")]
        public IActionResult Login()
        {
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
                    if (line.Split(',')[1]==name_and_surname)
                    {
                        break;
                    }
                }
            }
            string pin_kod = "";
            using (var strema=new FileStream("wwwroot\\encrypt_file\\"+line.Split(',')[1]+".txt",FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(strema);
                Random rn = new Random();
                pin_kod = rn.Next(1000, 9999).ToString();
                sw.Write(pin_kod);
                sw.Close();

            }
            ViewBag.pin_kod = pin_kod;
            return View();
        }

        [HttpGet]
        [Route("{pin_kod}")]
        public IActionResult Bring_Encryption_File(string pin_kod)
        {
            return View();
        }
    }
}
