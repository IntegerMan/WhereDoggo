using MattEland.WhereDoggo.Core.Events;
using MattEland.WhereDoggo.Core.Events.Claims;

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
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.Players[1]].Probabilities[RoleTypes.Mason].ShouldBe(0);
        probabilities[game.Players[2]].Probabilities[RoleTypes.Mason].ShouldBe(0);
    }

    [Test]
    public void LoneMasonShouldHaveOnlyMasonEvent()
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
  
        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is OnlyMasonEvent);
    }

    [Test]
    public void LoneMasonShouldInferOtherMasonInCenter()
    {
        // Arrange
        Game game = RunGame();
        GamePlayer player = game.Players.First();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (CenterCardSlot slot in game.CenterSlots)
        {
            probabilities[slot].Probabilities[RoleTypes.Mason].ShouldBe(0.2m);
        }
    }

    private static Game RunGame() =>
        RunGame(new[] {
            // Player Roles
            RoleTypes.Mason,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Werewolf,
            RoleTypes.Mason,
            RoleTypes.Villager
        });

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
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.Players[1]].Probabilities[RoleTypes.Mason].ShouldBe(1);
    }

    [Test]
    public void DualMasonsShouldNotHaveOnlyMasonEvent()
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

        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldNotContain(e => e is OnlyMasonEvent);
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
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();
        
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
        // Arrange
        Game game = RunGame();
        GamePlayer mason = game.Players.First();

        // Act
        game.Run();

        // Assert
        mason.Events.Any(e => e is ClaimedRoleEvent { ClaimedRole: RoleTypes.Mason }).ShouldBeTrue();
    }
}