namespace MattEland.WhereDoggo.Core.Tests;

public abstract class GameTestsBase
{
    protected Game RunGame(ICollection<RoleTypes> assignedRoles)
    {
        Game game = new(assignedRoles, randomizeSlots: false);
        game.Start();
        game.PerformNightPhase();

        return game;
    }
    
}