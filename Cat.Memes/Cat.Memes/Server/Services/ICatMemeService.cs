using Cat.Memes.Shared;

namespace Cat.Memes.Server.Services;

public interface ICatMemeService
{
    Task<List<CatMeme>> GetCatMemes();
}