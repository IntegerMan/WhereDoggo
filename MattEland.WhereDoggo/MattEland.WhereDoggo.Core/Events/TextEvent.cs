namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This is a simple event just used for debugging purposes. It should have no impact on AI decisions.
/// </summary>
public class TextEvent : GameEventBase
{
    public string Message { get; }

    public TextEvent(GamePhase phase, string message) : base(phase)
    {
        Message = message;
    }

    /// <inheritdoc />
    public override string ToString() => Message;
}