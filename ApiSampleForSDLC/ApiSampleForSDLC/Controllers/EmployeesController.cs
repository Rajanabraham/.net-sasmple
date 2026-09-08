using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using ApiSampleForSDLC.Models;

namespace ApiSampleForSDLC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeDbContext _context;

    public EmployeesController(EmployeeDbContext context)
    {
        _context = context;
    }

    // GET: api/Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return employee;
    }

    // POST: api/Employees
    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    // PUT: api/Employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }
        _context.Entry(employee).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
                return NotFound();
            else
                throw;
        }
        return NoContent();
    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PATCH: api/Employees/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchEmployee(int id, [FromBody] JsonPatchDocument<EmployeePatchDto> patchDoc)
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

        // Map existing entity to a mutable DTO
        var employeeToPatch = new EmployeePatchDto
        {
            Name = employee.Name,
            Age = employee.Age,
            Position = employee.Position
        };

        // Apply the patch to the DTO
        patchDoc.ApplyTo(employeeToPatch, ModelState);

        // Validate the patched DTO
        if (!TryValidateModel(employeeToPatch))
        {
            return ValidationProblem(ModelState);
        }

        // Copy patched values back to the entity
        employee.Name = employeeToPatch.Name;
        employee.Age = employeeToPatch.Age;
        employee.Position = employeeToPatch.Position;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    private bool EmployeeExists(int id) => _context.Employees.Any(e => e.Id == id);
}
