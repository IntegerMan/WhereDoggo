using System.Windows.Media;

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
    public string Icon => Role switch
    {
        RoleTypes.Werewolf => "Solid_Dog",
        RoleTypes.MysticWolf => "Solid_ArrowsToEye", // Or Solid_ShieldDog, Solid_Paw, Solid_Bone
        RoleTypes.Seer => "Solid_Eye",
        RoleTypes.ApprenticeSeer => "Solid_EyeLowVision",
        RoleTypes.Mason => "Solid_PeopleCarryBox", // or Solid_TrowelBricks
        RoleTypes.Villager => "Solid_Person",
        RoleTypes.Insomniac => "Solid_Bed",
        _ => "Solid_Question"
    };

    public Brush TeamForeground => Team == Teams.Villagers ? Brushes.Blue : Brushes.Red;

    public override string ToString() => CardName;
}