using AutoMapper;
using LabApi.DTO;
using LabApi.Models;
using LabApi.Repo;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IGenericRepo<Student> _repo;
        private readonly IMapper _mapper;
        public StudentController(IGenericRepo<Student> studentRepo, IMapper mapper)
        {
            _repo = studentRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var students = await _repo.GetAllAsync();
            if (students == null)
            {
                return NotFound();
            }

            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _repo.GetByIdWithIncludesAsync(id, s => s.Department);
            if (student == null)
            {
                return NotFound();
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = $"Student with id {id} is found", Student = studentDTO });
        }

        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> Get(string name)
        {
            var students = await _repo.FindAsync(s => s.Name == name);
            var student = students.FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = $"Student with name {name} is found", Student = studentDTO });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Student student)
        {
            await _repo.AddAsync(student);
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return CreatedAtAction(nameof(Get), new { id = student.Id }, new { message = "created", Student = studentDTO });
        }

        [HttpPut]
        public async Task<IActionResult> Update(Student student)
        {
            await _repo.UpdateAsync(student);
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = "updated", Student = studentDTO });
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Student> patchDoc)
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

            var student = await _repo.GetByIdAsync(id);
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = "patched", Student = studentDTO });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            await _repo.DeleteAsync(student);
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = "deleted", Student = studentDTO });
        }
    }
}
