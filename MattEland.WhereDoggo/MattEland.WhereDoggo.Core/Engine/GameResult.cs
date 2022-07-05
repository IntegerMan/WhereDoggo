namespace MattEland.WhereDoggo.Core.Engine;

public class GameResult
{
    public bool WerewolfKilled { get; set; }
    public IEnumerable<GamePlayer> Winners { get; set; }
}