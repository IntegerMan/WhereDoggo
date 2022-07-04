namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// This is a simple event just used for debugging purposes. It should have no impact on AI decisions.
/// </summary>
public class TextEvent : GameEventBase
{
    public string Message { get; }

    public TextEvent(string message)
    {
        Message = message;
    }

    public override string ToString() => Message;
}