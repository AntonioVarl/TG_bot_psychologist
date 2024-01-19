using Telegram.Bot;
using Telegram.Bot.Types;
using TG_bot_psychologist.Services;

namespace TG_bot_psychologist.StateMachine.States;

public class WaitingForNameState : ChatStateBase
{
    private readonly UsersDataProvider _usersDataProvider;
    private readonly ITelegramBotClient _botClient;
    
    public WaitingForNameState(ChatStateMachine stateMachine, ITelegramBotClient botClient, UsersDataProvider usersDataProvider) : base(stateMachine)
    {
        _usersDataProvider = usersDataProvider;
        _botClient = botClient;
    }
    
    public override async Task OnEnter(long chatId)
    {
        await base.OnEnter(chatId);
        await _botClient.SendTextMessageAsync(chatId, "Пожалуйста, введите ваше Имя.");
    }

    public override async Task HandleMessage(Message message)
    {
        var chatId = message.Chat.Id;
        _usersDataProvider.SetUserName(chatId, message.Text);
        
        var telegramName = message.From?.Username;
        _usersDataProvider.SetTelegramName(chatId, telegramName);

        await _stateMachine.TransitTo<WaitingForPhoneState>(chatId);
    }
}