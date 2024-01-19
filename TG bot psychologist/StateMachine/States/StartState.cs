using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG_bot_psychologist.Data;
using TG_bot_psychologist.Extensions;

namespace TG_bot_psychologist.StateMachine.States;

public class StartState : ChatStateBase
{
    private readonly ITelegramBotClient _botClient;
    private readonly string _agencyName;


    public StartState(ChatStateMachine stateMachine, ITelegramBotClient botClient, string agencyName) :
        base(stateMachine)
    {
        _botClient = botClient;
        _agencyName = agencyName;
    }

    public override async Task OnEnter(long chatId)
    {
        await base.OnEnter(chatId);
        await SendGreeting(chatId);
    }
    
    public override Task HandleMessage(Message message)
    {
        return Task.CompletedTask;
    }

    private async Task SendGreeting(long chatId)
    {
        var greetingText =
            $"Приветствую!\nЯ первый психологический бот с искусственным интелектом, созданный агентством психологической поддержки *{_agencyName}*." +
            "\nЯ могу оказать Вам мгновенную первичную консультацию по любому интересующему Вас вопросу," +
            " либо передать Ваши данные нашему специалисту для более детальной консультации.";
        greetingText = greetingText.EscapeMarkdownV2();

        var button1 = InlineKeyboardButton.WithCallbackData("Задать вопрос", GlobalData.QUESTION);
        var button2 = InlineKeyboardButton.WithCallbackData("Консультация специалиста", GlobalData.SPECIALIST);

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { button1 },
            new[] { button2 }
        });

        await _botClient.SendTextMessageAsync(chatId, greetingText, replyMarkup: keyboard,
            parseMode: ParseMode.MarkdownV2);
        await _stateMachine.TransitTo<IdleState>(chatId);
    }
}