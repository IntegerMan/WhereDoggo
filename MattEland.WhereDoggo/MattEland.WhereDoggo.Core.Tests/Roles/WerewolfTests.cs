using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

[Category("Roles")]
public class WerewolfTests : GameTestsBase
{

    [Test]
    public void WerewolvesShouldThinkTheyAreWerewolves()
    {
        // Arrange
        Game game = SetupGame();
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
        Game game = SetupGame();
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
        Game game = SetupGame();


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
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is OnlyWolfEvent);
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    private static Game SetupGame() =>
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
        Game game = SetupGame();
        GamePlayer wolf = game.Players.First();

        // Act
        game.Run();

        // Assert
        wolf.Events.Any(e => e is ClaimedRoleEvent { ClaimedRole: RoleTypes.Werewolf }).ShouldBeFalse();
    }

}