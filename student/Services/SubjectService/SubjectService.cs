using DemoAPI.DTOs.Requests;
using DemoAPI.DTOs.Responses;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoAPI.Services.SubjectService
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext context;

        public SubjectService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public BaseResponse CreateSubject(CreateSubjectRequest request)
        {
            BaseResponse response;

            try
            {
                SubjectModel newSubject = new SubjectModel
                {
                    subject_code = request.subject_code,
                    subject_name = request.subject_name,
                    in_charge = request.in_charge
                };

                context.Subjects.Add(newSubject);
                context.SaveChanges();

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = new { message = "Subject created successfully" }
                };

                return response;
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };

                return response;
            }
        }

        public BaseResponse SubjectList()
        {
            BaseResponse response;

            try
            {
                List<object> subjects = context.Subjects.Select(subject => new
                {
                    subject.subject_id,
                    subject.subject_code,
                    subject.subject_name,
                    subject.in_charge
                }).ToList<object>();

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = subjects
                };

                return response;
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };

                return response;
            }
        }

        public BaseResponse GetStudentCountForSubject(long subjectId)
        {
            BaseResponse response;

            try
            {
                bool subjectExists = context.Subjects.Any(s => s.subject_id == subjectId);

                if (!subjectExists)
                {
                    return new BaseResponse
                    {
                        status_code = StatusCodes.Status404NotFound,
                        data = new { message = "Subject not found." }
                    };
                }

                int studentCount = context.Students.Count(s => s.subject_id == subjectId);

                response = new BaseResponse
                {
                    status_code = StatusCodes.Status200OK,
                    data = new
                    {
                        subject_id = subjectId,
                        student_count = studentCount
                    }
                };
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    status_code = StatusCodes.Status500InternalServerError,
                    data = new { message = "Internal server error: " + ex.Message }
                };
            }

            return response;
        }


        public BaseResponse GetSubjectById(long id)
        {
            BaseResponse response;

            try
            {
                object? subject = context.Subjects
                    .Where(s => s.subject_id == id)
                    .Select(s => new
                    {
                        s.subject_id,
                        s.subject_code,
                        s.subject_name,
                        s.in_charge
                    })
                    .FirstOrDefault();

                if (subject != null)
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status200OK,
                        data = new { subject }
                    };
                }
                else
                {
                    response = new BaseResponse
                    {
                        status_code = StatusCodes.Status400BadRequest,
                        data = new { message = "No subject found" }
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
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
