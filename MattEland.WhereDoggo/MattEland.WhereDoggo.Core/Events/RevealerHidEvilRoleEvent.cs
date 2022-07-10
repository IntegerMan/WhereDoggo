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
    public RevealerHidEvilRoleEvent(GamePlayer player, RoleContainerBase target) : base(GamePhase.Night, player, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} turned back over {Target}";
}