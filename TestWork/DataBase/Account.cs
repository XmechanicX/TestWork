using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWork.DataBase
{
    public enum StatusAccount
    {
        Active,
        Blocked
    }

    public class Account
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(16)]
        [Required]
        public string CardNumber { get; set; }
        [MaxLength(4)]
        [Required]
        public int PinCode { get; set; }
        [MaxLength(3)]
        [Required]
        public int CVV { get; set; }
        [Required]
        public DateTime Year { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public StatusAccount StatusAccounts { get; set; }
    }
}
