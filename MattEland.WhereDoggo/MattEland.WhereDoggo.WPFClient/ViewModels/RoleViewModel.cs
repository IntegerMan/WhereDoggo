using System.Diagnostics;
using System.Windows.Media;
using MattEland.Util;
using MattEland.WhereDoggo.WPFClient.Helpers;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class RoleViewModel : ViewModelBase
{
    private readonly RoleTypes _role;

    public RoleViewModel(RoleTypes role)
    {
        _role = role;
    }

    public string Text => _role.GetFriendlyName();

    public string Icon => IconHelpers.GetRoleIcon(_role);

    public Brush Foreground => BrushHelpers.GetTeamBrush(_role.DetermineTeam());

    public string ToolTip => _role switch
        {
            RoleTypes.Werewolf =>
                "The werewolf wakes up in the night to see if there are any other members of the werewolf team. " + Environment.NewLine +
                "If they are the only one that wakes, they get to look at a card in the center. " + Environment.NewLine +
                "If the werewolf is voted out, the villagers win.",
            RoleTypes.MysticWolf =>
                "The mystic wolf wakes up in the night to see if there are any other members of the werewolf team. " + Environment.NewLine +
                "If they are the only one that wakes, they get to look at a card in the center. " + Environment.NewLine +
                "The mystic wolf gets to look at one other player's card. " + Environment.NewLine +
                "If the mystic wolf is voted out, the villagers win.",
            RoleTypes.ApprenticeSeer =>
                "The apprentice seer gets to look at one card in the center during their turn. They are on the villager team.",
            RoleTypes.Villager =>
                "The villager does not wake up in the night or have any special ability. They are on the villager team.",
            RoleTypes.Insomniac =>
                "The insomniac wakes up at the end of the night to view their own card to see if it changed. They are on the villager team.",
            RoleTypes.Mason =>
                "The masons wake up in the night to see if any other masons are in the game. They are on the villager team.",
            _ => $"No help available for the {_role.GetFriendlyName()} role"
        };
}