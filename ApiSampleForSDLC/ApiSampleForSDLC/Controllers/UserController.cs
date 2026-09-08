using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSampleForSDLC.Models;

namespace ApiSampleForSDLC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly EmployeeContext _context;

    public UserController(EmployeeContext context)
    {
        _context = context;
    }

    // GET: api/user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users
            .Select(u => new UserDto { Id = u.Id, Username = u.Username, Email = u.Email })
            .ToListAsync();
        return Ok(users);
    }

    // GET: api/user/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        return Ok(new UserDto { Id = user.Id, Username = user.Username, Email = user.Email });
    }

    // POST: api/user
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createDto)
    {
        var user = new User
        {
            Username = createDto.Username,
            Email = createDto.Email,
            // Additional fields can be mapped here
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
    }

    // PUT: api/user/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("ID mismatch between route and payload.");

        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        user.Username = updateDto.Username;
        user.Email = updateDto.Email;
        // Map other updatable fields here

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/user/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
