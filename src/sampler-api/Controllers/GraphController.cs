using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sampler_api.Models;
using sampler_api.Repositories;

namespace sampler_api.Controllers
{
    [Route("api/[controller]")]
    public class GraphController : ControllerBase
    {
        private readonly IGraphRepository GraphRepository;
        public GraphController(IGraphRepository graphRepository)
        {
            GraphRepository = graphRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetGraph([FromQuery]SimulateParams simulateParams)
        {
            try
            {
                List<Graph> graphs = await GraphRepository.GetGraphs(simulateParams);
                return Ok(graphs);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}