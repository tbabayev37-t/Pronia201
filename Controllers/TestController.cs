using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Abstractions;

namespace MVCProniaTask.Controllers
{
    public class TestController(IEmailService _service) : Controller
    {
       public async Task<IActionResult>  SendEmail()
        {
           await _service.SendEmailAsync("tuncayab-mpa201@code.edu.az", "Email service", "<h1 style='color:red'>Service is done</h1>");
            return Ok("Ok");
        }
    }
}
