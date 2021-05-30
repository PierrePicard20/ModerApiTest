using Microsoft.AspNetCore.Mvc;

namespace ModerApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error() => Problem(statusCode: 500);
    }
}
