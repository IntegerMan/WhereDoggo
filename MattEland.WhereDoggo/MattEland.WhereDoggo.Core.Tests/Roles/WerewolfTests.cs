using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

[Category("Roles")]
public class WerewolfTests : GameTestsBase
{
    [Test]
    public void WerewolvesShouldThinkTheyAreWerewolves()
    {
        // Arrange
        Game game = SetupLoneWolfGame();
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(1);
        probabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }

    [Test]
    public void LoneWolfWhoSeesAWolfWithOnlyVillagersShouldKnowAllRoles()
    {
        // Arrange
        Game game = SetupLoneWolfGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        game.Run();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (KeyValuePair<IHasCard, CardProbabilities> kvp in probabilities)
        {
            kvp.Value.IsCertain.ShouldBeTrue($"Was not certain of role {kvp.Value}");
        }
    }


    [Test]
    public void LoneWolfWhoLooksShouldHaveCorrectEvent()
    {
        // Arrange
        Game game = SetupLoneWolfGame();


        // Act
        game.Run();

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is OnlyWolfEvent);
        player.Events.ShouldContain(e => e is ObservedCenterCardEvent);
        player.Events.ShouldNotContain(e => e is SkippedNightActionEvent);
    }

    [Test]
    public void LoneWolfWhoSkipsShouldHaveCorrectEvent()
    {
        // Arrange
        Game game = SetupLoneWolfGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is OnlyWolfEvent);
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    private static Game SetupLoneWolfGame() =>
        CreateGame(new[] {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        });

    private static Game SetupTwoWolfGame() =>
        CreateGame(new[] {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.ApprenticeSeer,
            RoleTypes.Insomniac
        });

    [TestCase(RoleTypes.Werewolf)]
    [TestCase(RoleTypes.MysticWolf)]
    public void WerewolvesShouldKnowOthersAreVillagers(RoleTypes centerCardEvilRole)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            centerCardEvilRole,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        game.Run();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        GamePlayer player2 = game.Players[1];
        probabilities[player2].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player2].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        probabilities[player2].Probabilities[centerCardEvilRole].ShouldBe(0);

        GamePlayer player3 = game.Players[2];
        probabilities[player3].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player3].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        probabilities[player3].Probabilities[centerCardEvilRole].ShouldBe(0);
    }

    [Test]
    [Category("Claims")]
    public void WerewolvesShouldNotClaimWerewolf()
    {
        // Arrange
        Game game = SetupLoneWolfGame();
        GamePlayer wolf = game.Players.First();

        // Act
        game.Run();

        // Assert
        wolf.Events.Any(e => e is ClaimedRoleEvent { ClaimedRole: RoleTypes.Werewolf }).ShouldBeFalse();
    }

    [Test]
    [Category("Claims")]
    public void WerewolvesWithoutSafeClaimsShouldInitiallyDeferClaimingRoles()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Mason,
            RoleTypes.Insomniac,
            RoleTypes.Mason
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer wolf = game.Players.First();

        // Act
        game.Run();

        // Assert
        wolf.Events.Any(e => e is DeferredClaimingRoleEvent).ShouldBeTrue();
    }

    [TestCase(RoleTypes.Insomniac)]
    [TestCase(RoleTypes.Villager)]
    [TestCase(RoleTypes.Seer)]
    [TestCase(RoleTypes.ApprenticeSeer)]
    public void LoneWolfShouldClaimSafeRolesSeenInCenter(RoleTypes safeRole)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Mason,
            RoleTypes.Werewolf,
            safeRole
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer wolf = game.Players.First();
        wolf.PickSingleCard = PickCardByIndex(2);

        // Act
        game.Run();

        // Assert
        wolf.Events.Any(e => e is DeferredClaimingRoleEvent).ShouldBeFalse();
        wolf.Events.OfType<ClaimedRoleEvent>().ShouldContain(e => e.ClaimedRole == safeRole);
    }

    [TestCase(RoleTypes.Exposer)]
    [TestCase(RoleTypes.Thing)]
    [TestCase(RoleTypes.Werewolf)]
    [TestCase(RoleTypes.MysticWolf)]
    [TestCase(RoleTypes.Mason)]
    public void LoneWolfShouldNotClaimUnsafeRolesSeenInCenter(RoleTypes unsafeRole)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Mason,
            RoleTypes.Werewolf,
            unsafeRole
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer wolf = game.Players.First();
        wolf.PickSingleCard = PickCardByIndex(2);

        // Act
        game.Run();

        // Validate that the wolf has deferred voting
        

        // Assert
        wolf.Events.OfType<ClaimedRoleEvent>().ShouldNotContain(e => e.ClaimedRole == unsafeRole);
    }

    [Test]
    public void WolvesShouldNotBothClaimSameRole()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Werewolf,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Villager,
            RoleTypes.Exposer,
            RoleTypes.Seer
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer wolf1 = game.Players[0];
        GamePlayer wolf2 = game.Players[1];

        // Act
        game.Run();

        // Assert
        var role1 = wolf1.OwnEvents.OfType<ClaimedRoleEvent>().First().ClaimedRole;
        var role2 = wolf2.OwnEvents.OfType<ClaimedRoleEvent>().First().ClaimedRole;
        role1.ShouldNotBe(role2);
    }

}