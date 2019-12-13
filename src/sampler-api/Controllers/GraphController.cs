using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace sampler_api.Controllers
{
    [Route("api/[controller]")]
    public class GraphController : ControllerBase
    {

        public async Task<IActionResult> GetMenu()
        {
            try
            {

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}