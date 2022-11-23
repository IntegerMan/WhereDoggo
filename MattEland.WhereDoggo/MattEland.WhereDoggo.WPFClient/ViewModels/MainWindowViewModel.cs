namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private const string StorytellerName = "Storyteller";
    private Game game;
    private readonly ObservableCollection<CardViewModel> centerCards = new();
    private readonly ObservableCollection<CardViewModel> playerCards = new();
    private readonly ObservableCollection<string> perspectives = new();
    private readonly ObservableCollection<string> events = new();

    public MainWindowViewModel()
    {
        game = SetupGame();

        // Simulate the entire game
        game.Run();

        ObserveGameEvents();
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

        game = new Game(assignedRoles, options);
        game.RunNextPhase(); // Move to the setup phase

        // Update the perspective
        perspectives.Clear();
        perspectives.Add(StorytellerName);
        foreach (var player in game.Players)
        {
            perspectives.Add(player.Name);
        }
        if (!perspectives.Contains(SelectedPerspective))
        {
            SelectedPerspective = StorytellerName;
        }

        NotifyGamePropertiesChanged();

        return game;
    }

    private void NotifyGamePropertiesChanged()
    {
        NotifyPropertyChanged(nameof(Log));
        NotifyPropertyChanged(nameof(CenterCards));
        NotifyPropertyChanged(nameof(PlayerCards));
        NotifyPropertyChanged(nameof(Perspectives));
        NotifyPropertyChanged(nameof(SelectedPerspective));
    }

    private void ObserveGameEvents()
    {
        centerCards.Clear();
        foreach (CenterCardSlot slot in game.CenterSlots)
        {
            centerCards.Add(new CardViewModel(slot));
        }

        playerCards.Clear();
        foreach (GamePlayer player in game.Players)
        {
            playerCards.Add(new CardViewModel(player));
        }

        events.Clear();

        foreach (GameEventBase evt in game.Events)
        {
            events.Add(evt.ToString()!);
        }
    }

    public ObservableCollection<string> Log => events;
    public ObservableCollection<CardViewModel> CenterCards => centerCards;
    public ObservableCollection<CardViewModel> PlayerCards => playerCards;

    public ObservableCollection<string> Perspectives => perspectives;

    public string SelectedPerspective { get; set; } = StorytellerName;
}
