using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    [Table("tblUser",Schema ="Auth")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }=string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; }=string.Empty;
        public string? Address { get; set; } 
        public string ContactNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate  { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
