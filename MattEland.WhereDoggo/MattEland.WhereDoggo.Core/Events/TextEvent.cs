namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This is a simple event just used for debugging purposes. It should have no impact on AI decisions.
/// </summary>
public class TextEvent : GameEventBase
{
    /// <summary>
    /// The message that was logged.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="TextEvent"/> class.
    /// </summary>
    /// <param name="message">The message to be logged</param>
    public TextEvent(string message) => Message = message;

    /// <inheritdoc />
    public override string ToString() => Message;
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}