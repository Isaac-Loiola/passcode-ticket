using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using passcode_ticket.Models;
using ProjetoDBZ.Data;

namespace passcode_ticket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext appDbContext)
        {
           _context = appDbContext;
        }

        public async Task<ActionResult<User>> create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("user created successfully!")




















            
    }
}