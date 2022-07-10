using MattEland.WhereDoggo.Core.Roles;

const int numPlayers = 3;

static GameResult RunAndShowGame(int numPlayers, bool showUI)
{
    Game game = new(numPlayers);
    GameRoleBase[] assignedRoles =
    {
        // Player Roles
        new InsomniacRole(),
        new WerewolfRole(),
        new VillagerRole(),
        // Center Cards
        new WerewolfRole(),
        new VillagerRole(),
        new VillagerRole()
    };
    game.SetUp(assignedRoles);

    if (showUI)
    {
        Console.WriteLine($"Starting a new game of \"{game.Name}\"");
        Console.WriteLine();
    }
    game.Start();

    if (showUI)
    {
        Console.WriteLine("After game start...");
        Console.WriteLine();
        game.DisplayGameState();
    }

    // Carry out night phase
    game.PerformNightPhase();

    if (showUI)
    {
        game.DisplayNightActions();

        // Show game state prior to vote
        Console.WriteLine("After night phase...");
        Console.WriteLine();
        game.DisplayPlayerKnowledge(true);
    }

    // Carry out vote phase
    game.PerformDayPhase();

    // Log all game events
    if (showUI)
    {
        game.DisplayAllEvents();
    }

    return game.Result!;
}

int numRuns = 1;
int villageWins = 0;
int wolfWins = 0;

for (int i = 0; i < numRuns; i++)
{
    GameResult result = RunAndShowGame(numPlayers, true);

    if (result.WerewolfKilled)
    {
        villageWins++;
    }
    else
    {
        wolfWins++;
    }
}

Console.WriteLine($"After {numRuns} runs, there were {villageWins} village wins and {wolfWins} wolf wins.");