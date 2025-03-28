using LabApi.DTO;
using LabApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var departments = _db.Departments.Include(d => d.Students).ToList();

            if (departments == null || !departments.Any())
            {
                return NotFound();
            }

            var departmentDTOs = departments.Select(department => new DepartmentDTO
            {
                DeptName = department.Name,
                StudentSNames = department.Students.Select(s => s.Name).ToList(),
                ManagerName = department.ManagerName,
                Count = department.Students.Count,
                Message = "Department found"
            }).ToList();

            return Ok(departmentDTOs);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var department = _db.Departments.Include(d=>d.Students).FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }
            DepartmentDTO departmentDTO = new DepartmentDTO { };
            departmentDTO.DeptName = department.Name;
            departmentDTO.StudentSNames = department.Students.Select(s => s.Name).ToList();
            departmentDTO.ManagerName = department.ManagerName;
            departmentDTO.Count = department.Students.Count;
            departmentDTO.Message = "Department found";

            return Ok(new { message = $"Department with id {id} is found", Department = departmentDTO });
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
