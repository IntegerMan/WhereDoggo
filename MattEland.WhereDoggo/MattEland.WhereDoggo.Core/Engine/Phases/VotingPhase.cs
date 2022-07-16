namespace MattEland.WhereDoggo.Core.Engine.Phases;

public class VotingPhase : GamePhaseBase
{
    public VotingPhase(Game game) : base(game)
    {
        
    }
    
    /// <inheritdoc />
    public override GamePhases Phase => GamePhases.Voting;

    /// <inheritdoc />
    public override string Name => "Voting";
    
    /// <inheritdoc />
    public override void Run(Game game)
    {
        // Create a dictionary of votes without any votes in it
        Dictionary<GamePlayer, int> votes = CreateVoteTracker();

        // Get votes for individual players
        CollectVotesFromPlayers(votes);

        IEnumerable<GamePlayer> votedPlayers = CalculateVotedOutPlayers(votes);
        foreach (GamePlayer votedPlayer in votedPlayers)
        {
            BroadcastEvent(new VotedOutEvent(votedPlayer));
        }

        bool wwVoted = votedPlayers.Any(p => p.CurrentTeam == Teams.Werewolves); // Revisit for Minion
        
        IList<GamePlayer> villagers = Game.Players.Where(p => p.CurrentTeam == Teams.Villagers).ToList();
        IList<GamePlayer> wolves = Game.Players.Where(p => p.CurrentTeam == Teams.Werewolves).ToList();
        IList<GamePlayer> winners = wwVoted ? villagers : wolves;
        
        Game.Result = new GameResult(wwVoted, winners);

        BroadcastEvent(wwVoted
            ? "The village wins!"
            : "The werewolves win!");

        BroadcastEvent(Game.Result.Winners.Any()
            ? $"The winners are {string.Join(", ", winners.Select(w => $"{w.Name} ({w.CurrentRole})"))}"
            : "No players won.");        
    }

    private static IEnumerable<GamePlayer> CalculateVotedOutPlayers(Dictionary<GamePlayer, int> votes)
    {
        int maxVotes = votes.Values.Max();
        
        IEnumerable<GamePlayer> votedPlayers = votes.Where(kvp => kvp.Value == maxVotes).Select(kvp => kvp.Key);
        
        return votedPlayers.ToList();
    }

    private void CollectVotesFromPlayers(IDictionary<GamePlayer, int> votes)
    {
        foreach (GamePlayer player in Game.Players)
        {
            GamePlayer votedPlayer = player.DetermineVoteTarget(Game.Randomizer);

            VotedEvent votedEvent = new(player, votedPlayer);
            LogEvent(votedEvent);

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