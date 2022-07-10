namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests related to the <see cref="MasonRole"/> in One Night Ultimate Werewolf
/// </summary>
[Category("Roles")]
public class MasonTests : GameTestsBase
{
    [Test]
    public void LoneMasonShouldKnowAllOtherPlayersAreNotMasons()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Mason,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            RoleTypes.Mason
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.Players[1]].Probabilities[RoleTypes.Mason].ShouldBe(0);
        probabilities[game.Players[2]].Probabilities[RoleTypes.Mason].ShouldBe(0);
    }

    [Test]
    public void LoneMasonShouldInferOtherMasonInCenter()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Mason,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Mason,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (CenterCardSlot slot in game.CenterSlots)
        {
            probabilities[slot].Probabilities[RoleTypes.Mason].ShouldBe(1m/Game.NumCenterCards);
        }
    }

    [Test]
    public void DualMasonsShouldKnowOtherMason()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Mason,
            RoleTypes.Mason,
            RoleTypes.Werewolf,
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
        probabilities[game.Players[1]].Probabilities[RoleTypes.Mason].ShouldBe(1);
    }

    [Test]
    public void DualMasonsShouldKnowOthersNotMasons()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Mason,
            RoleTypes.Mason,
            RoleTypes.Werewolf,
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
        foreach (CenterCardSlot slot in game.CenterSlots)
        {
            probabilities[slot].Probabilities[RoleTypes.Mason].ShouldBe(0);
        }
    }

    [Test]
    [Category("Claims")]
    public void MasonShouldEventuallyClaimMason()
    {
        Assert.Inconclusive("Not Implemented");
    }

}