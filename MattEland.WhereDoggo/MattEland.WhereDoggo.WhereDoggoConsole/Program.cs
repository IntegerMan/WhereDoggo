// See https://aka.ms/new-console-template for more information

using MattEland.WhereDoggo.Core.Engine;
const int numPlayers = 4;

OneNightWhereDoggoGame game = new(numPlayers);

Console.WriteLine($"Starting a new game of \"{game.Name}\"");
Console.WriteLine();

Console.WriteLine("After game start...");
Console.WriteLine();
game.DisplayGameState();