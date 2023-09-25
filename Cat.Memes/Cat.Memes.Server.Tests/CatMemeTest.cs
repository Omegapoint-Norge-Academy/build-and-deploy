using Cat.Memes.Shared;

namespace Cat.Memes.Server.Tests;

public class CatMemeTest
{
    [Fact]
    public void CanCreateCatMeme()
    {
        var catMeme = new CatMeme("123");
        Assert.Equal("123", catMeme.Image);
    }
}