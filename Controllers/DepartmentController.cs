using LabApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        ITIDbContext _db;
        public DepartmentController(ITIDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var departments = _db.Departments.ToList();
            if (departments is null)
            {
                return NotFound();
            }
            return Ok(departments);
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var department = _db.Departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(new { message = $"Department with id {id} is found", Department = department });
        }
        [HttpGet("{name:alpha}")]
        public IActionResult Get(string name)
        {
            var department = _db.Departments.FirstOrDefault(d => d.Name == name);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(new { message = $"Department with name {name} is found", Department = department });
        }
        [HttpPost]
        public IActionResult Add(Department department)
        {
            _db.Departments.Add(department);
            _db.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = department.Id }, new { message = "created", Department = department });
        }
        [HttpPut]
        public IActionResult Update(Department department)
        {
            _db.Departments.Update(department);
            _db.SaveChanges();
            return Ok(new { message = "updated", Department = department });
        }
        [HttpDelete]
        public IActionResult Delete(Department department)
        {
            _db.Departments.Remove(department);
            _db.SaveChanges();
            return Ok(new { message = "deleted", Department = department });
        }
    }
}
