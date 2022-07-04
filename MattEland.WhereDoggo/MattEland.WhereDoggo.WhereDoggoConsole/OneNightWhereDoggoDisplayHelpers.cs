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
    }
}