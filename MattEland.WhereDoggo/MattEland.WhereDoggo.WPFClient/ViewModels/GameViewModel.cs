namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class GameViewModel : ViewModelBase
{
    public const string StorytellerName = "Storyteller";
    private Game _game;
    private readonly ObservableCollection<CardViewModel> _centerCards = new();
    private readonly ObservableCollection<CardViewModel> _playerCards = new();
    private readonly ObservableCollection<string> _perspectives = new();
    private readonly ObservableCollection<EventViewModel> _events = new();
    private string? _selectedPerspective = null;
    private bool _showDeductiveEvents;
    private bool _showProbabilities;

    public GameViewModel()
    {
        _game = new Game(new List<RoleTypes>());
        NextCommand = new RelayCommand(Next);
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
        if (_perspectives.Count == 0)
        {
            _perspectives.Add(StorytellerName);
            foreach (GamePlayer player in _game.Players)
            {
                _perspectives.Add(player.Name);
            }

            if (!_perspectives.Contains(SelectedPerspective!))
            {
                SelectedPerspective = _game.Players.First().Name;
            }
        }

        ObserveGameEvents();

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
        NotifyPropertyChanged(nameof(NextCommand));
        NextCommand.NotifyCanExecuteChanged();
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

    public RelayCommand NewGameCommand => new(NewGame);
    public RelayCommand NextCommand { get; private set; }

    private void Next()
    {
        _game.RunNextStep();

        ObserveGameEvents();
    }

    private void NewGame()
    {
        _game = SetupGame();
        NextCommand = new RelayCommand(Next, () => !_game.IsCompleted);

        ObserveGameEvents();
    }

    public string? SelectedPerspective
    {
        get => _selectedPerspective;
        set
        {
            if (_selectedPerspective != value && value != null)
            {
                _selectedPerspective = value;
                ObserveGameEvents();
                NotifyGamePropertiesChanged();
            }
        }
    }

    public Game Game => _game;

    public bool ShowProbabilities
    {
        get => _showProbabilities;
        set
        {
            if (_showProbabilities != value)
            {
                _showProbabilities = value;
                NotifyPropertyChanged();
                ObserveGameEvents();
                NotifyGamePropertiesChanged();
            }
        }
    }
}