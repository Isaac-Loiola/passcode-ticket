using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace passcode_ticket.Models
{
    public class User
    {
        public int Id {get;set;}

        [Required]
        [MaxLength(100)]
        public string Name {get;set;}
        
        [Required]
        public string Email {get;set;}

        [Required]
        public string Password {get;set;}

        [Required]
        public string Sector {get;set;}

    }
}