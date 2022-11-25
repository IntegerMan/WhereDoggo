namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="MysticWolfRole" />
/// </summary>
public class MysticWolfTests : GameTestsBase
{
    [Test]
    public void MysticWolfShouldSeeOtherWerewolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Werewolf,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        
        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is SawAsWerewolfEvent);
    }
    
    [Test]
    public void MysticWolfShouldNeverPickOtherWerewolves()
    {
        for (int i = 0; i < 100; i++)
        {
            // Arrange
            RoleTypes[] assignedRoles =
            {
                // Player Roles
                RoleTypes.MysticWolf,
                RoleTypes.Werewolf,
                RoleTypes.Insomniac,
                // Center Cards
                RoleTypes.Revealer,
                RoleTypes.Villager,
                RoleTypes.Villager
            };

            // Act
            Game game = RunGame(assignedRoles);

            // Assert
            GamePlayer player = game.Players.First();
            player.Events.OfType<ObservedPlayerCardEvent>()
                .ShouldContain(e => e.ObservedCard.RoleType == RoleTypes.Insomniac);
        }
    }
    
    [Test]
    public void MysticWolfShouldBeSeenByOtherWerewolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Werewolf,
            RoleTypes.MysticWolf,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();
        
        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.Players[1]].ProbableTeam.ShouldBe(Teams.Werewolves);
        probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
        probabilities[game.Players[1]].ProbableRole.ShouldBe(RoleTypes.MysticWolf);
    }   
    
    [Test]
    public void MysticWolfShouldKnowOtherWolvesAreWolves()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Werewolf,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Villager,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        GamePlayer player = game.Players.First();
        
        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.Players[1]].ProbableTeam.ShouldBe(Teams.Werewolves);
        probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
        probabilities[game.Players[1]].ProbableRole.ShouldBe(RoleTypes.Werewolf);
    }
    
    [Test]
    public void MysticWolfShouldPerformLoneWolfAction()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Villager,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        
        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is OnlyWolfEvent);
        player.Events.ShouldContain(e => e is ObservedCenterCardEvent);
    }    
    
    [Test]
    public void MysticWolfShouldBeAbleToSeeOtherPlayersCard()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Villager,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer player = game.Players.First();
        player.PickSingleCard = PickFirstCard;
        game.Run();
        
        // Act
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Assert
        probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
        probabilities[game.Players[1]].ProbableRole.ShouldBe(assignedRoles[1]);
    }    
    
    [Test]
    public void MysticWolfWhoLookedShouldHaveAppropriateEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Villager,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        
        // Act
        Game game = RunGame(assignedRoles);

        // Assert
        GamePlayer player = game.Players.First();
        player.Events.ShouldContain(e => e is ObservedPlayerCardEvent);
    }
    
    [Test]
    public void MysticWolfWhoSkippedShouldHaveAppropriateEvent()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Villager,
            RoleTypes.Insomniac,
            // Center Cards
            RoleTypes.Revealer,
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
    
    [Test]
    [Category("Voting")]
    public void MysticWolfVotedOutShouldCauseWolfLoss()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.MysticWolf,
            RoleTypes.Mason,
            RoleTypes.Mason,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Werewolf,
            RoleTypes.Villager
        };
        Game game = RunGame(assignedRoles);
        
        // Act
        GameResult? result = game.Result;
        
        // Assert
        result.ShouldNotBeNull();
        result.WerewolfKilled.ShouldBeTrue();
        result.Winners.Count().ShouldBe(2);
        result.Winners.ShouldAllBe(w => w.CurrentCard.Team == Teams.Villagers);
    }     
    
    [Test]
    public void MysticWolfShouldNotBeAbleToViewSentinelTokenCard()
    {
        // Arrange
        RoleTypes[] assignedRoles =
        {
            // Player Roles
            RoleTypes.Villager,
            RoleTypes.Sentinel,
            RoleTypes.MysticWolf,
            RoleTypes.Werewolf,
            // Center Cards
            RoleTypes.Revealer,
            RoleTypes.Exposer,
            RoleTypes.Villager
        };
        Game game = CreateGame(assignedRoles);
        GamePlayer mWolf = game.Players[2];
        mWolf.PickSingleCard = PickFirstCard;
        GamePlayer sentinel = game.Players[1];
        sentinel.PickSingleCard = PickFirstCard;

        // Act
        game.Run();
        
        // Assert
        game.Players.First(p => p.HasSentinelToken).InitialCard.RoleType.ShouldBe(RoleTypes.Villager);
        
        ObservedPlayerCardEvent card = (ObservedPlayerCardEvent)mWolf.Events.First(e => e is ObservedPlayerCardEvent);
        card.ObservedCard.ShouldBe(game.Players[1].CurrentCard);
        
        IDictionary<IHasCard,CardProbabilities> probabilities = mWolf.Brain.BuildInitialRoleProbabilities();
        probabilities[game.Players[0]].IsCertain.ShouldBeFalse();
        probabilities[game.Players[1]].IsCertain.ShouldBeTrue();
    }    
    
    [Test]
    [Category("Claims")]
    public void MysticWolfShouldNotClaimMysticWolf()
    {
        Assert.Inconclusive("Claims are not implemented");
    }
}