using DemoAPI.DTOs.Requests;
using DemoAPI.DTOs.Responses;

namespace DemoAPI.Services.StudentService

{

    public interface IStudentService
    {

        BaseResponse CreateStudent(CreateStudentRequest request);

        BaseResponse StudentList();

        BaseResponse GetStudentById(long id);

        BaseResponse UpdateStudentById(long id, UpdateStudentRequest request);

        BaseResponse DeleteStudentById(long id);
        
    }

}