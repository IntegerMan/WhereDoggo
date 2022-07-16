using System;

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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();

        // Act
        game.Run();

        // Assert
        game.Players[1].IsRevealed.ShouldBeTrue();
        player.Events.ShouldContain(e => e is RevealedRoleEvent);
        player.Events.ShouldContain(e => e is RevealedRoleObservedEvent);
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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();;
        game.Run();

        // Act
        foreach (GamePlayer p in game.Players)
        {
            IDictionary<CardContainer, CardProbabilities> probabilities = p.Brain.BuildFinalRoleProbabilities();
            // Assert
            probabilities[game.Players[1]].ProbableRole.ShouldBe(RoleTypes.Villager);
            probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();
        
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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();
        
        // Act
        game.Run();

        // Assert
        foreach (GamePlayer p in game.Players.Where(p => p != player))
        {
            IDictionary<CardContainer, CardProbabilities> probabilities = p.Brain.BuildFinalRoleProbabilities();
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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (_) => null;
        
        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
    }
    
    [Test]
    public void RevealerShouldNotRevealWerewolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();

        // Act
        game.Run();

        // Assert
        game.Players[2].IsRevealed.ShouldBeFalse();
        player.Events.ShouldContain(e => e is RevealedRoleEvent);
        player.Events.ShouldContain(e => e is RevealedRoleObservedEvent);
        player.Events.ShouldContain(e => e is RevealerHidEvilRoleEvent);
    }

    [Test]
    public void RevealerShouldHaveKnowledgeOfWerewolfTheySaw()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Revealer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();
        game.Run();

        // Act
        IDictionary<CardContainer, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();
        // Assert
        probabilities[game.Players[1]].ProbableRole.ShouldBe(RoleTypes.Werewolf);
        probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
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