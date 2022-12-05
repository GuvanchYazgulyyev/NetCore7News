using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Output_Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [OutputCache]
    [OutputCache(PolicyName = "Custom")]
    public class OutputCatchingController : ControllerBase
    {
    }
}
