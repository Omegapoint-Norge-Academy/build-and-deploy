using Cat.Memes.Server.Services;
using Cat.Memes.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cat.Memes.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatController : ControllerBase
    {
        private readonly ICatMemeService _catMemeService;

        public CatController(ICatMemeService catMemeService)
        {
            _catMemeService = catMemeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatMemes()
        {
            var cats = await _catMemeService.GetCatMemes();
            return Ok(cats);
        }
    }
}