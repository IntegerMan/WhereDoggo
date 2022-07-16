namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// The setup phase is responsible for dealing individual roles to each player.
/// </summary>
public class SetupGamePhase : GamePhaseBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupGamePhase"/> class.
    /// </summary>
    /// <param name="game">The game.</param>
    public SetupGamePhase(Game game) : base(game)
    {
    }

    /// <inheritdoc />
    public override string Name => "Setup";

    /// <inheritdoc />
    public override void Run(Game game)
    {
        DealRolesToPlayersAndCenterSlots(game);

        HavePlayersLookAtInitialRoles(game);
    }

    private static void DealRolesToPlayersAndCenterSlots(Game game)
    {
        GameOptions options = game.Options;
        IList<RoleBase> roles = ShuffleRolesAsNeeded(game, options);

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < options.NumPlayers)
            {
                game.AddPlayer(new GamePlayer(options.PlayerNames[i], i + 1, roles[i], game));
            }
            else
            {
                game.AddCenterCard(new CenterCardSlot($"Center Card {centerIndex++}", roles[i]));
            }
        }
    }

    private static List<RoleBase> ShuffleRolesAsNeeded(Game game, GameOptions options)
    {
        IEnumerable<RoleBase> roles = game.Roles;

        if (options.RandomizeSlots)
        {
            roles = roles.OrderBy(r => game.Randomizer.Next() * game.Randomizer.Next()).ToList();
        }

        return roles.ToList();
    }

    private void HavePlayersLookAtInitialRoles(Game game)
    {
        foreach (GamePlayer player in game.Players)
        {
            LogEvent(new DealtRoleEvent(player, player.InitialRole));

            player.Brain.BuildInitialRoleProbabilities();
        }
    }
}