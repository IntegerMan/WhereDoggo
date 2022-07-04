using MattEland.WhereDoggo.Core.Engine;
using MattEland.WhereDoggo.Core.Engine.Events;

namespace MattEland.WhereDoggo.Core.OneNight;

public sealed class OneNightWhereDoggoGame : GameBase
{
    private readonly Random _random = new();

    public const int NumCenterCards = 3;

    public OneNightWhereDoggoGame(int numPlayers) : base(numPlayers)
    {
    }
    public override string Name => "One Night Ultimate Where Doggo?";

    public override List<RoleContainerBase> LoadRoleContainers(int numPlayers)
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

        List<RoleContainerBase> players = new(numPlayers + NumCenterCards);

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < numPlayers)
            {
                players.Add(new GamePlayer(playerNames[i], roles[i]));
            }
            else
            {
                players.Add(new RoleSlot("Center Card " + (centerIndex++), roles[i]));
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

    public void PerformNightPhase()
    {
        LogEvent(new TextEvent("Night Phase Starting"));

        List<GamePlayer> doggos = this.Players.Where(p => p.InitialRole.IsDoggo).ToList();

        switch (doggos.Count)
        {
            case 0:
                LogEvent(new TextEvent("No doggos awoke"));
                break;

            case 1:
                LogEvent(new OnlyDoggoEvent(doggos[0]));

                // TODO: Doggo should be able to look at a center card

                break;

            case > 1:
            {
                // Each doggo knows each other doggo is a doggo
                foreach (GamePlayer player in doggos)
                {
                    foreach (GamePlayer otherPlayer in doggos.Where(otherPlayer => otherPlayer != player))
                    {
                        LogEvent(new KnowsRoleEvent(player, otherPlayer, otherPlayer.CurrentRole));
                    }
                }

                break;
            }
        }

        LogEvent(new TextEvent("Night Phase Ending"));
    }
}