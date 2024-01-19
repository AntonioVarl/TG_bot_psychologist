using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TG_bot_psychologist.Data;

namespace TG_bot_psychologist.StateMachine.States;

public class DoneState : ChatStateBase
{
    private readonly ITelegramBotClient _botClient;

    public DoneState(ChatStateMachine stateMachine, ITelegramBotClient botClient) : base(stateMachine)
    {
        _botClient = botClient;
    }

    public override async Task OnEnter(long chatId)
    {
        await base.OnEnter(chatId);
        var response = "Спасибо, что воспользовались нашими услугами.\n" +
                       "Если потребуется, я могу оказать Вам мгновенную первичную консультацию по любому интересующему" +
                       " Вас вопросу, либо передать Ваши данные нашему специалисту для более детальной консультации.";

        var button1 = InlineKeyboardButton.WithCallbackData("Задать вопрос", GlobalData.QUESTION);
        var button2 = InlineKeyboardButton.WithCallbackData("Консультация специалиста", GlobalData.SPECIALIST);

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { button1 },
            new[] { button2 }
        });

        await _botClient.SendTextMessageAsync(chatId, response, replyMarkup: keyboard);
        await _stateMachine.TransitTo<IdleState>(chatId);
    }

    public override Task HandleMessage(Message message)
    {
        return Task.CompletedTask;
    }
}