using Telegram.Bot.Types;

namespace TG_bot_psychologist.AppSettings;

public class AppSettings
{
    public ChatId ManagerChannelId { get; init; }
    public ChatId SubscribeChannelId { get; init; }
    public string GptPrompt { get; init; }
    public string AgencyName { get; init; }
    public ChatId ErrorsLogChannelId { get; init; }
    public string ErrorsFilePath { get; init; }
}