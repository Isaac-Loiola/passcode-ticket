using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjetoDBZ.Data;

namespace passcode_ticket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public TicketController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // [HttpPost]
        // public 
    }
}