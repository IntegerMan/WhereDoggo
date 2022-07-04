using MattEland.WhereDoggo.Core.Engine;

namespace MattEland.WhereDoggo.Core.OneNight;

public sealed class OneNightWhereDoggoGame : GameBase
{
    private readonly Random _random = new();
    private readonly List<GameRoleBase> _centerRoles = new(NumCenterCards);

    public const int NumCenterCards = 3;

    public OneNightWhereDoggoGame(int numPlayers) : base(numPlayers)
    {
    }

    public IList<GameRoleBase> CenterRoles => _centerRoles;
    public override string Name => "One Night Ultimate Where Doggo?";

    public override List<GamePlayer> LoadPlayers(int numPlayers)
    {
        if (numPlayers is < 3 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(numPlayers), "Must have between 3 and 5 players");
        }

        if (Roles.Count < numPlayers)
        {
            throw new InvalidOperationException("Tried to load players but the number of players was less than the number of roles");
        }

        List<GameRoleBase> roles = this.Roles.OrderBy(r => _random.Next() + _random.Next() + _random.Next()).ToList();

        string[] playerNames = {"Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane"};

        List<GamePlayer> players = new(numPlayers);

        for (int i = 0; i < roles.Count; i++)
        {
            if (i < numPlayers)
            {
                players.Add(new GamePlayer(playerNames[i], roles[i]));
            }
            else
            {
                _centerRoles.Add(roles[i]);
            }
        }

        return players;
    }

    public override List<GameRoleBase> LoadRoles(int numPlayers)
    {
        if (numPlayers is < 3 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(numPlayers), "Must have between 3 and 5 players");
        }

        List<GameRoleBase> roles = new();

        const int numDoggos = 2;

        for (int i = 0; i < numDoggos; i++)
        {
            roles.Add(new DoggoRole());
        }
        for (int i = 0; i < numPlayers - numDoggos + NumCenterCards; i++)
        {
            roles.Add(new RabbitRole());
        }

        return roles;
    }

}