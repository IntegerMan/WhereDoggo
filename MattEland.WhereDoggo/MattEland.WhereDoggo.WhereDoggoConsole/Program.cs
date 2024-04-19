using MattEland.WhereDoggo.Core.Engine.Phases;
using MattEland.WhereDoggo.Core.Roles;
using Spectre.Console;

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
        AnsiConsole.MarkupLine($"Starting a new game of [Yellow]{game.Name}[/]");
        AnsiConsole.WriteLine();
    }

    GamePhaseBase phase;
    while (!game.RunNextPhase())
    {
        phase = game.CurrentPhase;

        if (!showUi) continue;
        
        if (phase.Name == "Night")
        {
            game.DisplayNightActions();
        }

        if (phase.Name != "Day")
        {
            AnsiConsole.MarkupLine($"After [Cyan]{phase}[/]...");
            game.DisplayGameState();
        }
        else
        {
            game.DisplayPlayerKnowledge(includeProbabilities: true);
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

AnsiConsole.MarkupLine($"After [Yellow]{numRuns}[/] runs, there were [Blue]{villageWins} village wins[/] and [Red]{wolfWins} wolf team[/] wins.");