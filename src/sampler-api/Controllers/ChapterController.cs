using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sampler_api.Helpers;
using sampler_api.Models;
using sampler_api.Repositories;

namespace sampler_api.Controllers
{
    [Route("api/[controller]")]
    public class ChapterController : ControllerBase
    {

        private readonly IChapterRepository ChapterRepository;

        public ChapterController(IChapterRepository chapterRepository)
        {
            ChapterRepository = chapterRepository;
        }
        [HttpGet("init")]
        public async Task<IActionResult> GetInitChapter()
        {
            Chapter chapter = await ChapterRepository.GetInitChapter();
            return Ok(chapter);
        }


        [HttpGet("update")]
        public async Task<IActionResult> UpdateGraphs([FromQuery]SimulateParams simulateParams, [FromBody]List<ChapterGraph> chapterGraphs)
        {
            List<ChapterGraph> chapter = await ChapterRepository.UpdateGraphs(simulateParams, chapterGraphs);
            return Ok(chapter);
        }
    }
}