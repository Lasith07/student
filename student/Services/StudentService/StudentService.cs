using DemoAPI.DTOs;
using DemoAPI.DTOs.Requests;
using DemoAPI.DTOs.Responses;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoAPI.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<StudentService> logger;

        public StudentService(ApplicationDbContext context, ILogger<StudentService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public BaseResponse CreateStudent(CreateStudentRequest request)
        {
            BaseResponse response;

            try
            {
                SubjectModel? subject = context.Subjects.FirstOrDefault(s => s.subject_id == request.subject_id);

                if (subject == null)
                {
                    return new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = "Invalid subject code" }
                    };
                }

                StudentModel newStudent = new StudentModel
                {
                    first_name = request.first_name,
                    last_name = request.last_name,
                    address = request.address,
                    email = request.email,
                    contact_number = request.contact_number,
                    subject_id = subject.subject_id
                };

                context.Add(newStudent);
                context.SaveChanges();

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = new { message = "Successfully created the new student" }
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating student");
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
                return response;
            }
        }

        public BaseResponse StudentList()
        {
            BaseResponse response;

            try
            {
                List<object> students = context.Students
                    .Include(s => s.Subject)
                    .Select(student => new
                    {
                        student.id,
                        student.first_name,
                        student.last_name,
                        student.address,
                        student.email,
                        student.contact_number,
                        student.subject_id,
                        subject = new
                        {
                            student.Subject.subject_name,
                            student.Subject.subject_code,
                            student.Subject.in_charge
                        }
                    }).ToList<object>();

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = students
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching student list");
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
                return response;
            }
        }

        public BaseResponse GetStudentById(long id)
        {
            BaseResponse response;
            try
            {
                object? student = context.Students
                    .Include(s => s.Subject)
                    .Where(s => s.id == id)
                    .Select(s => new
                    {
                        s.id,
                        s.first_name,
                        s.last_name,
                        s.address,
                        s.email,
                        s.contact_number,
                        s.subject_id,
                        subject = new
                        {
                            s.Subject.subject_name,
                            s.Subject.subject_code,
                            s.Subject.in_charge
                        }
                    }).FirstOrDefault();

                if (student != null)
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status200OK,
                        data = new { student }
                    };
                }
                else
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = "No student found with the provided ID." }
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving student with ID {StudentId}", id);
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
                return response;
            }
        }

        public BaseResponse UpdateStudentById(long id, UpdateStudentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = "Invalid request data." }
                    };
                }

                logger.LogInformation("Updating student ID: {StudentId} with subject_id: {SubjectId}", id, request.subject_id);

                var student = context.Students.FirstOrDefault(s => s.id == id);
                if (student == null)
                {
                    return new BaseResponse
                    {
                        status_code = StatusCodes.Status404NotFound,
                        data = new { message = "Student not found." }
                    };
                }

                var subject = context.Subjects.FirstOrDefault(s => s.subject_id == request.subject_id);
                if (subject == null)
                {
                    return new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = $"Invalid subject ID: {request.subject_id}" }
                    };
                }

                var emailExists = context.Students.Any(s => s.email == request.email && s.id != id);
                if (emailExists)
                {
                    return new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = "Email already in use." }
                    };
                }

                student.first_name = request.first_name;
                student.last_name = request.last_name;
                student.address = request.address;
                student.email = request.email;
                student.contact_number = request.contact_number;
                student.subject_id = request.subject_id;

                context.SaveChanges();

                return new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = new { message = "Student updated successfully" }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating student with ID {StudentId}", id);
                return new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
            }
        }


        public BaseResponse DeleteStudentById(long id)
        {
            BaseResponse response;
            try
            {
                StudentModel? studentToDelete = context.Students.FirstOrDefault(s => s.id == id);

                if (studentToDelete != null)
                {
                    context.Students.Remove(studentToDelete);
                    context.SaveChanges();

                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status200OK,
                        data = new { message = "Student deleted successfully" }
                    };
                }
                else
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = "No student found" }
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting student with ID {StudentId}", id);
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
                return response;
            }
        }
    }
}
