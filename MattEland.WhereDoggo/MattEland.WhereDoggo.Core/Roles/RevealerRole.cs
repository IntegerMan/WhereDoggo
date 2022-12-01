namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Revealer from One Night Ultimate Werewolf Daybreak
/// The Revealer looks at another player's card.
/// If that card is not a Werewolf or Tanner, the card stays flipped over
/// </summary>
/// <href>http://onenightultimate.com/?p=73</href>
[RoleFor(RoleTypes.Revealer)]
public class RevealerRole : CardBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Revealer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new RevealerNightAction();
        }
    }
}