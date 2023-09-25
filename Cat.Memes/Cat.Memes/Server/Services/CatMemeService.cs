using Cat.Memes.Shared;

namespace Cat.Memes.Server.Services;

public class CatMemeService : ICatMemeService
{
    private readonly HttpClient _client;

    public CatMemeService(HttpClient client)
    {
        _client = client;
    }

    public Task<List<CatMeme>> GetCatMemes()
    {
        return _client.GetFromJsonAsync<List<CatMeme>>("Cat")!;
    }
}