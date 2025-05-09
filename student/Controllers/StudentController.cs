using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Services.StudentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoAPI.DTOs.Responses;
using DemoAPI.DTOs.Requests;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService student;


        public StudentController(IStudentService studentservice)
        {
            student = studentservice;
        }

        [HttpPost("save")]
        public BaseResponse CreateStudent([FromBody] CreateStudentRequest request)
        {
            return student.CreateStudent(request);
        }


        [HttpGet("list")]

        public BaseResponse StudentList()
        {
            return student.StudentList();
            
        }

        [HttpGet("id/{id}")]
        public BaseResponse GetStudentById(long id)
        {
            return student.GetStudentById(id);

        }


        [HttpPut("update/{id}")]
        public BaseResponse UpdateStudentById(long id, UpdateStudentRequest request)
        {
            return student.UpdateStudentById(id, request);

        }

        [HttpDelete("delete/{id}")]
        public BaseResponse DeleteStudentId(long id)
        {
            return student.DeleteStudentById(id);

        }

    }
}
