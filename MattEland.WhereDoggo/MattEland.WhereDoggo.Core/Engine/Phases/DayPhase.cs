using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// The day phase has all players wake up and look around
/// </summary>
public class DayPhase : GamePhaseBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DayPhase"/> class.
    /// </summary>
    /// <param name="game">The game instance</param>
    public DayPhase(Game game) : base(game)
    {
    }

    /// <inheritdoc />
    public override string Name => "Day";
    
    /// <inheritdoc />
    public override void Run(Game game)
    {
        // Wake all players up
        foreach (GamePlayer p in game.Players)
        {
            p.Wake();
        }
        
        // Each player should claim their role if they're good
        foreach (GamePlayer player in game.Players)
        {
            if (player.InitialCard.Team == Teams.Villagers)
            {
                LogEvent(new ClaimedRoleEvent(player, player.InitialCard.RoleType));
            }
        }
    }    
}