using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace passcode_ticket.Models
{
    public class TicketHistory
    {
        public int Id {get;set;}
        public int TicketId {get;set;}
        public Ticket ticket {get;set;}

        public int UserId {get;set;}
        public User User {get;set;}
        public DateTime CalledAt{get;set;} = DateTime.Now;
        public string Sector{get;set;}

        public TicketHistory(int user, int ticket)
        {
            this.TicketId = ticket;
            this.UserId = user;
        } 
    }

}