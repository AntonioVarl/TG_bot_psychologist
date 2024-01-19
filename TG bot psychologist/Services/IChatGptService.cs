namespace TG_bot_psychologist.Services;

public interface IChatGptService
{
    public Task<string?> GetAnswerFromChatGpt(string question);
}