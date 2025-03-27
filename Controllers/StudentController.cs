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
        ITIDbContext _db;
        public StudentController(ITIDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var students = _db.Students.ToList();
            if (students is null)
            {
                return NotFound();
            }
            return Ok(students);
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var student = _db.Students.Include(s=>s.Department).FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(new { message = $"Student with id {id} is found", Student = student });
        }
        [HttpGet("{name:alpha}")]
        public IActionResult Get(string name)
        {
            var student = _db.Students.FirstOrDefault(s => s.Name == name);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(new { message = $"Student with name {name} is found", Student = student });
        }
        [HttpPost]
        public IActionResult Add(Student student)
        {
            _db.Students.Add(student);
            _db.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = student.Id }, new { message = "created", Strudent = student });
        }
        [HttpPut]
        public IActionResult Update(Student student)
        {
            _db.Students.Update(student);
            _db.SaveChanges();
            return Ok(new { message = "updated", Student = student });
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
            return Ok(new { message = "patched", Student = student });
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
            return Ok(new { message = "deleted", Student = student });
        }
    }
}
