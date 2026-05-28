using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using passcode_ticket.DTOs;
using passcode_ticket.Hubs;
using passcode_ticket.Migrations;
using passcode_ticket.Models;
using ProjetoDBZ.Data;

namespace passcode_ticket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<PanelHub> _hubContext;
        public TicketController(AppDbContext appDbContext, IHubContext<PanelHub> hubContext)
        {
            _context = appDbContext;
            _hubContext = hubContext;
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

        public async Task createHistory(int user, int ticket)
        {
            TicketHistory history = new TicketHistory(user, ticket);
            _context.TicketHistory.Add(history);

            await _context.SaveChangesAsync();
        }

        [HttpPost("call")]
        public  async Task<ActionResult<Ticket>> LastTicket([FromBody] CallTicketDTO dto)
        {
            var emergency = await _context.Tickets
                .Where(t => t.Type == "Emergency" && t.Status == "waiting")
                .OrderBy(t => t.CreatedAt)
                .FirstOrDefaultAsync();

            if(emergency != null)
            {
                emergency.Status = "called";
                await _context.SaveChangesAsync();
                return Ok(emergency);
            }

            var lastCalls = await _context.Tickets
                .Where(t => t.Status == "called")
                .OrderByDescending(t => t.CreatedAt)
                .Take(2)
                .ToListAsync();

            int followedPreferential = 0;
            foreach(var t in lastCalls)
            {
                if(t.Type == "preferential")
                    followedPreferential++;
                else
                    break;
            }

            Ticket next = null;

            if(followedPreferential >= 2)
            {
                next = await _context.Tickets
                    .Where(t => t.Type == "Normal" && t.Status == "waiting")
                    .OrderBy(t => t.CreatedAt)
                    .FirstOrDefaultAsync();
            }

            if(next == null)
            {
                next = await _context.Tickets
                    .Where(t => t.Type == "Preferential" && t.Status == "waiting")
                    .OrderBy(t => t.CreatedAt)
                    .FirstOrDefaultAsync();
            }
            
            if(next == null)
            {
                next = await _context.Tickets
                    .Where(t => t.Type == "Normal" && t.Status == "waiting")
                    .OrderBy(t => t.CreatedAt)
                    .FirstOrDefaultAsync();
            }

            if(next == null)
            {
                return NotFound("None passcode waiting");
            }

            next.Status = "Called";
            await _context.SaveChangesAsync();

            await createHistory(dto.IdUser, next.Id);

            await notifyPanel();
            return Ok(next);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> finishTicket(int id)
        {
            var foundTicket = await _context.Tickets.FindAsync(id);
            
            if(foundTicket == null)
                return NotFound("Ticket not found");

            foundTicket.Status = "finished";
            foundTicket.FinishedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok($"Ticket with Id {id} finished successfully");
        }

        private async Task notifyPanel()
        {
            var lastCalls = await _context.Tickets
                .Where(t => t.Status == "called")
                .OrderByDescending(t => t.CreatedAt)
                .Take(4)
                .ToListAsync();

            await _hubContext.Clients.All.SendAsync("lastCalls", lastCalls);
        }
    }
}