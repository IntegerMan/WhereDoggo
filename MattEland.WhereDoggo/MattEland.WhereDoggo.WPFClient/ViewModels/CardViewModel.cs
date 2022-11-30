using System.Windows.Media;
using MattEland.Util;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class CardViewModel : ViewModelBase
{
    private readonly IHasCard _card;
    private readonly GameViewModel _mainVM;
    private readonly IDictionary<IHasCard, CardProbabilities>? probabilities;

    public CardViewModel(IHasCard card, GameViewModel mainVM)
    {
        _card = card;
        _mainVM = mainVM;
        PlayerNameVM = new PlayerNameViewModel(card.Name, mainVM);

        GamePlayer? player = _mainVM.Game.Players.FirstOrDefault(p => p.Name == _mainVM.SelectedPerspective);

        probabilities = player?.Brain.BuildFinalRoleProbabilities();
        Probability = probabilities?[_card];

        if (Probability != null)
        {
            foreach (KeyValuePair<RoleTypes, decimal> kvp in Probability.Probabilities.OrderByDescending(kvp => kvp.Value))
            {
                if (kvp.Value > 0)
                {
                    RoleProbabilities.Add(new RoleProbabilityViewModel(kvp.Key, kvp.Value));
                }
            }
        }

        foreach (VotedEvent vote in mainVM.Game.Events.OfType<VotedEvent>().Where(ve => ve.Target == card))
        {
            if (vote.Player?.Name != null)
            {
                VotedBy.Add(vote.Player.Name);
            }
        }
    }

    public ObservableCollection<RoleProbabilityViewModel> RoleProbabilities { get; } = new();

    public bool IsPlayer => _card is GamePlayer;
    public string CardName => _card.Name;
    public RoleTypes Role => _card.CurrentCard.RoleType;

    public string Text => ShowValue 
        ? Role.GetFriendlyName() 
        : "Unknown";

    public Teams Team => _card.CurrentCard.Team;
    public string Icon => ShowValue
            ? IconHelpers.GetRoleIcon(Role)
            : IconHelpers.GetRoleIcon(null);

    public bool ShowProbabilities => _mainVM.ShowProbabilities && Probability != null;

    public bool ShowValue
    {
        get
        {
            CardProbabilities? probability = Probability;

            return probability == null || probability.IsCertain;
        }
    }

    public CardProbabilities? Probability { get; }

    public Brush TeamForeground
    {
        get
        {
            if (IsDead)
            {
                switch (Team)
                {
                    case Teams.Villagers:
                        return Brushes.White;

                    case Teams.Werewolves:
                        return BrushHelpers.GetTeamBrush(Team);
                }
            }

            if (Probability is {IsTeamCertain: true})
            {
                return BrushHelpers.GetTeamBrush(Probability.ProbableTeam);
            }

            return ShowValue
                ? BrushHelpers.GetTeamBrush(Team)
                : BrushHelpers.GetTeamBrush(null);
        }
    }

    public bool IsDead => _mainVM.Game.Events.OfType<VotedOutEvent>().Any(voe => voe.Player == _card);

    public Brush Background
    {
        get
        {
            if (IsDead)
            {
                return Brushes.Black;
            }

            if (Probability != null && Probability.IsTeamCertain)
            {
                return BrushHelpers.GetTeamBackgroundBrush(Probability.ProbableTeam);
            }

            return ShowValue
                ? BrushHelpers.GetTeamBackgroundBrush(Team)
                : BrushHelpers.GetTeamBackgroundBrush(null);
        }
    }

    public ObservableCollection<string> VotedBy { get; } = new();

    public PlayerNameViewModel PlayerNameVM { get; }
    public override string ToString() => CardName;
}