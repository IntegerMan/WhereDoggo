using System.Windows.Media;
using MattEland.WhereDoggo.WPFClient.Helpers;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class CardViewModel : ViewModelBase
{
    private readonly IHasCard _card;

    public CardViewModel(IHasCard card)
    {
        this._card = card;
    }

    public string CardName => _card.Name;
    public RoleTypes Role => _card.CurrentCard.RoleType;
    public Teams Team => _card.CurrentCard.Team;
    public string Icon => IconHelpers.GetRoleIcon(Role);

    public Brush TeamForeground => BrushHelpers.GetTeamBrush(Team);

    public override string ToString() => CardName;
}