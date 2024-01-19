using Telegram.Bot.Types.ReplyMarkups;

namespace TG_bot_psychologist.Data;

public class UserData
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string? Telegram { get; set; }
    public string? LastQuestion { get;  set; }
}