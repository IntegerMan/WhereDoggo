namespace MattEland.WhereDoggo.Core.Tests;

public class InferenceTests : GameTestsBase
{
    [Test]
    public void VillagersShouldThinkTheyAreVillagers()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new VillagerRole(), 
            new WerewolfRole(), 
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(), 
            new VillagerRole(), 
            new VillagerRole()
        };
        OneNightWhereDoggoGame game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        probabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
    }

    [Test]
    public void WerewolvesShouldThinkTheyAreWerewolves()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new WerewolfRole(),
            new VillagerRole(),
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(), 
            new VillagerRole(), 
            new VillagerRole()
        };
        OneNightWhereDoggoGame game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        probabilities[player].Probabilities[RoleTypes.Werewolf].ShouldBe(1);
        probabilities[player].Probabilities[RoleTypes.Villager].ShouldBe(0);
    }
    
    [Test]
    public void LoneWolfWhoSeesAWolfWithOnlyVillagersShouldKnowAllRoles()
    {
        // Arrange
        const int numPlayers = 3;
        OneNightWhereDoggoGame game = new(numPlayers);
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new WerewolfRole(),
            new VillagerRole(),
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(), 
            new VillagerRole(), 
            new VillagerRole()
        };
        game.SetUp(assignedRoles);
        GamePlayer player = game.Players.First();
        player.LoneWolfSlotSelectionStrategy = new SelectSpecificSlotLoneWolfStrategy(0);
        game.Start();
        game.PerformNightPhase();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        foreach (KeyValuePair<RoleContainerBase, ContainerRoleProbabilities> kvp in probabilities)
        {
            kvp.Value.IsCertain.ShouldBeTrue("Was not certain of role " + kvp.Value);
        }
    }       
    
    [Test]
    public void WerewolvesShouldKnowOthersAreVillagers()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new WerewolfRole(),
            new VillagerRole(),
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(), 
            new VillagerRole(), 
            new VillagerRole()
        };
        OneNightWhereDoggoGame game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities = 
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        GamePlayer player2 = game.Players[1];
        probabilities[player2].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player2].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
        
        GamePlayer player3 = game.Players[2];
        probabilities[player3].Probabilities[RoleTypes.Villager].ShouldBe(1);
        probabilities[player3].Probabilities[RoleTypes.Werewolf].ShouldBe(0);
    }

    [Test]
    public void VillagersShouldHaveMixedProbabilitiesOnOtherPlayers()
    {
        // Arrange
        GameRoleBase[] assignedRoles =
        {
            // Player Roles
            new VillagerRole(),
            new WerewolfRole(),
            new VillagerRole(),
            // Center Cards
            new WerewolfRole(),
            new VillagerRole(),
            new VillagerRole()
        };
        OneNightWhereDoggoGame game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, ContainerRoleProbabilities> probabilities =
            player.Brain.BuildFinalRoleProbabilities(player, game);

        // Assert
        // 2 Doggos, 3 Rabbits in 5 other players
        GamePlayer secondPlayer = game.Players[1];
        probabilities[secondPlayer].Probabilities[RoleTypes.Villager].ShouldBe(3.0M/5.0M);
        probabilities[secondPlayer].Probabilities[RoleTypes.Werewolf].ShouldBe(2.0M/5.0M);
    }
}