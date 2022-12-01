namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs if a <see cref="MasonRole"/> wakes up and sees that
/// there is no other <see cref="MasonRole"/> in play.
/// </summary>
public class OnlyMasonEvent : GameEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OnlyMasonEvent"/> class.
    /// </summary>
    /// <param name="player">The player that is the lone mason</param>
    public OnlyMasonEvent(GamePlayer player) : base(player)
    {
        
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that they were the only Mason";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new OnlyMasonClaim(Player!);
    }

}