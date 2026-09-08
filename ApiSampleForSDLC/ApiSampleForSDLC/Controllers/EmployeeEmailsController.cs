using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSampleForSDLC.Models;

namespace ApiSampleForSDLC.Controllers;

[ApiController]
[Route("api/employees/{employeeId}/emails")] 
public class EmployeeEmailsController : ControllerBase
{
    private readonly EmployeeContext _context;

    public EmployeeEmailsController(EmployeeContext context)
    {
        _context = context;
    }

    // GET: api/employees/{employeeId}/emails
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Email>>> GetEmails(int employeeId)
    {
        var employee = await _context.Employees
            .Include(e => e.Emails)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee == null)
            return NotFound($"Employee with id {employeeId} not found.");

        return Ok(employee.Emails);
    }

    // POST: api/employees/{employeeId}/emails
    [HttpPost]
    public async Task<ActionResult<Email>> AddEmail(int employeeId, [FromBody] EmailDto emailDto)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
            return NotFound($"Employee with id {employeeId} not found.");

        var email = new Email
        {
            EmailAddress = emailDto.EmailAddress,
            IsPrimary = emailDto.IsPrimary,
            EmployeeId = employeeId
        };

        // If the new email is marked primary, demote existing primary emails
        if (email.IsPrimary)
        {
            var currentPrimary = await _context.Emails
                .Where(e => e.EmployeeId == employeeId && e.IsPrimary)
                .ToListAsync();
            foreach (var p in currentPrimary)
                p.IsPrimary = false;
        }

        _context.Emails.Add(email);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmails), new { employeeId = employeeId }, email);
    }
}
