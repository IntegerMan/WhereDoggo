using MattEland.WhereDoggo.Core.Events;
using MattEland.WhereDoggo.Core.Roles;
using MattEland.WhereDoggo.Core.Strategies;

namespace MattEland.WhereDoggo.Core.Gamespace;

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
    public GameInferenceEngine Brain { get; } = new();
    public LoneWolfCardSelectionStrategyBase LoneWolfSlotSelectionStrategy { get; set; }
    public Teams CurrentTeam => CurrentRole.Team;
    public Teams InitialTeam => InitialRole.Team;

    public GamePlayer DetermineVoteTarget(OneNightWhereDoggoGame game, Random random)
    {
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            Brain.BuildFinalRoleProbabilities(this, game);

        // Try to figure out which team the player is on
        Teams probableTeams = probabilities[this].ProbableTeams;

        // Remove the player from the set of probabilities since self-voting is illegal
        probabilities.Remove(this);

        List<RoleContainerBase> keys = probabilities.Keys.Where(k => k is RoleSlot).ToList();
        foreach (RoleContainerBase key in keys)
        {
            probabilities.Remove(key);
        }

        List<RoleContainerBase> options = new();
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
}