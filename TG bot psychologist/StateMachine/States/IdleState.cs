using Telegram.Bot.Types;

namespace TG_bot_psychologist.StateMachine.States;

public class IdleState : ChatStateBase
{
    public IdleState(ChatStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override Task HandleMessage(Message message)
    {
        return Task.CompletedTask;
    }
}