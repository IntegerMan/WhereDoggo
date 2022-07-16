using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs when a revealer turns back over an evil card they previously revealed.
/// </summary>
public class RevealerHidEvilRoleEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="RevealerHidEvilRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The revealer</param>
    /// <param name="target">The card being hidden</param>
    public RevealerHidEvilRoleEvent(GamePlayer player, CardContainer target) : base(GamePhases.Night, player, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} turned back over {Target}";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}