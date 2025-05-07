using ASPCoreWebAPICRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPCoreWebAPICRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase //Parentclass,handle request and generate response of studentAPIController
    {
        private readonly CodeFirstDbContext context;

        public StudentAPIController(CodeFirstDbContext context)
        {
            this.context = context; // check if dependency injection is working 
        }

        // GET: api/StudentAPI
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            try //	Contains the main logic; if all goes well, returns normal output.
            {
                var data = await context.Students.ToListAsync(); // check retrived students
                return Ok(data);
            }
            catch (Exception ex) //	Catches unexpected runtime errors and returns a 500 Internal Server Error.
            {
                return StatusCode(500, "Internal server error: { ex.Message}");
            }
        }


        // GET: api/StudentAPI/1 ,GetStudentById
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            try
            {
                var student = await context.Students.FindAsync(id); //check if student exists
                if (student == null)
                    return NotFound($"Student with ID = {id} not found."); //	Used when the requested resource (e.g., student) is not available.
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: { ex.Message}");
            }
        }


        // POST: api/StudentAPI,Create
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student std)
        {
            try
            {
                await context.Students.AddAsync(std); //confirm creation
                await context.SaveChangesAsync();//
                return Ok(std);
            }
            catch
            {
                return StatusCode(500, "Internal server error: { ex.Message}");
            }
        }

        // PUT: api/StudentAPI/1 ,Update
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, Student std)
        {
            if (id != std.Id) //check ID match
                return BadRequest("Student ID Mismatch."); //Used when input validation fails(e.g., ID mismatch in PUT).

            try
            {
                context.Entry(std).State = EntityState.Modified; // entity tracking
                await context.SaveChangesAsync();
                return Ok(std);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                    return NotFound($"Student with ID = {id} not found.");
                else
                    throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating student: {ex.Message}");
            }
        }


        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            try
            {
                var std = await context.Students.FindAsync(id); //confirm existence
                if (std == null)
                    return NotFound($"Student with ID = {id} not found.");


                context.Students.Remove(std); // confirm deletion
                await context.SaveChangesAsync();
                return Ok($"tudent with ID = {id} deleted.");
            }
            catch(Exception ex) //Catches unexpected runtime errors and returns a 500 Internal Server Error.
            {
                return StatusCode(500, $"Error deleting studnet: {ex.Message}");
            }
        }
        private bool StudentExists(int id)
        {
            return context.Students.Any(e => e.Id == id); //Method checks if a student exists in the database using the Any() LINQ method.
        }//used in the catch (DbUpdateConcurrencyException) block of the PUT method to return a (404 Not Found) Error.

    }
}
