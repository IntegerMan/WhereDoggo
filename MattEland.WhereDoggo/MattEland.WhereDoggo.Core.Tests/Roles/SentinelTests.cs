using System;
using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests related to the Sentinel Role
/// </summary>
[Category("Roles")]
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
        IDictionary<CardContainer, CardProbabilities> finalProbabilities = player.Brain.BuildFinalRoleProbabilities();

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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;

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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;

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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;

        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is SentinelTokenPlacedEvent);
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
        IDictionary<CardContainer, CardProbabilities> probabilities = ww.Brain.BuildFinalRoleProbabilities();
        probabilities[game.CenterSlots.First()].Probabilities[RoleTypes.Sentinel].ShouldBe(0);
        probabilities[game.Players.First()].Probabilities[RoleTypes.Sentinel].ShouldBeGreaterThan(1M / assignedRoles.Length);
    }

    [Test]
    [TestCase(0, "Night")] // Sentinel
    [TestCase(1, "Night")] // Werewolf
    [TestCase(2, "Day")]   // Villager
    public void SentinelTokenCausesPlayersToSeeTokenOnCard(int playerIndex, string expectedPhase)
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
}