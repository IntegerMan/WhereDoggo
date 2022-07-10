using System;
using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests;

/// <summary>
/// Tests related to the Sentinel Role
/// </summary>
public class SentinelTests : GameTestsBase
{
    [Test]
    public void SentinelsShouldKnowTheyAreSentinels()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.ApprenticeSeer,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> finalProbabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        finalProbabilities[player].Probabilities[RoleTypes.Sentinel].ShouldBe(1);
        finalProbabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        finalProbabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }
    
    [Test]
    public void SentinelThatPlacesTokenShouldResultInCardWithToken()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.SentinelTokenPlacementStrategy = new SelectSpecificSlotPlacementStrategy(1); // WW player

        // Act
        game.Run();

        // Assert
        game.Players[1].HasSentinelToken.ShouldBeTrue();
    }

    [Test]
    public void SentinelThatPlacesTokenShouldHaveAppropriateEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
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
        player.Events.FirstOrDefault(e => e is SentinelTokenPlacedEvent).ShouldNotBeNull();
        player.Events.FirstOrDefault(e => e is SkippedNightActionEvent).ShouldBeNull();
    }

    [Test]
    public void SentinelMayChooseToNotPlaceTokenShouldResultInNoTokens()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.SentinelTokenPlacementStrategy = new OptOutSlotSelectionStrategy();

        // Act
        game.Run();

        // Assert
        game.Players.Any(p => p.HasSentinelToken).ShouldBeFalse();
    }    
    
    [Test]
    public void SentinelMayChooseToNotPlaceTokenShouldHaveAppropriateEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.SentinelTokenPlacementStrategy = new OptOutSlotSelectionStrategy();

        // Act
        game.Run();

        // Assert
        player.Events.FirstOrDefault(e => e is SkippedNightActionEvent).ShouldNotBeNull();
        player.Events.FirstOrDefault(e => e is SentinelTokenPlacedEvent).ShouldBeNull();
    }

    [Test]
    public void SentinelTokenCausesNonSentinelsToKnowSentinelNotInCenter()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };

        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer ww = game.Players[1];
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = ww.Brain.BuildFinalRoleProbabilities();
        probabilities[game.CenterSlots.First()].Probabilities[RoleTypes.Sentinel].ShouldBe(0);
        probabilities[game.Players.First()].Probabilities[RoleTypes.Sentinel].ShouldBeGreaterThan(1M/assignedRoles.Length);
    }  
    
    [Test]
    [TestCase(0, GamePhase.Night)] // Sentinel
    [TestCase(1, GamePhase.Night)] // Werewolf
    [TestCase(2, GamePhase.Day)]   // Villager
    public void SentinelTokenCausesPlayersToSeeTokenOnCard(int playerIndex, GamePhase expectedPhase)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };

        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer observer = game.Players[playerIndex];
        GameEventBase? observedEvent = observer.Events.FirstOrDefault(e => e is SentinelTokenObservedEvent);
        observedEvent.ShouldNotBeNull();
        observedEvent.Phase.ShouldBe(expectedPhase);
    }

    [Test]
    public void SentinelThrowsInvalidOperationExceptionWhenForcedToPutTokenOnThemselves()
    {
        // Arrange
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Sentinel,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.SentinelTokenPlacementStrategy = new SelectSpecificSlotPlacementStrategy(0); // Sentinel

        Assert.That(() =>
        {
            game.Run();
        }, Throws.TypeOf<InvalidOperationException>());
    }    
}