namespace MattEland.WhereDoggo.Core.Engine;

public class GameResult
{
    public GameResult(bool werewolfKilled, IEnumerable<GamePlayer> winners)
    {
        WerewolfKilled = werewolfKilled;
        Winners = winners;
    }

    public bool WerewolfKilled { get; }
    public IEnumerable<GamePlayer> Winners { get; }
}