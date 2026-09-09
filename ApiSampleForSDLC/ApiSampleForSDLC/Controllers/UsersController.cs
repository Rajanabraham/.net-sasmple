using ApiSampleForSDLC.Models;
using ApiSampleForSDLC.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiSampleForSDLC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // In‑memory store – replace with a proper EF Core DbContext in production.
        private static readonly List<User> _users = new();

        // Mapping helpers
        private static UserDto ToDto(User u) => new()
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role
        };

        private static User FromDto(UserDto dto) => new()
        {
            Id = dto.Id,
            Username = dto.Username,
            Email = dto.Email,
            Role = dto.Role
        };

        // GET api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _users.Select(ToDto);
            return Ok(result);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return Ok(ToDto(user));
        }

        // POST api/users
        [HttpPost]
        public IActionResult Create([FromBody] UserDto dto)
        {
            if (dto == null)
                return BadRequest();

            // Simple Id generation – in real DB this would be identity/sequence.
            var nextId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            var user = FromDto(dto);
            user.Id = nextId;
            _users.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, ToDto(user));
        }

        // PUT api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest();

            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
                return NotFound();

            existing.Username = dto.Username;
            existing.Email = dto.Email;
            existing.Role = dto.Role;
            return NoContent();
        }

        // DELETE api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            _users.Remove(user);
            return NoContent();
        }
    }
}
