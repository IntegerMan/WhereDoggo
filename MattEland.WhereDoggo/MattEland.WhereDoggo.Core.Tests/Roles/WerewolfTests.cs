﻿using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

[Category("Roles")]
public class WerewolfTests : GameTestsBase
{

    [Test]
    public void WerewolvesShouldThinkTheyAreWerewolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(1);
        probabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }

    [Test]
    public void LoneWolfWhoSeesAWolfWithOnlyVillagersShouldKnowAllRoles()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(0);
        game.Run();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (KeyValuePair<RoleContainerBase, CardProbabilities> kvp in probabilities)
        {
            kvp.Value.IsCertain.ShouldBeTrue($"Was not certain of role {kvp.Value}");
        }
    }

    [Test]
    public void LoneWolfWhoLooksShouldHaveCorrectEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
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
    public void LoneWolfWhoSkipsShouldHaveCorrectEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new OptOutSlotSelectionStrategy();
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    [Test]
    public void WerewolvesShouldKnowOthersAreVillagers()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(1);
        game.Start();
        game.PerformNightPhase();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        GamePlayer player2 = game.Players[1];
        probabilities[player2].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player2].Probabilities[RoleTypes.Werewolf].ShouldBe(0);

        GamePlayer player3 = game.Players[2];
        probabilities[player3].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player3].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
    }

}