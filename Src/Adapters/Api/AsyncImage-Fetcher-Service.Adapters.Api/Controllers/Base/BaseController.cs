using Microsoft.AspNetCore.Mvc;

namespace AsyncImage_Fetcher_Service.Adapters.Api.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
    }
}