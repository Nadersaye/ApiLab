using AutoMapper;
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
        private readonly IMapper _mapper;
        public DepartmentController(ITIDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var departments = _db.Departments.Where(d=>!d.IsDeleted).Include(d => d.Students ).ToList();

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
            var department = _db.Departments.Include(d=>d.Students).FirstOrDefault(d => d.Id == id && !d.IsDeleted);

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
            var department = _db.Departments.FirstOrDefault(d => d.Name == name && !d.IsDeleted);
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
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return CreatedAtAction(nameof(Get), new { id = department.Id }, new { message = "created", Department = departmentDTO });
        }
        [HttpPut]
        public IActionResult Update(Department department)
        {
            _db.Departments.Update(department);
            _db.SaveChanges();
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(new { message = "updated", Department = department });
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var department = _db.Departments.FirstOrDefault(d => d.Id == id && !d.IsDeleted);
            if (department == null)
            {
                return NotFound();
            }
            department.IsDeleted = true;
            _db.SaveChanges();
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(new { message = "deleted", Department = departmentDTO });
        }
    }
}
