using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sampler_api.Models;
using sampler_api.Repositories;

namespace sampler_api.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository MenuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            MenuRepository = menuRepository;
        }

        public async Task<IActionResult> GetMenu()
        {
            try
            {
                Menu menu = await MenuRepository.GetMenu();
                return Ok(menu);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}