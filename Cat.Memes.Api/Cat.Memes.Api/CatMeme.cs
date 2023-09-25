namespace Cat.Memes.Api;

public record CatMeme(string Image)
{
    public string Secret { get; set; }
};