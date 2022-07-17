using MattEland.WhereDoggo.Core.Engine.Phases;
using MattEland.WhereDoggo.Core.Roles;

static GameResult RunAndShowGame(bool showUi)
{
    RoleTypes[] assignedRoles =
    {
        // Player Roles
        RoleTypes.Mason,
        RoleTypes.Mason,
        RoleTypes.Werewolf,
        RoleTypes.MysticWolf,
        RoleTypes.Villager,
        RoleTypes.ApprenticeSeer,
    };
    Game game = new(assignedRoles);

    if (showUi)
    {
        Console.WriteLine($"Starting a new game of \"{game.Name}\"");
        Console.WriteLine();
    }

    GamePhaseBase phase = game.CurrentPhase;
    while (!game.RunNextPhase())
    {
        phase = game.CurrentPhase;
        
        if (showUi)
        {
            if (phase.Name == "Night")
            {
                game.DisplayNightActions();
            }

            if (phase.Name != "Day")
            {
                Console.WriteLine($"After {phase} Phase...");
                game.DisplayGameState();
            }
            else
            {
                game.DisplayPlayerKnowledge(includeProbabilities: true);
            }
        }
    }

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