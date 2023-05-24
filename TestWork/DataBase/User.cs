using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWork.DataBase
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string FistName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(50)]
        [Required]
        public string SurName { get; set; }
    }
}
