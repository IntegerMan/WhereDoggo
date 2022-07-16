namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a player within the game world.
/// </summary>
[DebuggerDisplay("{Name} ({CurrentRole})")]
public class GamePlayer : CardContainer
{
    private readonly Game _game;
    private readonly List<GameEventBase> _events = new();

    /// <summary>
    /// Instantiates an instance of the <see cref="GamePlayer"/> class.
    /// </summary>
    /// <param name="name">The name of the player</param>
    /// <param name="playerNumber">The player number. Used for some abilities and calculating adjacency</param>
    /// <param name="initialRole">The role they were initially dealt</param>
    /// <param name="game">The game the player is in</param>
    /// <param name="randomizer">The randomizer instance</param>
    public GamePlayer(string name, int playerNumber, RoleBase initialRole, Game game, Random randomizer) : base(name, initialRole)
    {
        _game = game;
        Number = playerNumber;
        Strategies = new GameStrategies(randomizer);
        Brain = new PlayerInferenceEngine(this, game);
    }

    /// <summary>
    /// Adds a new event to the game log
    /// </summary>
    /// <param name="eventBase">The event to add</param>
    public void LogEvent(GameEventBase eventBase) => _events.Add(eventBase);

    /// <summary>
    /// Gets all events for the player
    /// </summary>
    public IEnumerable<GameEventBase> Events => _events.AsReadOnly();
    
    /// <summary>
    /// Gets the <see cref="PlayerInferenceEngine"/> associated with the player.
    /// </summary>
    public PlayerInferenceEngine Brain { get; }

    /// <summary>
    /// Whether or not the sentinel token has been placed on the card
    /// </summary>
    public bool HasSentinelToken { get; set;  }

    /// <summary>
    /// The strategies the player uses for various roles.
    /// </summary>
    public GameStrategies Strategies { get; }

    /// <summary>
    /// The player number
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Returns which player the player wants to vote for
    /// </summary>
    /// <param name="random">The randomizer used to break ties when the player is split</param>
    /// <returns>The player to vote for</returns>
    public GamePlayer DetermineVoteTarget(Random random)
    {
        IDictionary<CardContainer, CardProbabilities> probabilities = Brain.BuildFinalRoleProbabilities();

        // Try to figure out which team the player is on
        Teams probableTeams = probabilities[this].ProbableTeam;

        // Remove the player from the set of probabilities since self-voting is illegal
        probabilities.Remove(this);

        List<CardContainer> keys = probabilities.Keys.Where(k => k is CenterCardSlot).ToList();
        foreach (CardContainer key in keys)
        {
            probabilities.Remove(key);
        }

        List<CardContainer> options;
        switch (probableTeams)
        {
            case Teams.Villagers:
            {
                decimal max = probabilities.Values.Max(p => p.CalculateTeamProbability(Teams.Werewolves));
                options = probabilities.Where(kvp => kvp.Value.CalculateTeamProbability(Teams.Werewolves) == max)
                    .Select(kvp => kvp.Key)
                    .ToList();
                break;
            }
            case Teams.Werewolves:
            {
                decimal max = probabilities.Values.Max(p => p.CalculateTeamProbability(Teams.Villagers));
                options = probabilities.Where(kvp => kvp.Value.CalculateTeamProbability(Teams.Villagers) == max)
                    .Select(kvp => kvp.Key)
                    .ToList();
                break;
            }
            default:
                options = probabilities.Select(p => p.Key).ToList();
                break;
        }

        return (GamePlayer) options.GetRandomElement(random)!;
    }

    /// <summary>
    /// Occurs when a player wakes up - either in the night or in the morning
    /// </summary>
    public void Wake()
    {
        _game.LogEvent(new WokeUpEvent(_game.CurrentPhase, this));

        // Allow for players to observe sentinel tokens
        foreach (GamePlayer player in _game.Players)
        {
            if (!player.HasSentinelToken) continue;

            if (!Events.Any(e => e is SentinelTokenObservedEvent sto && sto.Target == player))
            {
                _game.LogEvent(new SentinelTokenObservedEvent(this, player, _game.CurrentPhase));
            }
        }
        
        // Allow for players to observe revealed roles
        foreach (CardContainer card in _game.Entities)
        {
            if (!card.IsRevealed) continue;
            
            if (!Events.Any(e => e is RevealedRoleObservedEvent obs && obs.Target == card))
            {
                _game.LogEvent(new RevealedRoleObservedEvent(_game.CurrentPhase, this, card));
            }
        }
    }
}