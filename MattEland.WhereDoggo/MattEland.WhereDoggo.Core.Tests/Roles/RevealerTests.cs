﻿using System;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="RevealerRole"/>
/// </summary>
[Category("Roles")]
public class RevealerTests : GameTestsBase
{
    [Test]
    public void RevealerShouldRevealVillagers()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;

        // Act
        game.Run();

        // Assert
        game.Players[1].CurrentCard.IsRevealed.ShouldBeTrue();
        player.Events.ShouldContain(e => e is RevealedRoleEvent);
        player.Events.ShouldContain(e => e is RevealedRoleObservedEvent);
    }

    private static Game SetupGame() =>
        CreateGame(new[] {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        });

    [Test] 
    public void AllPlayersShouldKnowRevealedRoles()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        game.Run();

        // Act
        foreach (GamePlayer p in game.Players)
        {
            IDictionary<IHasCard, CardProbabilities> probabilities = p.Brain.BuildFinalRoleProbabilities();
            // Assert
            probabilities[game.Players[1]].ProbableRole.ShouldBe(RoleTypes.Villager);
            probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
        }
    }
    
    [Test] 
    public void AllPlayersShouldHaveSawRevealedRoleEventWhenRoleIsRevealed()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        
        // Act
        game.Run();

        // Assert
        foreach (GamePlayer p in game.Players)
        {
            p.Events.Any(e => e is RevealedRoleObservedEvent obs && obs.Target == game.Players[1]).ShouldBeTrue();
        }
    }
    
    [Test] 
    public void OtherPlayersShouldKnowRevealerInPlayWhenCardIsRevealed()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        
        // Act
        game.Run();

        // Assert
        foreach (GamePlayer p in game.Players.Where(p => p != player))
        {
            IDictionary<IHasCard, CardProbabilities> probabilities = p.Brain.BuildFinalRoleProbabilities();
            foreach (CenterCardSlot slot in game.CenterSlots)
            {
                probabilities[slot].Probabilities[RoleTypes.Revealer].ShouldBe(0);
            }
        }
    }
    
    [Test] 
    public void RevealerWhoDoesNotRevealShouldHaveSkippedEvent()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;
        
        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
    }
    
    [Test]
    public void RevealerShouldNotRevealWerewolves()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;

        // Act
        game.Run();

        // Assert
        game.Players[2].CurrentCard.IsRevealed.ShouldBeFalse();
        player.Events.ShouldContain(e => e is RevealedRoleEvent);
        player.Events.ShouldContain(e => e is RevealedRoleObservedEvent);
        player.Events.ShouldContain(e => e is RevealerHidEvilRoleEvent);
    }

    [Test]
    public void RevealerShouldHaveKnowledgeOfWerewolfTheySaw()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        game.Run();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();
        // Assert
        probabilities[game.Players[1]].ProbableRole.ShouldBe(RoleTypes.Werewolf);
        probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
    }
    
    [Test]
    public void RevealerShouldNotBeAbleToRevealSentinelTokenCard()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer revealer = game.Players[2];
        revealer.PickSingleCard = PickFirstCard;
        GamePlayer sentinel = game.Players[1];
        sentinel.PickSingleCard = PickFirstCard;

        // Act
        game.Run();
        
        // Assert
        game.Players[0].HasSentinelToken.ShouldBeTrue();
        game.Players[0].CurrentCard.IsRevealed.ShouldBeFalse();
        game.Players[1].CurrentCard.IsRevealed.ShouldBeTrue();
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