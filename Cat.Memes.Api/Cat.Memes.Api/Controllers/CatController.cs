using Cat.Memes.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cat.Memes.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CatController : ControllerBase
{
    private readonly ICatMemeService _catMemeService;
    private readonly IConfiguration _configuration;

    public CatController(ICatMemeService catMemeService, IConfiguration configuration)
    {
        _catMemeService = catMemeService;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var catMemes = _catMemeService.GetCatMemes();
        foreach (var catMeme in catMemes)
        {
            catMeme.Secret = _configuration["TestSecret"];
        }
        return Ok(catMemes);
    }
}