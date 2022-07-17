﻿namespace MattEland.WhereDoggo.Core.Engine;

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
    /// Gets the <see cref="PlayerInferenceEngine"/> associated with the player.
    /// </summary>
    public PlayerInferenceEngine Brain { get; }

    /// <summary>
    /// Whether or not the sentinel token has been placed on the card
    /// </summary>
    public bool HasSentinelToken { get; set;  }

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

        return (GamePlayer) options.GetRandomElement(random)!;
    }

    /// <summary>
    /// Occurs when a player wakes up - either in the night or in the morning
    /// </summary>
    public void Wake()
    {
        _game.LogEvent(new WokeUpEvent(this));

        // Allow for players to observe sentinel tokens
        foreach (GamePlayer player in _game.Players)
        {
            if (!player.HasSentinelToken) continue;

            if (!Events.Any(e => e is SentinelTokenObservedEvent sto && sto.Target == player))
            {
                _game.LogEvent(new SentinelTokenObservedEvent(this, player));
            }
        }
        
        // Allow for players to observe revealed roles
        foreach (IHasCard holder in _game.Entities)
        {
            if (!holder.CurrentCard.IsRevealed) continue;
            
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
}