using System.Windows.Media;
using MattEland.Util;
using MattEland.WhereDoggo.WPFClient.Helpers;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class CardViewModel : ViewModelBase
{
    private readonly IHasCard _card;
    private readonly MainWindowViewModel _mainVm;

    public CardViewModel(IHasCard card, MainWindowViewModel mainVM)
    {
        this._card = card;
        _mainVm = mainVM;
    }

    public string CardName => _card.Name;
    public RoleTypes Role => _card.CurrentCard.RoleType;

    public string Text => ShowValue 
        ? Role.GetFriendlyName() 
        : "Unknown";

    public Teams Team => _card.CurrentCard.Team;
    public string Icon
    {
        get
        {
            return ShowValue
                ? IconHelpers.GetRoleIcon(Role)
                : IconHelpers.GetRoleIcon(null);
        }
    }

    public bool ShowValue
    {
        get
        {
            if (_mainVm.SelectedPerspective == MainWindowViewModel.StorytellerName)
            {
                return true;
            }

            GamePlayer? player = _mainVm.Game.Players.First(p => p.Name == _mainVm.SelectedPerspective);

            IDictionary<IHasCard, CardProbabilities> probs = player.Brain.BuildFinalRoleProbabilities();

            return probs[_card].IsCertain;
        }
    }

    public Brush TeamForeground
    {
        get
        {
            return ShowValue 
                ? BrushHelpers.GetTeamBrush(Team) 
                : BrushHelpers.GetTeamBrush(null);
        }
    }

    public Brush Background
    {
        get
        {
            return ShowValue
                ? BrushHelpers.GetTeamBackgroundBrush(Team)
                : BrushHelpers.GetTeamBackgroundBrush(null);
        }
    }

    public override string ToString() => CardName;
}