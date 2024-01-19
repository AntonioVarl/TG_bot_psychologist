using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TG_bot_psychologist.Services;

public class SubscriptionService
{
    public ChatId SubscribeChatId { get; }
    private readonly ITelegramBotClient _botClient;

    private readonly List<ChatMemberStatus> _allowedStatuses = new()
    {
        ChatMemberStatus.Member, ChatMemberStatus.Administrator, ChatMemberStatus.Creator
    };

    public SubscriptionService(TelegramBotClient botClient, ChatId subscribeChatId)
    {
        _botClient = botClient;
        SubscribeChatId = subscribeChatId;
    }

    public async Task<bool> IsSubscribed(long userId)
    {
        try
        {
            var chatMember = await _botClient.GetChatMemberAsync(SubscribeChatId, userId);
            if (_allowedStatuses.Contains(chatMember.Status))
            {
                return true;
            }
        }
        catch (ApiRequestException exception)
        {
            Console.WriteLine(exception);
            return false;
        }

        return false;
    }
}