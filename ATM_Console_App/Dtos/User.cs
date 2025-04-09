using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App.Dtos
{

    [Table("User_Tbl")]
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Wallet { get; set; }

        public string Islock { get; set; }

    }
}
