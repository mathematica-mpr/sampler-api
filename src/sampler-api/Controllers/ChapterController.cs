using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sampler_api.Models;
using sampler_api.Repositories;

namespace sampler_api.Controllers
{
    [Route("api/[controller]")]
    public class ChapterController : Controller
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

        [HttpGet("{id}/update")]
        public async Task<IActionResult> GetUpdatedChapter(int id, [FromQuery]SimulateParams simulateParams)
        {
            Chapter chapter = await ChapterRepository.GetUpdatedChapter(id, simulateParams);
            return Ok(chapter);
        }
    }
}