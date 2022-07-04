using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MattEland.WhereDoggo.Core.Engine;
using MattEland.WhereDoggo.Core.Engine.Roles;
using NUnit.Framework;
using Shouldly;

namespace MattEland.WhereDoggo.Core.Tests;

public class InferenceTests
{
    [Test]
    public void VillagersShouldThinkTheyAreVillagers()
    {
        // Arrange
        const int numPlayers = 3;
        OneNightWhereDoggoGame game = new(numPlayers);
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new RabbitRole(), 
            new DoggoRole(), 
            new RabbitRole(),
            // Center Cards
            new DoggoRole(), 
            new RabbitRole(), 
            new RabbitRole()
        };
        game.SetUp(assignedRoles);
        game.Start();
        game.PerformNightPhase();
        GamePlayer player = game.Players.First();
        GameInferenceEngine inferrer = new();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            inferrer.BuildFinalRoleProbabilities(player, game);

        // Assert
        probabilities[player].ProbabilityRabbit.ShouldBe(1);
        probabilities[player].ProbabilityDoggo.ShouldBe(0);
    }        
        
    [Test]
    public void WerewolvesShouldThinkTheyAreWerewolves()
    {
        // Arrange
        const int numPlayers = 3;
        OneNightWhereDoggoGame game = new(numPlayers);
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new DoggoRole(),
            new RabbitRole(),
            new RabbitRole(),
            // Center Cards
            new DoggoRole(), 
            new RabbitRole(), 
            new RabbitRole()
        };
        game.SetUp(assignedRoles);
        game.Start();
        game.PerformNightPhase();
        GamePlayer player = game.Players.First();
        GameInferenceEngine inferrer = new();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            inferrer.BuildFinalRoleProbabilities(player, game);

        // Assert
        probabilities[player].ProbabilityDoggo.ShouldBe(1);
        probabilities[player].ProbabilityRabbit.ShouldBe(0);
    }       
    
    [Test]
    public void WerewolvesShouldKnowOthersAreVillagers()
    {
        // Arrange
        const int numPlayers = 3;
        OneNightWhereDoggoGame game = new(numPlayers);
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new DoggoRole(),
            new RabbitRole(),
            new RabbitRole(),
            // Center Cards
            new DoggoRole(), 
            new RabbitRole(), 
            new RabbitRole()
        };
        game.SetUp(assignedRoles);
        game.Start();
        game.PerformNightPhase();
        GamePlayer player = game.Players.First();
        GameInferenceEngine inferrer = new();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            inferrer.BuildFinalRoleProbabilities(player, game);

        // Assert
        GamePlayer player2 = game.Players[1];
        probabilities[player2].ProbabilityRabbit.ShouldBe(1);
        probabilities[player2].ProbabilityDoggo.ShouldBe(0);
        GamePlayer player3 = game.Players[2];
        probabilities[player3].ProbabilityRabbit.ShouldBe(1);
        probabilities[player3].ProbabilityDoggo.ShouldBe(0);
    }

    [Test]
    public void RabbitsShouldHaveMixedProbabilitiesOnOtherPlayers()
    {
        // Arrange
        const int numPlayers = 3;
        OneNightWhereDoggoGame game = new(numPlayers);
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new RabbitRole(),
            new DoggoRole(),
            new RabbitRole(),
            // Center Cards
            new DoggoRole(),
            new RabbitRole(),
            new RabbitRole()
        };
        game.SetUp(assignedRoles);
        game.Start();
        game.PerformNightPhase();
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        // 2 Doggos, 3 Rabbits in 5 other players
        GamePlayer secondPlayer = game.Players[1];
        probabilities[secondPlayer].ProbabilityRabbit.ShouldBe(3.0M/5.0M);
        probabilities[secondPlayer].ProbabilityDoggo.ShouldBe(2.0M/5.0M);
    }
}