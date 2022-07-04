namespace MattEland.WhereDoggo.Core.Engine;

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
}