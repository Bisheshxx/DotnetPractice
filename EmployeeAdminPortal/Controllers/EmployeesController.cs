using Microsoft.AspNetCore.Mvc;
using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;

namespace EmployeeAdminPortal.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public EmployeesController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        var allEmployees = dbContext.Employees.ToList();
        return Ok(allEmployees);
    }
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetEmployeeByID(Guid id)
    {
        var Employee = dbContext.Employees.Find(id);
        if (Employee == null)
        {
            return NotFound();
        }
        return Ok(Employee);
    }

    [HttpPost]
    public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
    {
        if (addEmployeeDto == null)
        {
            return BadRequest("Employee data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var EmployeeEntity = new Employee()
        {
            Name = addEmployeeDto.Name,
            Email = addEmployeeDto.Email,
            Phone = addEmployeeDto.Phone,
            Salary = addEmployeeDto.Salary,
        };

        try
        {
            dbContext.Employees.Add(EmployeeEntity);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while saving the employee.");
        }

        return CreatedAtAction(nameof(GetEmployeeByID), new { id = EmployeeEntity.Id }, EmployeeEntity);
    }
    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult updateEmployee(Guid id, updateEmployeeDto updateEmployeeDto)
    {
        var Employee = dbContext.Employees.Find(id);
        if (Employee is null)
        {
            return NotFound();

        }
        Employee.Name = updateEmployeeDto.Name;
        Employee.Email = updateEmployeeDto.Email;
        Employee.Phone = updateEmployeeDto.Phone;
        Employee.Salary = updateEmployeeDto.Salary;

        dbContext.SaveChanges();

        return Ok(Employee);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult deleteEmployee(Guid id)
    {
        var Employee = dbContext.Employees.Find(id);
        if (Employee is null)
        {
            return NotFound();
        }
        dbContext.Employees.Remove(Employee);
        dbContext.SaveChanges();
        return Ok();
    }
}
