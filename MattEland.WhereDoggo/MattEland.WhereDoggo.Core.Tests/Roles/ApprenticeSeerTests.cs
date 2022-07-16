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
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();
        game.Run();

        // Act
        IDictionary<CardContainer, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        CardProbabilities cardProbs = probabilities[game.CenterSlots[0]];
        cardProbs.Probabilities[RoleTypes.Insomniac].ShouldBe(1);
        cardProbs.IsCertain.ShouldBeTrue();
    }

    [Test]
    public void ApprenticeSeerShouldBeCertainSingleCardNotInPlay()
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
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (cards) => cards.First();
        game.Run();

        // Act
        IDictionary<CardContainer, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

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
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (_) => null;
        game.Run();

        // Act
        IDictionary<CardContainer, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

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
        GamePlayer player = game.Players.First();
        player.Strategies.PickSingleCard = (_) => null;

        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
        player.Events.ShouldNotContain(e => e is ObservedCenterCardEvent);
    }

    [Test]
    public void ApprenticeSeerWhoUsedAbilityShouldHaveCorrectEvent()
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

        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is ObservedCenterCardEvent);
        player.Events.ShouldNotContain(e => e is SkippedNightActionEvent);
    }

    [Test]
    [Category("Claims")]
    public void ApprenticeSeerShouldClaimToBeApprenticeSeer()
    {
        Assert.Inconclusive();
    }

    [Test]
    [Category("Claims")]
    public void ApprenticeSeerShouldClaimToSeeTheCardTheySaw()
    {
        Assert.Inconclusive("Not Implemented");
    }

}