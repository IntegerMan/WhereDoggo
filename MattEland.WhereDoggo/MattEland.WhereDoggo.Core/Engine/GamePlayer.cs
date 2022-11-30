using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a player within the game world.
/// </summary>
[DebuggerDisplay("{Name} ({CurrentCard})")]
public class GamePlayer : IHasCard
{
    private readonly Game _game;
    private readonly List<GameEventBase> _events = new();

    /// <summary>
    /// Instantiates an instance of the <see cref="GamePlayer"/> class.
    /// </summary>
    /// <param name="name">The name of the player</param>
    /// <param name="playerNumber">The player number. Used for some abilities and calculating adjacency</param>
    /// <param name="initialCard">The role they were initially dealt</param>
    /// <param name="game">The game the player is in</param>
    public GamePlayer(string name, int playerNumber, CardBase initialCard, Game game)
    {
        _game = game;
        Name = name;
        Number = playerNumber;
        Random random = game.Randomizer;
        InitialCard = initialCard;
        CurrentCard = initialCard;
        PickSingleCard = (targets) => targets.MinBy(_ => random.Next() * random.Next());
        PickSeerCards = (_, slots) => slots.OrderBy(_ => random.Next() * random.Next()).Take(2).ToList();
        Brain = new PlayerInferenceEngine(this, game);
    }

    /// <summary>
    /// The strategy to use when selecting a single card from multiple. Applies to multiple roles
    /// </summary>
    public Func<IEnumerable<IHasCard>, IHasCard?> PickSingleCard { get; set; }

    /// <summary>
    /// The function to use when picking cards for the <see cref="SeerRole"/>. A seer-specific function is required
    /// because the seer gets the choice to skip, pick one card from a player, or pick two cards from the center.
    /// </summary>
    public Func<IEnumerable<IHasCard>, IEnumerable<IHasCard>, List<IHasCard>> PickSeerCards { get; set; }

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
    /// Gets all events that stem from this player
    /// </summary>
    public IEnumerable<GameEventBase> OwnEvents => Events.Where(e => e.Player == this);

    /// <summary>
    /// Gets the <see cref="PlayerInferenceEngine"/> associated with the player.
    /// </summary>
    public PlayerInferenceEngine Brain { get; }

    /// <summary>
    /// Whether or not the sentinel token has been placed on the card
    /// </summary>
    public bool HasSentinelToken { get; set; }

    /// <inheritdoc />
    public string Name { get; }

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
        IDictionary<IHasCard, CardProbabilities> probabilities = Brain.BuildFinalRoleProbabilities();

        // Try to figure out which team the player is on
        Teams probableTeams = probabilities[this].ProbableTeam;

        // Remove the player from the set of probabilities since self-voting is illegal
        probabilities.Remove(this);

        List<IHasCard> keys = probabilities.Keys.Where(k => k is CenterCardSlot).ToList();
        foreach (IHasCard key in keys)
        {
            probabilities.Remove(key);
        }

        List<IHasCard> options;
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

        return (GamePlayer)options.GetRandomElement(random)!;
    }

    /// <summary>
    /// Occurs when a player wakes up - either in the night or in the morning
    /// </summary>
    public void Wake()
    {
        _game.LogEvent(new WokeUpEvent(this));
    }

    /// <summary>
    /// Has the player observe the visible tokens and face-up cards in the game area
    /// </summary>
    public void ObserveVisibleState()
    {
        // Allow for players to observe sentinel tokens
        foreach (GamePlayer player in _game.Players)
        {
            if (!player.HasSentinelToken)
            {
                continue;
            }

            if (!Events.Any(e => e is SentinelTokenObservedEvent sto && sto.Target == player))
            {
                _game.LogEvent(new SentinelTokenObservedEvent(this, player));
            }
        }

        // Allow for players to observe revealed roles
        foreach (IHasCard holder in _game.Entities)
        {
            if (!holder.CurrentCard.IsRevealed)
            {
                continue;
            }

            if (!Events.Any(e => e is RevealedRoleObservedEvent obs && obs.Target == holder))
            {
                _game.LogEvent(new RevealedRoleObservedEvent(this, holder));
            }
        }
    }

    /// <inheritdoc />
    public CardBase InitialCard { get; }

    /// <inheritdoc />
    public CardBase CurrentCard { get; set; }

    /// <summary>
    /// Represents the last role this player has publicly claimed.
    /// </summary>
    public RoleTypes? ClaimedRole => Events.OfType<ClaimedRoleEvent>().Where(cre => cre.Player == this).LastOrDefault()?.ClaimedRole;

    /// <inheritdoc />
    public override string ToString() => $"{Name}";

    /// <summary>
    /// Gets the role that the player wants to claim, or null if they don't want to claim a role
    /// </summary>
    /// <returns>The role claimed, or null</returns>
    public IEnumerable<ClaimBase> GetInitialRoleClaims()
    {
        switch (InitialCard.Team)
        {
            case Teams.Villagers:
                {
                    foreach (ClaimBase claim in InitialCard.GetClaims(this))
                    {
                        yield return claim;
                    }
                }
                break;

            case Teams.Werewolves:
                {
                    RoleTypes? claim = Brain.DetermineBestSafeRoleClaim();
                    if (claim != null)
                    {
                        yield return new ClaimedRoleEvent(this, claim.Value);
                    }
                    else
                    {
                        yield return new DeferredClaimingRoleEvent(this);
                    }
                }
                break;

            default:
                throw new NotSupportedException($"Team {InitialCard.Team} is not supported for claiming roles");
        }
    }

    /// <summary>
    /// Gets the final role that the player wants to claim
    /// </summary>
    /// <returns>The role claimed</returns>
    public IEnumerable<ClaimBase> GetFinalRoleClaims()
    {
        switch (InitialCard.Team)
        {
            case Teams.Villagers:
                yield return new ClaimedRoleEvent(this, InitialCard.RoleType);
                break;

            case Teams.Werewolves:
                yield return new ClaimedRoleEvent(this, Brain.DetermineBestRoleClaim());
                break;

            default:
                throw new NotSupportedException($"Team {InitialCard.Team} is not supported for claiming roles");
        }
    }
}