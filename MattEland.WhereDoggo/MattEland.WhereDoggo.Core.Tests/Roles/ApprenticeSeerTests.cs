using System;
using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="ApprenticeSeerRole"/>
/// </summary>
[Category("Roles")]
public class ApprenticeSeerTests : GameTestsBase
{
    [Test]
    public void ApprenticeSeerShouldBeCertainOfTheCardTheySaw()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = (cards) => cards.First();
        game.Run();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        CardProbabilities cardProbs = probabilities[game.CenterSlots[0]];
        cardProbs.Probabilities[RoleTypes.Insomniac].ShouldBe(1);
        cardProbs.IsCertain.ShouldBeTrue();
    }

    [Test]
    public void ApprenticeSeerShouldBeCertainSingleCardNotInPlay()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        game.Run();

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (GamePlayer slot in game.Players)
        {
            probabilities[slot].Probabilities[RoleTypes.Insomniac].ShouldBe(0);
        }
    }

    [Test]
    public void ApprenticeSeerWhoSkippedShouldHaveNoCertainKnowledgeOfCenter()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;
        game.RunUntil("Voting");

        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        foreach (CenterCardSlot slot in game.CenterSlots)
        {
            probabilities[slot].IsCertain.ShouldBeFalse();
        }
    }

    [Test]
    public void ApprenticeSeerWhoSkippedShouldHaveCorrectEvent()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;

        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    private static Game SetupGame()
    {
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
        Game game = CreateGame(assignedRoles);
        return game;
    }

    [Test]
    public void ApprenticeSeerWhoUsedAbilityShouldHaveCorrectEvent()
    {
        // Arrange
        Game game = SetupGame();

        // Act
        game.Run();

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is ObservedCenterCardEvent);
        player.Events.ShouldNotContain(e => e is SkippedNightActionEvent);
    }

    [Test]
    [Category("Claims")]
    public void ApprenticeSeerShouldClaimToBeApprenticeSeer()
    {
        // Arrange
        Game game = SetupGame();
        GamePlayer apprenticeSeer = game.Players.First();

        // Act
        game.Run();

        // Assert
        apprenticeSeer.Events.Any(e => e is ClaimedRoleEvent { ClaimedRole: RoleTypes.ApprenticeSeer }).ShouldBeTrue();
    }

    [Test]
    [Category("Claims")]
    public void ApprenticeSeerShouldClaimToSeeTheCardTheySaw()
    {
        Assert.Inconclusive("Not Implemented");
    }
}