using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TG_bot_psychologist.StateMachine.States;
using static TG_bot_psychologist.Data.GlobalData;

namespace TG_bot_psychologist.StateMachine;

public class ChatStateController
{
    private readonly ChatStateMachine _stateMachine;

    public ChatStateController(ChatStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task HandleUpdate(Update update)
    {
        string? data;
        Message message;

        switch (update.Type)
        {
            case UpdateType.Message:
                data = update.Message.Text;
                message = update.Message;
                break;
            case UpdateType.CallbackQuery:
                data = update.CallbackQuery.Data;
                message = update.CallbackQuery.Message;
                break;
            default:
                return;
        }

        var chatId = message.Chat.Id;

        switch (data)
        {
            case START:
                await _stateMachine.TransitTo<StartState>(chatId);
                break;
            case CHECK_SUBSCRIPTION:
                await _stateMachine.TransitTo<StartState>(chatId);
                break;
            case QUESTION:
                await _stateMachine.TransitTo<QuestionState>(chatId);
                break;
            case SPECIALIST:
                await _stateMachine.TransitTo<WaitingForNameState>(chatId);
                break;
            case DONE:
                await _stateMachine.TransitTo<DoneState>(chatId);
                break;
            default:
                var state = _stateMachine.GetState(chatId);
                await state.HandleMessage(message);
                break;
        }
    }
}