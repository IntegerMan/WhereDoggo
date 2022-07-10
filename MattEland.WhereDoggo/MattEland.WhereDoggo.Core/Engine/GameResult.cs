namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// The final result of a game, including which team(s) won and which players are included in that set of winners.
/// </summary>
/// <remarks>
/// This class is immutable
/// </remarks>
public class GameResult
{
    /// <summary>
    /// Creates a new instance of the <see cref="GameResult"/> class
    /// </summary>
    /// <param name="werewolfKilled"><c>true</c> if at least one werewolf was voted out, otherwise <c>false</c></param>
    /// <param name="winners">The player(s) who won.</param>
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