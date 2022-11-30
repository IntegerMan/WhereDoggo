using System.Windows.Media;
using MattEland.WhereDoggo.Core.Events.Claims;
using MattEland.WhereDoggo.WPFClient.Helpers;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class EventViewModel : ViewModelBase
{
    private readonly GameEventBase _sourceEvent;
    private readonly GameViewModel _mainVm;

    public EventViewModel(GameEventBase sourceEvent, GameViewModel mainVM)
    {
        _sourceEvent = sourceEvent;
        _mainVm = mainVM;
    }

    public string Tooltip => $"({Phase}) {Text}";
    public string Type => _sourceEvent.GetType().Name;
    public string? Text => _sourceEvent.ToString();
    
    public string? SuffixText
    {
        get
        {
            if (_sourceEvent is ClaimBase {IsLie: true})
            {
                if (_mainVm.SelectedPerspective == _sourceEvent?.Player?.Name || 
                    _mainVm.SelectedPerspective == GameViewModel.StorytellerName)
                {
                    return " (Lie)";
                }
            }
            
            return string.Empty;
        }
    }
    
    public bool ShowSuffix => !string.IsNullOrWhiteSpace(SuffixText);

    public string Phase => _sourceEvent.Phase ?? "Unknown Phase";
    public FontWeight FontWeight => _sourceEvent is (TextEvent or VotedOutEvent) ? FontWeights.Bold : FontWeights.Normal;

    public Brush Foreground
    {
        get
        {
            if (_sourceEvent.IsDeductiveEvent) return Brushes.DimGray;
            
            return _sourceEvent switch
            {
                DealtRoleEvent dre => BrushHelpers.GetTeamBrush(dre.Role.Team),
                _ => Phase switch
                {
                    "Setup" => Brushes.DarkGreen,
                    "Night" => Brushes.DarkBlue,
                    "Day" => Brushes.DeepPink,
                    "Voting" => Brushes.Red,
                    _ => Brushes.BlueViolet
                }
            };
        }
    }

    public string MainIcon => _sourceEvent switch
        {
            DealtRoleEvent dre => IconHelpers.GetRoleIcon(dre.Role.RoleType),
            KnowsRoleEvent => "Solid_Eye",
            SawNotRoleEvent => "Solid_EyeSlash",
            SkippedNightActionClaim => "Solid_Ban",
            ObservedCenterCardEvent => "Solid_Eye",
            ObservedPlayerCardEvent => "Solid_Eye",
            VotedOutEvent => "Solid_UserXMark",
            DeferredClaimingRoleEvent => "Solid_Clock",
            OnlyWolfEvent => IconHelpers.GetRoleIcon(RoleTypes.Werewolf),
            OnlyMasonEvent => IconHelpers.GetRoleIcon(RoleTypes.Mason),
            InsomniacSawOwnCardEvent => IconHelpers.GetRoleIcon(RoleTypes.Insomniac),
            SawAsWerewolfEvent => IconHelpers.GetRoleIcon(RoleTypes.Werewolf),
            ClaimedRoleEvent cre => IconHelpers.GetRoleIcon(cre.ClaimedRole),
            VotedEvent => IconHelpers.GetPhaseIcon(Phase),
            WokeUpEvent => IconHelpers.GetPhaseIcon(Phase),
            TextEvent => IconHelpers.GetPhaseIcon(Phase),
            _ => _sourceEvent.Player!.ClaimedRole != null 
                ? IconHelpers.GetRoleIcon(_sourceEvent.Player!.ClaimedRole)
                : "Solid_Question",
        };

    public bool ShowText => true;
    public bool IsUnknownEventType => !ShowText;
}