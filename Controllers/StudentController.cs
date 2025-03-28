using AutoMapper;
using LabApi.DTO;
using LabApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ITIDbContext _db;
        private readonly IMapper _mapper;
        public StudentController(ITIDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var students = _db.Students.Include(s => s.Department).ToList();
            if (students == null )
            {
                return NotFound();
            }

            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOs);
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var student = _db.Students.Include(s=>s.Department).FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            StudentDTO studentDTO = new StudentDTO { };
            studentDTO.name = student.Name;
            studentDTO.address = student.Address;
            studentDTO.deptName = student.Department!.Name;
            studentDTO.skill = "problem solving";
            return Ok(new { message = $"Student with id {id} is found", Student = studentDTO });
        }
        [HttpGet("{name:alpha}")]
        public IActionResult Get(string name)
        {
            var student = _db.Students.FirstOrDefault(s => s.Name == name);
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = $"Student with name {name} is found", Student = studentDTO });
        }
        [HttpPost]
        public IActionResult Add(Student student)
        {
            _db.Students.Add(student);
            _db.SaveChanges();
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return CreatedAtAction(nameof(Get), new { id = student.Id }, new { message = "created", Strudent = studentDTO });
        }
        [HttpPut]
        public IActionResult Update(Student student)
        {
            _db.Students.Update(student);
            _db.SaveChanges();
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = "updated", Student = studentDTO });
        }
        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<Student> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(student, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.SaveChanges();
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = "patched", Student = studentDTO });
        }



        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            _db.Students.Remove(student);
            _db.SaveChanges();
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(new { message = "deleted", Student = studentDTO });
        }
    }
}
