namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameEventBase
{
    public GamePlayer? Player { get; }

    protected GameEventBase(GamePlayer? player = null)
    {
        Player = player;
    }
}