using System.Diagnostics;

namespace MattEland.WhereDoggo.Core.Gamespace;

/// <summary>
/// Represents a player within the game world.
/// </summary>
[DebuggerDisplay("{Name} ({CurrentRole})")]
public class GamePlayer : RoleContainerBase
{
    private readonly Game _game;
    private readonly List<GameEventBase> _events = new();

    public GamePlayer(string name, RoleBase initialRole, Game game, Random randomizer) : base(name, initialRole)
    {
        _game = game;
        Strategies = new GameStrategies(randomizer, this);
        Brain = new PlayerInferenceEngine(this, game);
    }

    public void AddEvent(GameEventBase eventBase) => _events.Add(eventBase);

    public IList<GameEventBase> Events => _events.AsReadOnly();
    public PlayerInferenceEngine Brain { get; }
    public Teams CurrentTeam => CurrentRole.Team;
    public Teams InitialTeam => InitialRole.Team;

    /// <summary>
    /// Whether or not the sentinel token has been placed on the card
    /// </summary>
    public bool HasSentinelToken { get; set;  }

    public GameStrategies Strategies { get; }

    /// <summary>
    /// Returns which player the player wants to vote for
    /// </summary>
    /// <param name="random">The randomizer used to break ties when the player is split</param>
    /// <returns>The player to vote for</returns>
    public GamePlayer DetermineVoteTarget(Random random)
    {
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = Brain.BuildFinalRoleProbabilities();

        // Try to figure out which team the player is on
        Teams probableTeams = probabilities[this].ProbableTeams;

        // Remove the player from the set of probabilities since self-voting is illegal
        probabilities.Remove(this);

        List<RoleContainerBase> keys = probabilities.Keys.Where(k => k is CenterCardSlot).ToList();
        foreach (RoleContainerBase key in keys)
        {
            probabilities.Remove(key);
        }

        List<RoleContainerBase> options;
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
        foreach (GamePlayer player in _game.Players)
        {
            if (!player.IsRevealed) continue;
            
            if (!Events.Any(e => e is KnowsRoleEvent kre && kre.Target == player))
            {
                _game.LogEvent(new KnowsRoleEvent(_game.CurrentPhase, this, player));
            }
        }
    }
}