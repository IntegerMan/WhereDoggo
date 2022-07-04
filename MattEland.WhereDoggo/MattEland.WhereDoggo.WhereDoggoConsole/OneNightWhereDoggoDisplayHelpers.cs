namespace MattEland.WhereDoggo.WhereDoggoConsole;

public static class OneNightWhereDoggoDisplayHelpers
{
    public static void DisplayGameState(this OneNightWhereDoggoGame game)
    {
        foreach (GamePlayer player in game.Players)
        {
            Console.WriteLine($"{player.Name} is a {player.InitialRole}");
        }

        int slotNumber = 1;
        foreach (GameRoleBase role in game.CenterRoles)
        {
            Console.WriteLine($"Center Slot #{slotNumber++} is holding {role}");
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

    public static void DisplayPlayerKnowledge(this OneNightWhereDoggoGame game)
    {
        foreach (GamePlayer player in game.Players)
        {
            player.DisplayPlayerKnowledge();
        }

        Console.WriteLine();
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