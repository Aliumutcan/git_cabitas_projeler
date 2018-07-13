using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENCRYPTİON_FİLES_WEB.Models
{
    public class Users
    {
        public string id;
        public string name_and_surname;
        public PgpPublicKey public_key;

        public Users(string id,string name_and_surname, PgpPublicKey public_key)
        {
            this.id = id;
            this.name_and_surname = name_and_surname;
            this.public_key = public_key;
        }
    }
}
