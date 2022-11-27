using System.Windows.Media;
using MattEland.Util;
using MattEland.WhereDoggo.WPFClient.Helpers;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class PlayerNameViewModel : ViewModelBase
{
    private readonly GameViewModel _mainVM;
    private readonly string _name;

    public PlayerNameViewModel(string name, GameViewModel mainVM)
    {
        _name = name;
        _mainVM = mainVM;
    }

    public bool IsSelected => _mainVM.SelectedPerspective == _name;
    public FontWeight Weight => IsSelected ? FontWeights.Bold : FontWeights.Normal;

    public string Name => _name;

    public override string ToString() => _name;

    public string Tooltip => IsSelected
        ? $"You are viewing the game from {Name}'s perspective"
        : $"Click to view the game from {Name}'s perspective";

    public RelayCommand ClickCommand => new(() => _mainVM.SelectedPerspective = _name);
}