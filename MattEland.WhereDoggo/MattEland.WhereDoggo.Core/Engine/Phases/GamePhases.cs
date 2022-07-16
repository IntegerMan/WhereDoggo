namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// Represents a phase within the game
/// </summary>
public enum GamePhases
{
    /// <summary>
    /// The initial setup of a game. Occurs before the night phase.
    /// </summary>
    Setup,
    /// <summary>
    /// The night phase involves waking roles waking up and performing their tasks in sequence
    /// </summary>
    Night,
    /// <summary>
    /// This phase encompasses discovering visible changes in the game board and AI Agents discussing their identities
    /// and trying to plan for voting
    /// </summary>
    Day,
    /// <summary>
    /// The final voting phase of the game where individual AI agents vote.
    /// </summary>
    Voting
}