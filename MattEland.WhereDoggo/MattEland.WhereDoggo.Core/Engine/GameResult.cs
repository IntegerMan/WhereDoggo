namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// The final result of a game, including which team(s) won and which players are included in that set of winners.
/// </summary>
public class GameResult
{
    public GameResult(bool werewolfKilled, IEnumerable<GamePlayer> winners)
    {
        WerewolfKilled = werewolfKilled;
        Winners = winners;
    }

    /// <summary>
    /// Whether or not at least one werewolf was killed
    /// </summary>
    public bool WerewolfKilled { get; }
    
    /// <summary>
    /// The player(s) who won. 
    /// </summary>
    public IEnumerable<GamePlayer> Winners { get; }
}