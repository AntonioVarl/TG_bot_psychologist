using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG_bot_psychologist.Data;
using TG_bot_psychologist.Extensions;
using TG_bot_psychologist.Services;

namespace TG_bot_psychologist.StateMachine.States;

public class QuestionState : ChatStateBase
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatGptService _chatGptService;
    private readonly UsersDataProvider _usersDataProvider;

    public QuestionState(ChatStateMachine stateMachine, ITelegramBotClient botClient, IChatGptService chatGptService,
        UsersDataProvider usersDataProvider) : base(stateMachine)
    {
        _botClient = botClient;
        _chatGptService = chatGptService;
        _usersDataProvider = usersDataProvider;
    }
    
    public override async Task OnEnter(long chatId)
    {
        await base.OnEnter(chatId);

        var response = "Задайте свой вопрос. Мне может потребоваться около минуты, чтобы ответить.";
        await _botClient.SafeSendTextMessageAsync(chatId, response);
    }
    
    public override async Task HandleMessage(Message message)
    {
        var chatId = message.Chat.Id;
        var question = message.Text;

        _usersDataProvider.SaveLastQuestion(chatId, question);

        await _botClient.SendChatActionAsync(chatId, ChatAction.Typing);

        var answer = await _chatGptService.GetAnswerFromChatGpt(question);

        if (answer == null)
        {
            await HandleNullAnswer(chatId);
            return;
        }

        await _botClient.SendTextMessageAsync(chatId, answer);

        var replyMarkup = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Задать вопрос", GlobalData.QUESTION)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Консультация специалиста", GlobalData.SPECIALIST)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Я все узнал, спасибо", GlobalData.DONE)
            }
        });

        var response = "Хотите задать еще вопрос?";
        await _botClient.SafeSendTextMessageAsync(chatId, response, replyMarkup: replyMarkup);
        await _stateMachine.TransitTo<IdleState>(chatId);
    }

    private async Task HandleNullAnswer(long chatId)
    {
        var response = "Пожалуйста, задайте Ваш вопрос снова.";
        var button1 = InlineKeyboardButton.WithCallbackData("Задать вопрос", GlobalData.QUESTION);
        var button2 = InlineKeyboardButton.WithCallbackData("Консультация специалиста", GlobalData.SPECIALIST);

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { button1 },
            new[] { button2 }
        });

        await _botClient.SafeSendTextMessageAsync(chatId, response, replyMarkup: keyboard);
    }
}