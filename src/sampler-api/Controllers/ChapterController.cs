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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapter(int id)
        {
            Chapter chapter = await ChapterRepository.GetInitChapter(id);
            return Ok(chapter);
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> GetUpdatedChapter(int id, [FromQuery]SimulateParams simulateParams)
        {
            Chapter test2 = await ChapterRepository.GetUpdatedChapter(id, simulateParams);
            return Ok(test2);
        }
    }
}