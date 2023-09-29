using Cat.Memes.Shared;

namespace Cat.Memes.Server.Services;

public class CatMemeService : ICatMemeService
{
    private readonly HttpClient _client;

    public CatMemeService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<CatMeme>> GetCatMemes()
    {
        try
        {
            return await _client.GetFromJsonAsync<List<CatMeme>>("Cat") ?? new List<CatMeme>();
        }
        catch (Exception)
        {
            return new List<CatMeme>();
        }
    }
}