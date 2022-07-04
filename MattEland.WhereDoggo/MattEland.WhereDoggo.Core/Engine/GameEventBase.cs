namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameEventBase
{
    public GamePlayer? Player { get; }
    public int Id { get; set; }

    protected GameEventBase(GamePlayer? player = null)
    {
        Player = player;
    }
}