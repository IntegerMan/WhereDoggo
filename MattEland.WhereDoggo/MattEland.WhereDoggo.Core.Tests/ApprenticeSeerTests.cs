namespace MattEland.WhereDoggo.Core.Tests;

/// <summary>
/// Tests for the <see cref="ApprenticeSeerRole"/>
/// </summary>
public class ApprenticeSeerTests : GameTestsBase
{
    [Test]
    public void ApprenticeSeerShouldBeCertainOfTheCardTheySaw()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.ApprenticeSeer,
            RoleTypes.Werewolf,
            RoleTypes.Villager,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = new(assignedRoles, randomizeSlots: false);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCardFromCenterStrategy = new SelectSpecificSlotPlacementStrategy(0);
        game.Run();

        // Act
        IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        CardProbabilities cardProbs = probabilities[game.CenterSlots[0]];
        cardProbs.Probabilities[RoleTypes.Insomniac].ShouldBe(1);
        cardProbs.IsCertain.ShouldBeTrue();
    }

    [Test]
    public void ApprenticeSeerShouldBeCertainSingleCardNotInPlay()
    {
        Assert.Inconclusive();
    }

    [Test]
    public void ApprenticeSeerShouldClaimToBeApprenticeSeer()
    {
        Assert.Inconclusive();
    }

    [Test]
    public void ApprenticeSeerShouldClaimToSeeTheCardTheySaw()
    {
        Assert.Inconclusive();
    }

}