using MattEland.WhereDoggo.Core.Gamespace;

namespace MattEland.WhereDoggo.Core.Events;

public abstract class GameEventBase
{
    public GamePhase Phase { get; }
    public GamePlayer? Player { get; }
    public int Id { get; set; }

    protected GameEventBase(GamePhase phase, GamePlayer? player = null)
    {
        Phase = phase;
        Player = player;
    }

    public virtual void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, ContainerRoleProbabilities probabilities)
    {
        // No updates by default
    }
}