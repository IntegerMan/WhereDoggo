using MattEland.WhereDoggo.Core.Roles;

static GameResult RunAndShowGame(bool showUi)
{
    RoleTypes[] assignedRoles =
    {
        // Player Roles
        RoleTypes.Mason,
        RoleTypes.Mason,
        RoleTypes.Werewolf,
        RoleTypes.Werewolf,
        RoleTypes.Villager,
        RoleTypes.ApprenticeSeer,
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
    game.PerformDayPhase();

    if (showUi)
    {
        game.DisplayNightActions();

        // Show game state prior to vote
        Console.WriteLine("Before voting...");
        Console.WriteLine();
        game.DisplayPlayerKnowledge(true);
    }

    // Carry out vote phase
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
    GameResult result = RunAndShowGame(true);

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