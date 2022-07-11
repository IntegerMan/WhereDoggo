namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Contains options on launching a <see cref="Game"/>
/// </summary>
public class GameOptions
{
    /// <summary>
    /// Determines whether or not the provided roles should be randomly assigned to players.
    /// This should be true except for testing purposes.
    /// </summary>
    public bool RandomizeSlots { get; set; } = true;
    
    /// <summary>
    /// Options related to the handling of the <see cref="RevealerRole"/>
    /// </summary>
    public ExposerOptions ExposerOptions { get; } = new();

    /// <summary>
    /// Gets the names for the players playing the game
    /// </summary>
    /// <remarks>
    /// It is safe to clear, add, and remove entries from this list before creating a game
    /// </remarks>
    public List<string> PlayerNames { get; } = new() { "Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane" };
}