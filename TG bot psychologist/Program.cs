using TG_bot_psychologist.AppSettings;
using TG_bot_psychologist.Services;
using TG_bot_psychologist.StateMachine;
using Newtonsoft.Json;
using Telegram.Bot;

public class Program
{
    private static async Task Main()
    {
        var secretsJson = File.ReadAllText("AppSettings/secrets.json");
        var secrets = JsonConvert.DeserializeObject<Secrets>(secretsJson);

        var settingsJson = File.ReadAllText("AppSettings/app_settings.json");
        var settings = JsonConvert.DeserializeObject<AppSettings>(settingsJson);

        var botClient = new TelegramBotClient(secrets.ApiKeys.TelegramKey);

        ErrorNotificationService.Initialize(botClient, settings.ErrorsLogChannelId, settings.ErrorsFilePath);

        var subscriptionService = new SubscriptionService(botClient, settings.SubscribeChannelId);
        IChatGptService chatGptService = new ChatGptService(secrets.ApiKeys.OpenAiKey, settings);
        var usersDataProvider = new UsersDataProvider();

        var chatStateMachine = new ChatStateMachine(botClient, settings, chatGptService, usersDataProvider);
        var chatStateController = new ChatStateController(chatStateMachine);

        var telegramBot = new TelegramBotController(botClient, subscriptionService, chatStateController);

        telegramBot.StartBot();
        await Task.Delay(Timeout.Infinite);
    }
}