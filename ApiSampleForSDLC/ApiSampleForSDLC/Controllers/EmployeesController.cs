using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using ApiSampleForSDLC.Models;

namespace ApiSampleForSDLC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    // In‑memory store for demo purposes. In a real project this would be replaced by a DbContext.
    private static readonly List<Employee> _employees = new();
    private static int _nextId = 1;

    // GET: api/employees
    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        return Ok(_employees);
    }

    // GET: api/employees/{id}
    [HttpGet("{id}")]
    public ActionResult<Employee> Get(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            return NotFound();
        return Ok(employee);
    }

    // POST: api/employees
    [HttpPost]
    public ActionResult<Employee> Create([FromBody] Employee employee)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        employee.Id = _nextId++;
        _employees.Add(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    // DELETE: api/employees/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            return NotFound();

        _employees.Remove(employee);
        return NoContent();
    }

    // PATCH: api/employees/{id}
    // Supports JSON Patch (RFC 6902) for partial updates.
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] JsonPatchDocument<EmployeePatchDto> patchDoc)
    {
        if (patchDoc == null)
            return BadRequest("Patch document cannot be null.");

        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            return NotFound();

        // Map the current entity to a mutable DTO.
        var employeeDto = new EmployeePatchDto
        {
            Name = employee.Name,
            Age = employee.Age,
            Salary = employee.Salary
        };

        // Apply the patch operations to the DTO.
        patchDoc.ApplyTo(employeeDto, ModelState);

        // Validate the patched DTO.
        if (!TryValidateModel(employeeDto))
            return ValidationProblem(ModelState);

        // Persist changes back to the entity.
        employee.Name = employeeDto.Name;
        employee.Age = employeeDto.Age;
        employee.Salary = employeeDto.Salary;

        return NoContent();
    }
}
