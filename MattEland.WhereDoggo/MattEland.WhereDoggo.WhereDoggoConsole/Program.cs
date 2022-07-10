using MattEland.WhereDoggo.Core.Roles;

const int numPlayers = 3;

static GameResult RunAndShowGame(int numPlayers, bool showUi)
{
    RoleTypes[] assignedRoles =
    {
        // Player Roles
        RoleTypes.Insomniac,
        RoleTypes.Werewolf,
        RoleTypes.Werewolf,
        RoleTypes.Sentinel,
        RoleTypes.Villager,
        RoleTypes.Villager,
    };
    Game game = new(assignedRoles);

    if (showUi)
    {
        Console.WriteLine($"Starting a new game of \"{game.Name}\"");
        Console.WriteLine();
    }
    game.Start();

    if (showUi)
    {
        Console.WriteLine("After game start...");
        Console.WriteLine();
        game.DisplayGameState();
    }

    // Carry out night phase
    game.PerformNightPhase();

    if (showUi)
    {
        game.DisplayNightActions();

        // Show game state prior to vote
        Console.WriteLine("After night phase...");
        Console.WriteLine();
        game.DisplayPlayerKnowledge(true);
    }

    // Carry out vote phase
    game.PerformDayPhase();
    game.PerformVotePhase();

    // Log all game events
    if (showUi)
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