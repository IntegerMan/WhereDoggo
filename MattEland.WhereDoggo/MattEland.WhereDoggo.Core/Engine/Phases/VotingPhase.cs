namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// The voting phase occurs when all players commit their votes. This is the final phase of the game.
/// </summary>
public class VotingPhase : GamePhaseBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VotingPhase"/> class.
    /// </summary>
    /// <param name="game">The game.</param>
    public VotingPhase(Game game) : base(game)
    {
        
    }

    /// <inheritdoc />
    public override string Name => "Voting";
    
    /// <inheritdoc />
    public override void Run(Game game)
    {
        // Create a dictionary of votes without any votes in it
        Dictionary<GamePlayer, int> votes = CreateVoteTracker();

        // Get votes for individual players
        CollectVotesFromPlayers(votes);

        IList<GamePlayer> votedPlayers = CalculateVotedOutPlayers(votes);
        foreach (GamePlayer votedPlayer in votedPlayers)
        {
            BroadcastEvent(new VotedOutEvent(votedPlayer));
        }

        bool wwVoted = votedPlayers.Any(p => p.CurrentCard.Team == Teams.Werewolves); // Revisit for Minion
        bool wwPresent = game.Players.Any(p => p.CurrentCard.Team == Teams.Werewolves);
        
        IList<GamePlayer> villagers = Game.Players.Where(p => p.CurrentCard.Team == Teams.Villagers).ToList();
        IList<GamePlayer> wolves = Game.Players.Where(p => p.CurrentCard.Team == Teams.Werewolves).ToList();
        
        // Villagers win if they vote out at least one werewolf or if nobody dies and no werewolves are present
        bool isCircleVote = votedPlayers.Count <= 0;
        IList<GamePlayer> winners = wwVoted || (!wwPresent && isCircleVote) ? villagers : wolves;
        
        Game.Result = new GameResult(wwVoted, winners);

        BroadcastEvent(Game.Result.Winners.Any()
            ? $"The winners are {string.Join(", ", winners.Select(w => $"{w.Name} ({w.CurrentCard})"))}"
            : "No players won.");        
    }

    private static IList<GamePlayer> CalculateVotedOutPlayers(Dictionary<GamePlayer, int> votes)
    {
        int maxVotes = votes.Values.Max();

        // In the case where people voted in a circle, nobody dies.
        if (maxVotes <= 1) { return new List<GamePlayer>(); }
        
        IEnumerable<GamePlayer> votedPlayers = votes.Where(kvp => kvp.Value == maxVotes).Select(kvp => kvp.Key);
        
        return votedPlayers.ToList();
    }

    private void CollectVotesFromPlayers(IDictionary<GamePlayer, int> votes)
    {
        foreach (GamePlayer player in Game.Players)
        {
            GamePlayer votedPlayer = player.DetermineVoteTarget(Game.Randomizer);

            VotedEvent votedEvent = new(player, votedPlayer);
            BroadcastEvent(votedEvent);

            votes[votedPlayer] += 1;
        }
    }

    private Dictionary<GamePlayer, int> CreateVoteTracker()
    {
        Dictionary<GamePlayer, int> votes = new();
        foreach (GamePlayer player in Game.Players)
        {
            votes[player] = 0;
        }

        return votes;
    }
}