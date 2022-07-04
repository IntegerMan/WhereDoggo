﻿namespace MattEland.WhereDoggo.WhereDoggoConsole;

public static class OneNightWhereDoggoDisplayHelpers
{
    public static void DisplayGameState(this OneNightWhereDoggoGame game)
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

    public static void DisplayNightActions(this OneNightWhereDoggoGame game)
    {
        Console.WriteLine("During the Night:");
        List<GameEventBase> events = game.FindEventsForPhase(GamePhase.Night);
        foreach (GameEventBase e in events)
        {
            Console.WriteLine($"\t{e}");
        }
        Console.WriteLine();
    }

    public static void DisplayPlayerKnowledge(this OneNightWhereDoggoGame game)
    {
        foreach (GamePlayer player in game.Players)
        {
            player.DisplayPlayerKnowledge();
        }
    }    
    
    public static void DisplayAllEvents(this OneNightWhereDoggoGame game)
    {
        Console.WriteLine("All Game Events:");

        foreach (GameEventBase @event in game.Events)
        {
            Console.WriteLine($"\t{@event}");
        }

        Console.WriteLine();
    }
}