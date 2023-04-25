using AuthGuard.API.Infrastructure;
using AuthGuard.API.Infrastructure.Data.Entities;
using AuthGuard.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;
        private readonly DbSet<Employee> _employee;
        public EmployeeController(EmployeeContext context)
        {
            _context = context;
            _employee = _context.Set<Employee>();
        }
        [HttpGet("{id}")]
        [Authorize("APolicy")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employee.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null)
                return NotFound(employee);

            return Ok(employee);
        }

        [HttpPost]
        [Authorize("BPolicy")]
        public async Task<IActionResult> Post(CreateEmployeeRequestModel model)
        {
            Employee employee = new()
            {
                Age = model.Age,
                Name = model.Name,
                Surname = model.Surname,
            };
            await _employee.AddAsync(employee);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return Ok(employee);

            return Problem();
        }

        [HttpPut]
        [Authorize("CPolicy")]
        public async Task<IActionResult> Update(Employee employee)
        {
            var result = await _employee.AsNoTracking().FirstOrDefaultAsync(x => x.Id == employee.Id);
            if (result is null)
                return NotFound(result);
            
            _employee.Update(employee);
            
            var state = await _context.SaveChangesAsync();
            if (state > 0)
                return Ok();

            return Problem();
        }

        [HttpDelete("{id}")]
        [Authorize("DPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employee.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null)
                return NotFound(employee);

            _employee.Remove(employee);

            var state = await _context.SaveChangesAsync();
            if (state > 0)
                return Ok();

            return Problem();
        }
    }
}
