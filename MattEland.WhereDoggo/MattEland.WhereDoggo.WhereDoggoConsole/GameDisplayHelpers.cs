using MattEland.WhereDoggo.Core.Events;
using Spectre.Console;

namespace MattEland.WhereDoggo.WhereDoggoConsole;

public static class GameDisplayHelpers
{
    public static void DisplayGameState(this Game game)
    {
        foreach (IHasCard holder in game.Entities)
        {
            Console.WriteLine($"\t{holder} is a {holder.CurrentCard}");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Gets a list of events that occurred inside the game during a specific phase.
    /// </summary>
    /// <param name="game">The game. Usable as an extension method</param>
    /// <param name="phase">The phase to search for</param>
    /// <returns>Events that occurred during that phase</returns>
    public static List<GameEventBase> FindEventsForPhase(this Game game, string phase) =>
        game.Events.Where(e => e.Phase == phase)
            .OrderBy(e => e.Id)
            .ToList();
    
    public static void DisplayNightActions(this Game game)
    {
        AnsiConsole.MarkupLine("During the [Cyan]Night[/]:");
        List<GameEventBase> events = game.FindEventsForPhase("Night");
        foreach (GameEventBase e in events)
        {
            AnsiConsole.WriteLine($"\t{e}");
        }
        AnsiConsole.WriteLine();
    }

    public static void DisplayPlayerKnowledge(this Game game, bool includeProbabilities)
    {
        foreach (GamePlayer player in game.Players)
        {
            player.DisplayPlayerKnowledge();

            if (!includeProbabilities)
            {
                continue;
            }

            AnsiConsole.MarkupLine($"[Orange1]{player.Name}[/] Assumed Probabilities:");
                
            IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

            foreach (KeyValuePair<IHasCard, CardProbabilities> kvp in probabilities)
            {
                AnsiConsole.WriteLine($"\t{kvp.Key.Name} probabilities ({kvp.Value})");
            }

            AnsiConsole.WriteLine();
        }
    }

    public static void DisplayPlayerKnowledge(this GamePlayer player)
    {
        AnsiConsole.MarkupLine($"[Orange1]{player.Name}[/] knows:");

        foreach (GameEventBase @event in player.Events)
        {
            AnsiConsole.WriteLine($"\t{@event}");
        }

        AnsiConsole.WriteLine();
    }

    public static void DisplayAllEvents(this Game game)
    {
        AnsiConsole.WriteLine("All Game Events:");

        foreach (GameEventBase @event in game.Events)
        {
            AnsiConsole.WriteLine($"\t{@event}");
        }

        AnsiConsole.WriteLine();
    }
}