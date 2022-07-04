const int numPlayers = 4;

OneNightWhereDoggoGame game = new(numPlayers);
game.SetUp();

Console.WriteLine($"Starting a new game of \"{game.Name}\"");
Console.WriteLine();
game.Start();

Console.WriteLine("After game start...");
Console.WriteLine();
game.DisplayGameState();

// Carry out night phase
game.PerformNightPhase();
game.DisplayNightActions();

// Show game state prior to vote
Console.WriteLine("After night phase...");
Console.WriteLine();
game.DisplayPlayerKnowledge(true);

// Carry out vote phase

// Log all game events
game.DisplayAllEvents();