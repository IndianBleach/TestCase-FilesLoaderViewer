using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : Controller
    {
    }
}
