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
}