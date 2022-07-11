namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Options related to the <see cref="ExposerRole"/>
/// </summary>
public class ExposerOptions
{
    /// <summary>
    /// The chance that the <see cref="ExposerRole"/> will reveal 1 card.
    /// </summary>
    public int OneCardChance { get; set; } = 75;
    /// <summary>
    /// The chance that the <see cref="ExposerRole"/> will reveal 2 cards.
    /// </summary>
    public int TwoCardChance { get; set; } = 20;
    /// <summary>
    /// The chance that the <see cref="ExposerRole"/> will reveal 3 cards.
    /// </summary>
    public int ThreeCardChance { get; set; } = 5;

    /// <summary>
    /// Calculates the number of cards that should be exposed based on the probabilities
    /// </summary>
    /// <param name="random">The randomizer</param>
    /// <returns>The number of cards. This will be 1, 2, or 3</returns>
    public int DetermineCardsToExpose(Random random)
    {
        // Build a list of options based on probability
        List<int> choices = new();
        for (int i=0; i<OneCardChance; i++)
        {
            choices.Add(1);
        }
        for (int i=0; i < TwoCardChance; i++)
        {
            choices.Add(2);
        }
        for (int i=0; i < ThreeCardChance; i++)
        {
            choices.Add(3);
        }

        // Get and return a random element
        return choices.GetRandomElement(random);
    }

    /// <summary>
    /// Forces the number of cards that the <see cref="ExposerRole"/> reveals to be <paramref name="numCards"/>
    /// </summary>
    /// <param name="numCards">The number of cards exposed, from 1 to 3</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="numCards"/> was not 1, 2, or 3</exception>
    public void ForceNumberOfCardsRevealed(int numCards)
    {
        switch (numCards)
        {
            case 1:
                OneCardChance = 100;
                TwoCardChance = 0;
                ThreeCardChance = 0;
                break;
            case 2:
                OneCardChance = 0;
                TwoCardChance = 100;
                ThreeCardChance = 0;
                break;
            case 3:
                OneCardChance = 0;
                TwoCardChance = 0;
                ThreeCardChance = 100;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(numCards), "Number of cards must be between 1 and 3");
        }
    }
}