using ApiSampleForSDLC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiSampleForSDLC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private static readonly List<Employee> employees = new()
    {
        new Employee
        {
            Id = 1,
            Name = "Rajan",
            Department = ".NET",
            Salary = 50000
        }
    };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employee = employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            employee.Id = employees.Count == 0
                ? 1
                : employees.Max(x => x.Id) + 1;

            employees.Add(employee);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                employee);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Employee employee)
        {
            var existing = employees.FirstOrDefault(x => x.Id == id);

            if (existing == null)
                return NotFound();

            existing.Name = employee.Name;
            existing.Department = employee.Department;
            existing.Salary = employee.Salary;

            return Ok(existing);
        }

        
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] EmployeePatchDto patchDto)
        {
            if (patchDto == null)
                return BadRequest(new { message = "Patch payload cannot be null." });

            var existing = employees.FirstOrDefault(x => x.Id == id);

            if (existing == null)
                return NotFound(new { message = $"Employee with ID {id} not found." });

            if (!string.IsNullOrWhiteSpace(patchDto.Name))
                existing.Name = patchDto.Name;

            if (!string.IsNullOrWhiteSpace(patchDto.Department))
                existing.Department = patchDto.Department;

            if (patchDto.Salary.HasValue)
                existing.Salary = patchDto.Salary.Value;

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var employee = employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
                return NotFound();

            employees.Remove(employee);

            return NoContent();
        }
    }
}
