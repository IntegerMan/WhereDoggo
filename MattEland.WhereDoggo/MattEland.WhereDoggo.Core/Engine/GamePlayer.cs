using MattEland.WhereDoggo.Core.Engine.Strategies;

namespace MattEland.WhereDoggo.Core.Engine;

public class GamePlayer : RoleContainerBase
{
    private readonly List<GameEventBase> _events = new();

    public GamePlayer(string name, GameRoleBase initialRole, Random random) : base(name, initialRole)
    {
        this.LoneWolfSlotSelectionStrategy = new RandomLoneWolfSlotSelectionStrategy(random);
    }

    public void AddEvent(GameEventBase eventBase)
    {
        this._events.Add(eventBase);
    }

    public IList<GameEventBase> Events => _events.AsReadOnly();
    public bool StartedAsDoggo => InitialRole.IsDoggo;
    public GameInferenceEngine Brain { get; } = new();
    public LoneWolfCardSelectionStrategyBase LoneWolfSlotSelectionStrategy { get; set; }

    public GamePlayer DetermineVoteTarget(OneNightWhereDoggoGame game, Random random)
    {
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            Brain.BuildFinalRoleProbabilities(this, game);

        // Try to figure out which team the player is on
        bool isVillage = true;
        if (probabilities[this].BelievedToBeWerewolf)
        {
            isVillage = false;
        }

        // Remove the player from the set of probabilities since self-voting is illegal
        probabilities.Remove(this);

        List<RoleContainerBase> keys = probabilities.Keys.Where(k => k is RoleSlot).ToList();
        foreach (RoleContainerBase key in keys)
        {
            probabilities.Remove(key);
        }

        List<RoleContainerBase> options = new List<RoleContainerBase>();
        if (isVillage)
        {
            decimal max = probabilities.Values.Max(p => p.ProbabilityDoggo);
            options = probabilities.Where(kvp => kvp.Value.ProbabilityDoggo == max)
                .Select(kvp => kvp.Key)
                .ToList();
        }
        else
        {
            decimal max = probabilities.Values.Max(p => p.ProbabilityRabbit);
            options = probabilities.Where(kvp => kvp.Value.ProbabilityRabbit == max)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        return (GamePlayer) options.GetRandomElement(random)!;
    }
}