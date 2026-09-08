using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using ApiSampleForSDLC.Models;

namespace ApiSampleForSDLC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeContext _context;

    public EmployeesController(EmployeeContext context)
    {
        _context = context;
    }

    // Existing actions (Get, Post, etc.) remain unchanged ...

    // PATCH: api/Employees/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchEmployee(int id, JsonPatchDocument<EmployeePatchDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        var employeeToPatch = new EmployeePatchDto
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Salary = employee.Salary
        };

        patchDoc.ApplyTo(employeeToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map patched fields back to the entity
        employee.FirstName = employeeToPatch.FirstName;
        employee.LastName = employeeToPatch.LastName;
        employee.Email = employeeToPatch.Email;
        employee.Salary = employeeToPatch.Salary;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
