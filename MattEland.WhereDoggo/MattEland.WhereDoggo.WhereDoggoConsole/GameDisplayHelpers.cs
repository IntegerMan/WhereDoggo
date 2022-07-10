using MattEland.WhereDoggo.Core.Events;
using MattEland.WhereDoggo.Core.Gamespace;

namespace MattEland.WhereDoggo.WhereDoggoConsole;

public static class GameDisplayHelpers
{
    public static void DisplayGameState(this Game game)
    {
        foreach (RoleContainerBase container in game.Entities)
        {
            Console.WriteLine($"{container} is a {container.CurrentRole}");
        }

        Console.WriteLine();
    }


    public static void DisplayPlayerKnowledge(this GamePlayer player)
    {
        Console.WriteLine(player.Name + " knows:");

        foreach (GameEventBase @event in player.Events)
        {
            Console.WriteLine($"\t{@event}");
        }

        Console.WriteLine();
    }

    public static void DisplayNightActions(this Game game)
    {
        Console.WriteLine("During the Night:");
        List<GameEventBase> events = game.FindEventsForPhase(GamePhase.Night);
        foreach (GameEventBase e in events)
        {
            Console.WriteLine($"\t{e}");
        }
        Console.WriteLine();
    }

    public static void DisplayPlayerKnowledge(this Game game, bool includeProbabilities)
    {
        foreach (GamePlayer player in game.Players)
        {
            player.DisplayPlayerKnowledge();

            if (!includeProbabilities) continue;
            
            Console.WriteLine($"{player.Name} Assumed Probabilities:");
                
            IDictionary<RoleContainerBase, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

            foreach (KeyValuePair<RoleContainerBase, CardProbabilities> kvp in probabilities)
            {
                Console.WriteLine($"\t{kvp.Key.Name} probabilities ({kvp.Value})");
            }

            Console.WriteLine();
        }
    }    
    
    public static void DisplayAllEvents(this Game game)
    {
        Console.WriteLine("All Game Events:");

        foreach (GameEventBase @event in game.Events)
        {
            Console.WriteLine($"\t{@event}");
        }

        Console.WriteLine();
    }
}