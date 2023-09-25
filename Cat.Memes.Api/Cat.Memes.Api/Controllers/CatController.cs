using Cat.Memes.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cat.Memes.Api.Controllers;

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
    public IActionResult Get()
    {
        return Ok(_catMemeService.GetCatMemes());
    }
}