namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// The voting phase occurs when all players commit their votes. This is the final phase of the game.
/// </summary>
public class VotingPhase : GamePhaseBase
{
    private readonly Dictionary<GamePlayer, int> votes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="VotingPhase"/> class.
    /// </summary>
    /// <param name="game">The game.</param>
    public VotingPhase(Game game) : base(game)
    {
    }

    /// <inheritdoc />
    protected internal override void Initialize(Game game)
    {
        // Create a dictionary of votes without any votes in it
        foreach (GamePlayer player in Game.Players)
        {
            votes[player] = 0;
        }

        foreach (GamePlayer player in Game.Players)
        {
            EnqueueAction(() => CollectVote(player));
        }

        EnqueueAction(() => {
            TabulateVotes();
            DetermineWinners(game);
        });
    }

    /// <inheritdoc />
    public override string Name => "Voting";

    private void DetermineWinners(Game game)
    {
        bool wwVoted = votedOutPlayers.Any(p => p.CurrentCard.Team == Teams.Werewolves); // Revisit for Minion
        bool wwPresent = game.Players.Any(p => p.CurrentCard.Team == Teams.Werewolves);

        IList<GamePlayer> villagers = Game.Players.Where(p => p.CurrentCard.Team == Teams.Villagers).ToList();
        IList<GamePlayer> wolves = Game.Players.Where(p => p.CurrentCard.Team == Teams.Werewolves).ToList();

        // Villagers win if they vote out at least one werewolf or if nobody dies and no werewolves are present
        bool isCircleVote = votedOutPlayers.Count <= 0;
        IList<GamePlayer> winners = wwVoted || (!wwPresent && isCircleVote) ? villagers : wolves;

        Game.Result = new GameResult(wwVoted, winners);

        BroadcastEvent(Game.Result.Winners.Any()
            ? $"The winners are {string.Join(", ", winners.Select(w => $"{w.Name} ({w.CurrentCard})"))}"
            : "No players won.");
    }

    private readonly List<GamePlayer> votedOutPlayers = new();

    private void TabulateVotes()
    {
        int maxVotes = votes.Values.Max();

        // In the case where people voted in a circle, nobody dies since all have only 1 vote.
        if (maxVotes > 1)
        {
            votedOutPlayers.AddRange(votes.Where(kvp => kvp.Value == maxVotes).Select(kvp => kvp.Key));
        }

        foreach (GamePlayer votedPlayer in votedOutPlayers)
        {
            BroadcastEvent(new VotedOutEvent(votedPlayer));
        }
    }

    private void CollectVote(GamePlayer player)
    {
        GamePlayer votedPlayer = player.DetermineVoteTarget(Game.Randomizer);

        VotedEvent votedEvent = new(player, votedPlayer);
        BroadcastEvent(votedEvent);

        votes[votedPlayer] += 1;
    }
}