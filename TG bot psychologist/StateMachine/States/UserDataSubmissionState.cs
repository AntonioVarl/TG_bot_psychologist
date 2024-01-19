using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using TG_bot_psychologist.Data;
using TG_bot_psychologist.Services;

namespace TG_bot_psychologist.StateMachine.States;

public class UserDataSubmissionState : ChatStateBase
{
    private readonly UsersDataProvider _usersDataProvider;
    private readonly ITelegramBotClient _botClient;
    private readonly ChatId _managerChannelId;

    public UserDataSubmissionState(ChatStateMachine stateMachine, ITelegramBotClient botClient,
        UsersDataProvider usersDataProvider, ChatId managerChannelId) : base(stateMachine)
    {
        _usersDataProvider = usersDataProvider;
        _managerChannelId = managerChannelId;
        _botClient = botClient;
    }

    public override async Task OnEnter(long chatId)
    {
        await base.OnEnter(chatId);

        var response = "Спасибо за обращение. Данные переданы нашему специалисту, он свяжется с Вами в скором времени.";
        await _botClient.SendTextMessageAsync(chatId, response);
        await _stateMachine.TransitTo<IdleState>(chatId);
    }

    public override Task HandleMessage(Message message)
    {
        return Task.CompletedTask;
    }
    
    public override async Task OnExit(long chatId)
    {
        await base.OnExit(chatId);
        await SendUserInfoToManager(chatId);
    }

    private async Task SendUserInfoToManager(long chatId)
    {
        try
        {
            var userProfile = _usersDataProvider.GetUserData(chatId);
            var userInfo = BuildUserInfo(userProfile);

            await _botClient.SendTextMessageAsync(_managerChannelId, userInfo);

            _usersDataProvider.ClearUserData(chatId);
        }
        catch (ApiRequestException exception)
        {
            Console.WriteLine(exception);
        }
    }

    private string BuildUserInfo(UserData userProfile)
    {
        var userInfo = "Информация о клиенте:" +
                       $"\nФИО: {userProfile.Name}" +
                       $"\nНомер телефона: {userProfile.Phone}";

        if (!string.IsNullOrEmpty(userProfile.Telegram))
        {
            userInfo += $"\nTelegram: @{userProfile.Telegram}";
        }

        if (!string.IsNullOrEmpty(userProfile.LastQuestion))
        {
            userInfo += $"\nПоследний отправленный вопрос боту: {userProfile.LastQuestion}";
        }

        return userInfo;
    }
}