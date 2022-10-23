using Microsoft.AspNetCore.Mvc;
using StudentDataManagement.Model;
using StudentDataManagement.Service;

namespace StudentDataManagement.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(Database.GetStudents());
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            Student student;
            try
            {
                student = Database.GetStudent(indexNumber);

            } catch(IncorrectStudentDataException)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            try
            {
                Database.CreateStudent(student);
            } catch(IncorrectStudentDataException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(student);
        }

        [HttpPut("{indexNumber}")]
        public IActionResult UdateStudent(string indexNumber, Student student)
        {
            if(indexNumber != student.IndexNumber)
            {
                return BadRequest();
            }

            return Ok(Database.UpdateStudent(indexNumber, student));

        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            try
            {
                Database.DeleteStudent(indexNumber);    
            } catch(IncorrectStudentDataException e)
            {
                return BadRequest(e.Message);
            }
                
            return Ok($"Usunięto z bazy danych studenta o indeksie {indexNumber}");
        }
    }
}
