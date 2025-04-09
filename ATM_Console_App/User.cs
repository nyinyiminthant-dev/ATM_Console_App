using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App
{
    public class User
    {     
        public string UserID { get; set; }
        public string Password { get; set; }
        public decimal Wallet { get; set; }
       

        public User(string userId, string password, decimal wallet)
        {
            UserID = userId;
            Password = password;
            Wallet = wallet;
        }
    }
}
