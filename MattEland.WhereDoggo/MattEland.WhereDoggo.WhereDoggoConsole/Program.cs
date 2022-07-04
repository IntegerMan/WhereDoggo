const int numPlayers = 4;

OneNightWhereDoggoGame game = new(numPlayers);

Console.WriteLine($"Starting a new game of \"{game.Name}\"");
Console.WriteLine();

Console.WriteLine("After game start...");
Console.WriteLine();
game.DisplayGameState();

// Carry out night phase
game.PerformNightPhase();

game.DisplayAllEvents();
game.DisplayPlayerKnowledge();

// Carry out vote phase