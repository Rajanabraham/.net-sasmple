using Microsoft.AspNetCore.Mvc;
using ApiSampleForSDLC.Models;

namespace ApiSampleForSDLC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    // In‑memory store – mimics a repository for demo / unit‑test purposes.
    private static readonly List<Employee> _employees = new()
    {
        new Employee { Id = 1, Name = "John Doe", Position = "Developer", Department = "Engineering", Salary = 85000 },
        new Employee { Id = 2, Name = "Jane Smith", Position = "Tester", Department = "Quality Assurance", Salary = 65000 }
    };

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
        if (employee == null)
            return BadRequest();

        employee.Id = _employees.Max(e => e.Id) + 1;
        _employees.Add(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    // PUT: api/employees/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Employee updated)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            return NotFound();

        // Replace the whole entity (except the Id)
        employee.Name = updated.Name;
        employee.Position = updated.Position;
        employee.Department = updated.Department;
        employee.Salary = updated.Salary;
        return NoContent();
    }

    // PATCH: api/employees/{id}
    // Allows partial update of an employee using EmployeePatchDto.
    // Only properties supplied (non‑null) are updated.
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] EmployeePatchDto patchDto)
    {
        if (patchDto == null)
            return BadRequest();

        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            return NotFound();

        // Apply supplied fields – keep original values for null fields.
        if (patchDto.Name != null)
            employee.Name = patchDto.Name;
        if (patchDto.Position != null)
            employee.Position = patchDto.Position;
        if (patchDto.Department != null)
            employee.Department = patchDto.Department;
        if (patchDto.Salary.HasValue)
            employee.Salary = patchDto.Salary.Value;

        return NoContent();
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
}
