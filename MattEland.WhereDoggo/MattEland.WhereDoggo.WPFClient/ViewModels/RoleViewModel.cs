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
}