namespace MattEland.WhereDoggo.Core.Tests;

public abstract class GameTestsBase
{
    protected static Game CreateGame(ICollection<RoleTypes> assignedRoles, GameOptions? options = null)
    {
        options ??= CreateGameOptions();

        return new Game(assignedRoles, options);
    }

    protected static GameOptions CreateGameOptions()
    {
        return new GameOptions
        {
            RandomizeSlots = false
        };
    }

    protected static Game RunGame(ICollection<RoleTypes> assignedRoles, GameOptions? gameOptions = null)
    {
        Game game = CreateGame(assignedRoles, gameOptions);
        game.Run();

        return game;
    }
    
}