using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs when a player knows if another player is on the Werewolf team.
/// This event does not tell the player which werewolf role the player had.
/// </summary>
public class SawAsWerewolfEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="SawAsWerewolfEvent"/> event.
    /// </summary>
    /// <param name="player">The player observing the <paramref name="target"/> as one of the werewolves</param>
    /// <param name="target">The player known to be one of the werewolves</param>
    public SawAsWerewolfEvent(GamePlayer player, CardContainer target) : base(GamePhases.Night, player, target)
    {
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        if (Target == target)
        {
            foreach (RoleTypes role in probabilities.PossibleRoles.Where(r => r.DetermineTeam() != Teams.Werewolves))
            {
                probabilities.MarkAsCannotBeRole(role);
            }
        }
    }
}