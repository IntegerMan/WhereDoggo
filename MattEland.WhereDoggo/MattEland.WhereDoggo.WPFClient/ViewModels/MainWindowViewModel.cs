namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Game game;
    private readonly ObservableCollection<CardViewModel> centerCards = new();
    private readonly ObservableCollection<CardViewModel> playerCards = new();
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

        NotifyGamePropertiesChanged();

        return game;
    }

    private void NotifyGamePropertiesChanged()
    {
        NotifyPropertyChanged(nameof(Log));
        NotifyPropertyChanged(nameof(CenterCards));
        NotifyPropertyChanged(nameof(PlayerCards));
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

    public string Title => "Where Doggo by Matt Eland (@IntegerMan)";

    public ObservableCollection<string> Log => events;

    public ObservableCollection<CardViewModel> CenterCards => centerCards;
    public ObservableCollection<CardViewModel> PlayerCards => playerCards;
}
