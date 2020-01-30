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

        [HttpGet]
        public async Task<IActionResult> GetGraph(string guid)
        {
            try
            {
                List<Graph> graphs = await GraphRepository.GetGraphs(guid);
                return Ok(graphs);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("Update")]
        public async Task<IActionResult> UpdateGraph([FromQuery]SimulateParams simulateParams)
        {
            try
            {
                List<Graph> graphs = await GraphRepository.UpdateGraphs(simulateParams);
                return Ok(graphs);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}