using AutoMapper;
using LabApi.DTO;
using LabApi.Models;
using LabApi.Repo;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IGenericRepo<Department> _repo;
        private readonly IMapper _mapper;
        public DepartmentController(IGenericRepo<Department> departmentRepo, IMapper mapper)
        {
            _repo = departmentRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var departments = await _repo.GetAllWithIncludesAsync(d => d.Students);
            if (departments == null)
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
        public async Task<IActionResult> Get(int id)
        {
            var department = await _repo.GetByIdWithIncludesAsync(id, d => d.Students);
            if (department == null)
            {
                return NotFound();
            }
            DepartmentDTO departmentDTO = new DepartmentDTO
            {
                DeptName = department.Name,
                StudentSNames = department.Students.Select(s => s.Name).ToList(),
                ManagerName = department.ManagerName,
                Count = department.Students.Count,
                Message = "Department found"
            };

            return Ok(new { message = $"Department with id {id} is found", Department = departmentDTO });
        }

        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> Get(string name)
        {
            var departments = await _repo.FindAsync(d => d.Name == name && !d.IsDeleted);
            var department = departments.FirstOrDefault();
            if (department == null)
            {
                return NotFound();
            }
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(new { message = $"Department with name {name} is found", Department = departmentDTO });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {
            await _repo.AddAsync(department);
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return CreatedAtAction(nameof(Get), new { id = department.Id }, new { message = "created", Department = departmentDTO });
        }

        [HttpPut]
        public async Task<IActionResult> Update(Department department)
        {
            await _repo.UpdateAsync(department);
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(new { message = "updated", Department = departmentDTO });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repo.SoftDeleteAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            var department = await _repo.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(new { message = "deleted", Department = departmentDTO });
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Department> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            try
            {
                await _repo.PatchAsync(id, patchDoc);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            var department = await _repo.GetByIdAsync(id);
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return Ok(new { message = "patched", Department = departmentDTO });
        }
    }
}
