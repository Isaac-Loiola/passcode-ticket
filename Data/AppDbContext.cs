using Microsoft.EntityFrameworkCore;
using passcode_ticket.Models;
namespace ProjetoDBZ.Data {
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions options) : base(options) {}
        public DbSet<Ticket> Tickets {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<TicketHistory> TicketHistory {get;set;}
    }
}
