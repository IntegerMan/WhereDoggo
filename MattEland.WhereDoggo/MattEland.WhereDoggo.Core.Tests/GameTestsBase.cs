namespace MattEland.WhereDoggo.Core.Tests;

public abstract class GameTestsBase
{
    protected OneNightWhereDoggoGame RunGame(GameRoleBase[] orderedRoles)
    {
        int numPlayers = orderedRoles.Length - OneNightWhereDoggoGame.NumCenterCards;
        
        OneNightWhereDoggoGame game = new(numPlayers);
        
        game.SetUp(orderedRoles, randomizeSlots: false);
        game.Start();
        game.PerformNightPhase();

        return game;
    }
}