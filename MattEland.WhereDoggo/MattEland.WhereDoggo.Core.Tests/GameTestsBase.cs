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

    protected static GameOptions CreateGameOptions() => new()
        {
            RandomizeSlots = false
        };

    protected static Game RunGame(ICollection<RoleTypes> assignedRoles, GameOptions? gameOptions = null)
    {
        Game game = CreateGame(assignedRoles, gameOptions);
        game.RunUntil("Voting");

        return game;
    }
    protected static Game RunFullGame(ICollection<RoleTypes> assignedRoles, GameOptions? gameOptions = null)
    {
        Game game = CreateGame(assignedRoles, gameOptions);
        game.Run();

        return game;
    }

    protected static Func<IEnumerable<IHasCard>, IHasCard?> PickNothing => (_) => null;
    protected static Func<IEnumerable<IHasCard>, IHasCard?> PickFirstCard => (cards) => cards.First();
    protected static Func<IEnumerable<IHasCard>, IHasCard?> PickCardByIndex(int index) => (cards) => cards.ToList()[index];
}