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
