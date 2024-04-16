using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OpenAIDemo.Services;

namespace OpenAIDemo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherServices _teacherServices;
        public TeachersController(ITeacherServices teacherServices)
        {

            _teacherServices = teacherServices;

        }
        // [HttpGet("getTeachers")]
        [HttpGet]
        [Route("/getTeachers2")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var result = await _teacherServices.GetAllTeachers();
            return Ok(result);
        }
    }
}
