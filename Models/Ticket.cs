using System.ComponentModel.DataAnnotations;

namespace passcode_ticket.Models
{
    public class Ticket
    {   
        public int Id {get;set;}
        public string? Code {get;set;}
        public string Type {get;set;}
        public string Status {get;set;} = "waiting";
        public string Sector {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime? FinishedAt {get;set;}
    }
}