﻿using System;
using MattEland.WhereDoggo.Core.Events;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="RevealerRole"/>
/// </summary>
public class RevealerTests : GameTestsBase
{
    [Test]
    public void RevealerShouldRevealVillagers()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(1);

        // Act
        game.Run();

        // Assert
        game.Players[1].IsRevealed.ShouldBeTrue();
    }    
    
    [Test]
    public void RevealerShouldNotBeAbleToRevealThemselves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(0);

        // Act / Assert
        Assert.That(() => game.Run(), Throws.TypeOf<InvalidOperationException>());
    }

    [Test] 
    public void AllPlayersShouldKnowRevealedRoles()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(1);
        game.Run();

        // Act
        foreach (GamePlayer p in game.Players)
        {
            IDictionary<RoleContainerBase, CardProbabilities> probabilities = p.Brain.BuildFinalRoleProbabilities();
            // Assert
            probabilities[game.Players[2]].LikelyRole.ShouldBe(RoleTypes.Villager);
            probabilities[game.Players[2]].IsCertain.ShouldBeTrue();
        }
    }
    
    [Test] 
    public void AllPlayersShouldHaveSawRevealedRoleEventWhenRoleIsRevealed()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(1);
        
        // Act
        game.Run();

        // Assert
        foreach (GamePlayer p in game.Players)
        {
            p.Events.Where(e => e is KnowsRoleEvent).Cast<KnowsRoleEvent>().ShouldContain(e => e.Target == game.Players[1]);
        }
    }
    
    [Test]
    public void RevealerShouldNotRevealWerewolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(2);

        // Act
        game.Run();

        // Assert
        game.Players[2].IsRevealed.ShouldBeFalse();
    }

    [Test]
    public void RevealerShouldHaveKnowledgeOfWerewolfTheySaw()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(2);
        game.Run();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();
        // Assert
        probabilities[game.Players[2]].LikelyRole.ShouldBe(RoleTypes.Werewolf);
        probabilities[game.Players[2]].IsCertain.ShouldBeTrue();
    }
    
    [Test]
    public void RevealerShouldNotRevealTanners()
    {
        Assert.Inconclusive("Not Implemented");
    }
    
    [Test]
    [Category("Claims")]
    public void RevealerShouldClaimRevealer()
    {
        Assert.Inconclusive("Not Implemented");
    }
}