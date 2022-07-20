using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : Controller
    { }
}
