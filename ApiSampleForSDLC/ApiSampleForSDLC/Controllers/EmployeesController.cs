using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
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

    // Existing actions (GET, POST, PUT, DELETE) are assumed to be present here.
    // ------------------------------------------------------------
    // PATCH: api/Employees/{id}
    // ------------------------------------------------------------
    /// <summary>
    /// Partially updates an existing employee using a JSON Patch document.
    /// </summary>
    /// <param name="id">The identifier of the employee to update.</param>
    /// <param name="patchDoc">The JSON Patch document describing the changes.</param>
    /// <returns>NoContent if successful; appropriate error response otherwise.</returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchEmployee(int id, [FromBody] JsonPatchDocument<EmployeePatchDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest("Patch document cannot be null.");
        }

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        // Map the entity to a mutable DTO
        var employeeToPatch = new EmployeePatchDto
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Position = employee.Position,
            Salary = employee.Salary
        };

        // Apply the patch to the DTO
        patchDoc.ApplyTo(employeeToPatch, ModelState);

        // Validate the patched DTO
        TryValidateModel(employeeToPatch);
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        // Map the patched DTO back onto the entity
        employee.FirstName = employeeToPatch.FirstName;
        employee.LastName = employeeToPatch.LastName;
        employee.Email = employeeToPatch.Email;
        employee.Position = employeeToPatch.Position;
        employee.Salary = employeeToPatch.Salary;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    private bool EmployeeExists(int id) => _context.Employees.Any(e => e.Id == id);
}
