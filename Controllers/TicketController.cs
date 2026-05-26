using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using passcode_ticket.Models;
using ProjetoDBZ.Data;

namespace passcode_ticket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TicketController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Create(Ticket ticket)
        {
            int lastNumber = 0;

            var lastTicket = await _context.Tickets.OrderByDescending(t => t.Id).FirstOrDefaultAsync(); 

            if(lastTicket != null)
            {
                lastNumber = lastTicket.CreatedAt.Date == DateTime.Today? Convert.ToInt32(lastTicket.Code.Substring(1, 3)) : 0;
            }

            string code = $"{ticket.Type.Substring(0, 1).ToUpper()}{lastNumber + 1 :D3}";
            ticket.Code = code;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}