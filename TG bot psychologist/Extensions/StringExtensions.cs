namespace TG_bot_psychologist.Extensions;

public static class StringExtensions
{
    public static string? EscapeMarkdownV2(this string? text)
    {
        var specialCharacters = new[]
            { '_', '[', ']', '(', ')', '~', '`', '>', '#', '+', '-', '=', '|', '{', '}', '.', '!' };

        return specialCharacters.Aggregate(text, (current, character) => current?.Replace(character.ToString(), "\\" + character));
    }
}