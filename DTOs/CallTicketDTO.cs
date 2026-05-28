using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;

namespace passcode_ticket.DTOs
{
    public class CallTicketDTO
    {
        [Required]
        public int IdUser {get;set;}
    }
}