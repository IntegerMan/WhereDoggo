using System.Windows.Input;

namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public const string StorytellerName = "Storyteller";
    private Game _game;
    private readonly ObservableCollection<CardViewModel> _centerCards = new();
    private readonly ObservableCollection<CardViewModel> _playerCards = new();
    private readonly ObservableCollection<string> _perspectives = new();
    private readonly ObservableCollection<EventViewModel> _events = new();
    private string _selectedPerspective = StorytellerName;
    private bool _showDeductiveEvents;

    public MainWindowViewModel()
    {
        _game = new(new List<RoleTypes>());
        NewGame();
    }

    private Game SetupGame()
    {
        RoleTypes[] assignedRoles =
        {
            RoleTypes.Mason,
            RoleTypes.Mason,
            RoleTypes.Werewolf,
            RoleTypes.MysticWolf,
            RoleTypes.Villager,
            RoleTypes.ApprenticeSeer,
            RoleTypes.Insomniac,
        };

        GameOptions options = new()
        {
            NumPlayers = 4
        };

        _game = new Game(assignedRoles, options);
        _game.RunNextPhase(); // Move to the setup phase

        // Update the perspective
        _perspectives.Clear();
        _perspectives.Add(StorytellerName);
        foreach (var player in _game.Players)
        {
            _perspectives.Add(player.Name);
        }
        if (!_perspectives.Contains(SelectedPerspective))
        {
            SelectedPerspective = StorytellerName;
        }

        NotifyGamePropertiesChanged();

        return _game;
    }

    private void NotifyGamePropertiesChanged()
    {
        NotifyPropertyChanged(nameof(Events));
        NotifyPropertyChanged(nameof(CenterCards));
        NotifyPropertyChanged(nameof(PlayerCards));
        NotifyPropertyChanged(nameof(Perspectives));
        NotifyPropertyChanged(nameof(SelectedPerspective));
        NotifyPropertyChanged(nameof(Roles));
    }

    private void ObserveGameEvents()
    {
        GamePlayer? perspective = _game.Players.FirstOrDefault(p => p.Name == SelectedPerspective);
        
        _centerCards.Clear();
        foreach (CenterCardSlot slot in _game.CenterSlots)
        {
            _centerCards.Add(new CardViewModel(slot, this));
        }

        _playerCards.Clear();
        foreach (GamePlayer player in _game.Players)
        {
            _playerCards.Add(new CardViewModel(player, this));
        }

        _events.Clear();
        foreach (GameEventBase evt in perspective?.Events ?? _game.Events)
        {
            if (evt.IsDeductiveEvent && !ShowDeductiveEvents)
            {
                continue;
            }
            
            _events.Add(new EventViewModel(evt, this));
        }
    }

    public ObservableCollection<EventViewModel> Events => _events;
    public ObservableCollection<CardViewModel> CenterCards => _centerCards;
    public ObservableCollection<CardViewModel> PlayerCards => _playerCards;
    public IEnumerable<RoleViewModel> Roles => _game.Roles.OrderBy(r => r.NightActionOrder).Select(r => new RoleViewModel(r.RoleType));

    public ObservableCollection<string> Perspectives => _perspectives;

    public bool ShowDeductiveEvents
    {
        get => _showDeductiveEvents;
        set
        {
            if (_showDeductiveEvents != value)
            {
                _showDeductiveEvents = value;
                
                NotifyPropertyChanged();
                ObserveGameEvents();

                NotifyGamePropertiesChanged();
            }
        }
    }

    public ICommand NewGameCommand => new RelayCommand(NewGame);

    private void NewGame()
    {
        _game = SetupGame();

        // Simulate the entire game
        _game.Run();

        ObserveGameEvents();
    }

    public string SelectedPerspective
    {
        get => _selectedPerspective;
        set
        {
            if (_selectedPerspective != value)
            {
                _selectedPerspective = value;
                ObserveGameEvents();
                NotifyGamePropertiesChanged();
            }
        }
    }

    public Game Game => _game;
}