namespace Flyweight;

/// <summary>
/// Concrete Flyweight
/// </summary>
public class CharacterA : ICharacter
{
    private const char _actualCharacter = 'a';
    private string _fontFamily = string.Empty;
    private int _fontSize;

    public void Draw(string fontFamily, int fontSize)
    {
        _fontFamily = fontFamily;
        _fontSize = fontSize;
        Console.WriteLine($"Drawing {_actualCharacter}, {_fontFamily} {_fontSize}");
    }
}