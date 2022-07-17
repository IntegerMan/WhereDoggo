namespace MattEland.WhereDoggo.Core.Events.Claims;

/// <summary>
/// Represents a player claiming to be of a specific role
/// </summary>
public class RoleClaim : ClaimBase
{
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
    }
}