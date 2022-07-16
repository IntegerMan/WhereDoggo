using System;

namespace MattEland.WhereDoggo.Core.Tests;

public abstract class GameTestsBase
{
    protected static Game CreateGame(ICollection<RoleTypes> assignedRoles, GameOptions? options = null)
    {
        options ??= CreateGameOptions();

        Game game = new Game(assignedRoles, options);

        // Set up the game
        game.RunNextPhase();
        
        return game;
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

    protected Func<IEnumerable<CardContainer>, CardContainer?> PickNothing => (_) => null;
    protected Func<IEnumerable<CardContainer>, CardContainer?> PickFirstCard => (cards) => cards.First();
}