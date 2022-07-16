namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="ThingRole"/>
/// </summary>
[Category("Roles")]
public class ThingTests : GameTestsBase
{
    [Test]
    public void ThingShouldHaveTappedPlayerEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Thing,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        
        // Act
        game.Run();

        // Assert
        player.Events.ShouldContain(e => e is ThingTappedEvent);
    }
    
    [Test]
    public void OtherPlayerShouldHaveTappedByThingEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Thing,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer thing = game.Players[0];
        GamePlayer target = game.Players[2];
        thing.PickSingleCard = PickFirstCard;
        
        // Act
        game.Run();

        // Assert
        target.Events.ShouldContain(e => e is ThingTappedEvent);
    }
    
    [Test]
    public void OtherPlayerShouldBeCertainOfTheThingsRole()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Thing,
            RoleTypes.Villager,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Insomniac,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer thing = game.Players[0];
        GamePlayer target = game.Players[2];
        thing.PickSingleCard = PickFirstCard;
        game.Run();
        
        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = target.Brain.BuildInitialRoleProbabilities();

        // Assert
        probabilities[thing].IsCertain.ShouldBeTrue();
        probabilities[thing].ProbableRole.ShouldBe(RoleTypes.Thing);
        probabilities[thing].ProbableTeam.ShouldBe(Teams.Villagers);
    }
    
    [Test]
    public void ThingWhoSkippedShouldHaveSkippedEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Thing,
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
        player.Events.ShouldContain(e => e is SkippedNightActionEvent);
    }
}