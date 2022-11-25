using System.Windows.Media;
using MattEland.Util;
using MattEland.WhereDoggo.WPFClient.Helpers;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class CardViewModel : ViewModelBase
{
    private readonly IHasCard _card;
    private readonly MainWindowViewModel _mainVM;
    private readonly IDictionary<IHasCard, CardProbabilities>? probabilities;

    public CardViewModel(IHasCard card, MainWindowViewModel mainVM)
    {
        _card = card;
        _mainVM = mainVM;

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
    }

    public ObservableCollection<RoleProbabilityViewModel> RoleProbabilities { get; } = new();

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

    public Brush TeamForeground => ShowValue 
            ? BrushHelpers.GetTeamBrush(Team) 
            : BrushHelpers.GetTeamBrush(null);

    public Brush Background => ShowValue
            ? BrushHelpers.GetTeamBackgroundBrush(Team)
            : BrushHelpers.GetTeamBackgroundBrush(null);

    public override string ToString() => CardName;
}