namespace MattEland.WhereDoggo.Core.Events.Claims;

/// <summary>
/// Represents a social claim during the day phase
/// </summary>
public abstract class ClaimBase : GameEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimBase"/> class.
    /// </summary>
    /// <param name="player">The player that made the claim</param>
    protected ClaimBase(GamePlayer player) : base(player)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the claim is a lie
    /// </summary>
    public bool IsLie { get; set; }

    /// <summary>
    /// The textual description of the event
    /// </summary>
    public abstract string Text { get; }

    /// <inheritdoc />
    public override string ToString() => Text;
}