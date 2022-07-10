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
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> finalProbabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

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
        game.Start();

        // Act
        game.PerformNightPhase();

        // Assert
        game.Players[1].HasSentinelToken.ShouldBeTrue();
    }

    [Test]
    public void SentinelThatPlacesTokenShouldHaveAppropriateEvent()
    {
        Assert.Inconclusive("Not Implemented yet");
    }
    
    [Test]
    public void SentinelMayChooseToNotPlaceTokenShouldHaveAppropriateEvent()
    {
        Assert.Inconclusive("Not Implemented yet");
    }

    [Test]
    public void SentinelTokenCausesNonSentinelsToKnowSentinelNotInCenter()
    {
        Assert.Inconclusive("Not Implemented yet");
    }  
    
    [Test]
    public void SentinelThrowsInvalidOperationExceptionWhenForcedToPutTokenOnThemselves()
    {
        Assert.That(() =>
        {
            
        }, Throws.TypeOf<InvalidOperationException>());
    }    
}