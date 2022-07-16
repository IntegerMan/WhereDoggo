namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="SeerRole"/>
/// </summary>
[Category("Roles")]
public class SeerTests : GameTestsBase
{
    [Test]
    public void SeerWhoSkippedShouldHaveSkippedEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Seer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSeerCards = (_, _) => new List<CardContainer>();

        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
        player.Events.ShouldNotContain(e => e is ObservedPlayerCardEvent);
    }

    [Test]
    public void SeerWhoLookedAtCenterCardsShouldHaveTwoObservedCenterCardEvents()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Seer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        
        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is ObservedCenterCardEvent, 2);
        player.Events.ShouldNotContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedPlayerCardEvent);
    }

    [Test]
    public void SeerWhoLookedAtCenterCardsShouldHaveKnowledgeOfCenterCards()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Seer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSeerCards = (_, center) => new[] { center[0], center[1] }.ToList();
        game.Run();

        // Act
        var probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.CenterSlots[0]].IsCertain.ShouldBeTrue();
        probabilities[game.CenterSlots[0]].ProbableRole.ShouldBe(RoleTypes.Insomniac);
        probabilities[game.CenterSlots[1]].IsCertain.ShouldBeTrue();
        probabilities[game.CenterSlots[1]].ProbableRole.ShouldBe(RoleTypes.Werewolf);
    }

    [Test]
    public void SeerWhoLookedAtOtherPlayerShouldHaveAppropriateEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Seer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSeerCards = (players, _) => players.Take(1).ToList();
        
        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is ObservedPlayerCardEvent, 1);
        player.Events.ShouldNotContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    [Test]
    public void SeerWhoLookedAtOtherPlayerShouldHaveKnowledgeAboutTheirRole()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Seer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players[0];
        GamePlayer target = game.Players[1];
        player.Strategies.PickSeerCards = (players, _) => players.Take(1).ToList();
        game.Run();

        // Act
        var probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[target].IsCertain.ShouldBeTrue();
        probabilities[target].ProbableRole.ShouldBe(target.InitialRole.RoleType);
    }
}