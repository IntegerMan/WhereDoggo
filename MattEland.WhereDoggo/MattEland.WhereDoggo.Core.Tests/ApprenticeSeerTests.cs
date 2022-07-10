using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests;

/// <summary>
/// Tests for the <see cref="ApprenticeSeerRole"/>
/// </summary>
public class ApprenticeSeerTests : GameTestsBase
{
    [Test]
    public void ApprenticeSeerShouldBeCertainOfTheCardTheySaw()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.ApprenticeSeer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(0);
        game.Run();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        CardProbabilities cardProbs = probabilities[game.CenterSlots[0]];
        cardProbs.Probabilities[RoleTypes.Insomniac].ShouldBe(1);
        cardProbs.IsCertain.ShouldBeTrue();
    }

    [Test]
    public void ApprenticeSeerShouldBeCertainSingleCardNotInPlay()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.ApprenticeSeer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(0);
        game.Run();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (GamePlayer slot in game.Players)
        {
            probabilities[slot].Probabilities[RoleTypes.Insomniac].ShouldBe(0);
        }
    }

    [Test]
    public void ApprenticeSeerWhoSkippedShouldHaveNoCertainKnowledgeOfCenter()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.ApprenticeSeer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new OptOutSlotSelectionStrategy();
        game.Run();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (RoleSlot slot in game.CenterSlots)
        {
            probabilities[slot].IsCertain.ShouldBeFalse();
        }
    }

    [Test]
    public void ApprenticeSeerWhoSkippedShouldHaveCorrectEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.ApprenticeSeer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new OptOutSlotSelectionStrategy();

        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    [Test]
    public void ApprenticeSeerWhoUsedAbilityShouldHaveCorrectEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.ApprenticeSeer,
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
        player.Events.ShouldContain(e => e is ObservedCenterCardEvent);
        player.Events.ShouldNotContain(e => e is SkippedNightActionEvent);
    }

    [Test]
    public void ApprenticeSeerShouldClaimToBeApprenticeSeer()
    {
        Assert.Inconclusive();
    }

    [Test]
    public void ApprenticeSeerShouldClaimToSeeTheCardTheySaw()
    {
        Assert.Inconclusive();
    }

}