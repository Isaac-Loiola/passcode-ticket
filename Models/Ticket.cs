namespace passcode_ticket.Models
{
    public class Ticket
    {
        public enum EnumStatus 
        {
            waiting,
            called,
            finished
        }

        public int Id {get;set;}
        public string Code {get;set;}
        public string Type {get;set;}
        public EnumStatus Status {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime FinishedAt {get;set;}
    }
}