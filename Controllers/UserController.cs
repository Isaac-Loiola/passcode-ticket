using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using passcode_ticket.DTOs;
using passcode_ticket.Models;
using passcode_ticket.Services;
using ProjetoDBZ.Data;

namespace passcode_ticket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public UserController(AppDbContext appDbContext, TokenService tokenService)
        {
           _context = appDbContext;
           _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> create(User user)
        {
            // Encrypting the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("user created successfully!");
        }

        
        public async Task <ActionResult<User>> Auth([FromBody] AuthDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if(user == null)
                return Unauthorized("Email or password invalids");

            bool correctPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!correctPassword)
                return Unauthorized("Email or password invalids");                

            var token = _tokenService.GenerateToken(user);

            return Ok(new {token});
        }
    }
}