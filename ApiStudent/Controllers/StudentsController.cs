using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiStudent.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly studentContext _context;

        public StudentsController(studentContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Wyświetlnie Wszytkiech z listy Studentów
        /// </summary>
        /// <returns></returns>
        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentAll>>> GetStudents()
        {
            return await _context.StudentsAll.ToListAsync();
        }

        /// <summary>
        ///  Wyszukanie Studenta
        /// </summary>
        /// <param name="id"></param>
        /// <returns>wyszukany Student</returns>
        /// <remarks>
        /// Wyszukanie Stdenta
        /// 
        ///     GET /Student
        ///     {
        ///         "id":5
        ///     }  
        /// </remarks>
        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /// <summary>
        /// Edytowanie Studentów
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        /// <remarks>
        /// Przykładowae Edytowanie
        ///     
        ///     PUT /Student
        ///     {
        ///         "id":1,
        ///         "name":"kuba",
        ///         "surname":"Nowacki"
        ///     }
        /// </remarks>
        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Wprowadź studenta
        /// </summary>
        /// <param name="student"></param>
        /// <returns>A newly created Student</returns>
        /// <remarks>
        /// Przykładowae Edytowanie
        ///     
        ///     PUT /Student
        ///     {
        ///         "id":1,
        ///         "name":"kuba",
        ///         "surname":"Nowacki"
        ///     }
        /// </remarks>
        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "Bearer")]

        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        /// <summary>
        /// Usuwanie Studentów z lsty
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Usunieto Studenta</returns>
        /// <remarks>
        /// Usuwnaie Stdentów
        /// 
        ///     Delete /Student
        ///     {
        ///         "id":2
        ///     }
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
        private static StudentAll StudentAll(Student student) =>
            new StudentAll
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname
            };
    }
}
