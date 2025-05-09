using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Services.SubjectService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoAPI.DTOs.Responses;
using DemoAPI.DTOs.Requests;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService Subject;

        public SubjectController(ISubjectService subjectservice)
        {
            Subject = subjectservice;
        }

        [HttpPost("save")]
        public BaseResponse CreateSubject(CreateSubjectRequest request)
        {
            return Subject.CreateSubject(request);
        }

        [HttpGet("list")]
        public BaseResponse SubjectList()
        {
            return Subject.SubjectList();
        }

        [HttpGet("id/{id}")]
        public BaseResponse GetStudentById(long id)
        {
            return Subject.GetSubjectById(id);
        }

        [HttpGet("student-count/{subjectId}")]
        public IActionResult GetStudentCountForSubject(long subjectId)
        {
            var result = Subject.GetStudentCountForSubject(subjectId);
            return StatusCode(result.status_code, result.data);
        }
    }
}
