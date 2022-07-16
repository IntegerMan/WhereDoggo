namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="ExposerRole"/>
/// </summary>
[Category("Roles")]
public class ExposerTests : GameTestsBase
{
    [Test]
    public void ExposerShouldAlwaysRevealAtLeastOneCenterCard()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Exposer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };

        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        game.CenterSlots.Count(s => s.CurrentCard.IsRevealed).ShouldBeGreaterThan(0);
    }
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void ExposerShouldExposeCorrectNumberOfCards(int numCards)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Exposer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        GameOptions options = CreateGameOptions();
        options.ExposerOptions.ForceNumberOfCardsRevealed(numCards);

        // Act
        Game game = RunGame(assignedRoles, options);

        // Assert
        game.CenterSlots.Count(s => s.CurrentCard.IsRevealed).ShouldBe(numCards);
    }
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void ExposerShouldHaveCorrectNumberOfRevealedCardEvents(int numCards)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Exposer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        GameOptions options = CreateGameOptions();
        options.ExposerOptions.ForceNumberOfCardsRevealed(numCards);

        // Act
        Game game = RunGame(assignedRoles, options);

        // Assert
        game.Players.First().Events.Count(e => e is RevealedRoleEvent).ShouldBe(numCards);
    }    
    
    [Test]
    public void ExposerCausesAllPlayersToKnowExposerIsInPlay()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Exposer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        
        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        foreach (GamePlayer p in game.Players)
        {
            IDictionary<IHasCard, CardProbabilities> probabilities = p.Brain.BuildFinalRoleProbabilities();
            foreach (CenterCardSlot slot in game.CenterSlots)
            {
                probabilities[slot].Probabilities[RoleTypes.Exposer].ShouldBe(0);
            }
        }
    }
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void ExposerCausesAllPlayersToHaveObserveRevealedCardEvents(int numCards)
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Exposer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        GameOptions options = CreateGameOptions();
        options.ExposerOptions.ForceNumberOfCardsRevealed(numCards);

        // Act
        Game game = RunGame(assignedRoles, options);

        // Assert
        foreach (GamePlayer p in game.Players)
        {
            p.Events.Count(e => e is RevealedRoleObservedEvent).ShouldBe(numCards);
        }
    }
    
    [Test]
    public void ExposerWhoSkipsExposingShouldHaveCorrectEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Exposer,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickNothing;

        // Act
        game.Run();

        // Assert
        game.Players.First().Events.ShouldContain(e => e is SkippedNightActionEvent);
    }
}