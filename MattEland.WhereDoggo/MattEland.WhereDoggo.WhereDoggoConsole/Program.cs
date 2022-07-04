const int numPlayers = 4;

OneNightWhereDoggoGame game = new(numPlayers);

Console.WriteLine($"Starting a new game of \"{game.Name}\"");
Console.WriteLine();
game.Start();

Console.WriteLine("After game start...");
Console.WriteLine();
game.DisplayGameState();

// Carry out night phase
game.PerformNightPhase();
Console.WriteLine("After night phase...");
Console.WriteLine();

// Show game state prior to vote
game.DisplayPlayerKnowledge();

// Carry out vote phase

// Log all game events
game.DisplayAllEvents();