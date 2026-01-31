using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserContext _usercontext;

        public UserController(UserContext context)
        {
            _usercontext = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _usercontext.User.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(long id)
        {
            var user = await _usercontext.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserDTO>> PostUser(CreateUserDTO createUserDTO)
        {
            var user = new Users
            {
                First_Name = createUserDTO.First_Name,
                Last_Name = createUserDTO.Last_Name,
                Age = createUserDTO.Age,
                Sex = createUserDTO.Sex,
                Occupation = createUserDTO.Occupation,
            };
            _usercontext.User.Add(user);
            await _usercontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);

        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutUser(long Id, Users users)
        {
            if (Id != users.Id)
            {
                return BadRequest();
            }
            _usercontext.Entry(users).State = EntityState.Modified;

            try
            {
                await _usercontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItemExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            var user = await _usercontext.User.FindAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            _usercontext.User.Remove(user);
            await _usercontext.SaveChangesAsync();
            return NoContent();

        }

        private bool UserItemExists(long Id)
        {
            return _usercontext.User.Any(e => e.Id == Id);
        }
    }
}